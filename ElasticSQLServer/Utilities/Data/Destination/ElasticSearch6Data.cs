using ElasticSQLServer.Contracts.Data;
using ElasticSQLServer.Utilities.Data.Source;
using ElasticSQLServer.Utilities.Factory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities.Data.Destination
{
    /// <summary>
    /// Elasticsearch data.
    /// </summary>
    public class ElasticSearch6Data : IDataDestination
    {
        private string elasticHost = "";
        private string elasticIndex = "";
        private string url = "";
        private string documentUrl = "";
        private string hashRecord = "";
        private string documentId = "";
        private byte[] hash;
        private int counter = 1;
        private List<string> records;
        private StringBuilder builder;
        private StringBuilder recordBuilder;
        private StringBuilder jsonBuilder;

        /// <summary>
        /// Elasticsearch data constructor.
        /// </summary>
        public ElasticSearch6Data()
        {
            elasticHost = Environment.GetEnvironmentVariable("ElasticHost");
            elasticIndex = Environment.GetEnvironmentVariable("ElasticIndex");
            url = elasticHost + "/" + elasticIndex;
            documentUrl = url + "/" + Environment.GetEnvironmentVariable("ElasticDocument");
            records = new List<string>();
            builder = new StringBuilder();
            recordBuilder = new StringBuilder();
            jsonBuilder = new StringBuilder();
        }

        /// <summary>
        /// Creating Elasticsearch index.
        /// </summary>
        public async Task CreateIndex()
        {
            var result = await new SQLServerData().GetDataTypes();
            var content = new Index().CreateIndexJson(result);

            using (HttpContent httpContent = new StringContent(content))
            {
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await new ElasticClient().ElasticPut(url, httpContent);
            }
        }

        /// <summary>
        /// Converting source data to Elasticsearch data and saving it if the data is new.
        /// </summary>
        /// <param name="sqlData"></param>
        public async Task InsertIntoDocument(DataTable sqlData)
        {
            try
            {
                var rows = sqlData.Rows;
                var columns = sqlData.Columns;

                builder.Append("{\"" + elasticIndex + "\":[");

                char[] value = { '{' };

                for (int i = 0; i <= rows.Count - 1; i++)
                {
                    StringBuilder columnBuilder = new StringBuilder();

                    columnBuilder.Append("{");

                    for (int j = 0; j <= columns.Count; j++)
                    {
                        if (j != columns.Count)
                        {
                            var columnValue = columns[j].ColumnName.ToLower();
                            var cellValue = rows[i].ItemArray[j];

                            records.Add($"\"{columnValue}\":\"{cellValue}\"");

                            columnBuilder.Append($"\"{columnValue}\":\"{cellValue}\",");

                            recordBuilder.Append($"{cellValue}");
                            recordBuilder.Append(",");
                        }

                        else
                        {
                            using (MD5 md5 = MD5.Create())
                            {
                                md5.Initialize();
                                md5.ComputeHash(Encoding.UTF8.GetBytes(recordBuilder.ToString()));
                                hash = md5.Hash;
                            }

                            StringBuilder hashBuilder = new StringBuilder();

                            foreach (var hashValue in hash)
                            {
                                hashBuilder.Append(hashValue.ToString("x2"));
                            }

                            hashRecord = hashBuilder.ToString();

                            if (!(await HashExist(documentId + "/_search", hashRecord)))
                            {

                                columnBuilder.Append($"\"hash\" : \"{hashRecord}\"");
                                columnBuilder.Append("}");
                                builder.Append(columnBuilder.ToString());
                                counter++;

                                if (i != rows.Count - 1)
                                {
                                    builder.Append(",");
                                }
                            }
                        }
                    }
                }

                builder.Append("]}");

                jsonBuilder.Append(builder.ToString());

                string jsonResult = jsonBuilder.ToString();

                int open = jsonResult.IndexOf('[', jsonResult.Length - 3);
                int close = jsonResult.IndexOf(']', jsonResult.Length - 2);

                if (open == -1 && close >= 0)
                {
                    using (HttpContent httpContent = new StringContent(jsonResult))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        await new ElasticClient().ElasticPost(documentUrl, httpContent);

                        Console.WriteLine($"New records are inserted into document at: {DateTime.Now.ToString()}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Getting Elasticsearch document id.
        /// </summary>
        /// <returns></returns>
        private async Task GetDocumentId()
        {
            string response = "";

            StringBuilder builder = new StringBuilder();
            response = await new ElasticClient().ElasticGet(documentUrl + "/_search");
            documentId = JsonConvert.DeserializeObject<dynamic>(response)._id;
        }

        private async Task<bool> HashExist(string documentId, string hash)
        {
            try
            {
                await GetDocumentId();

                StringBuilder builder = new StringBuilder();

                builder.Append("{\"query\": { \"query_string\": {\"query\" : \"");
                builder.Append($"{hash}\"");
                builder.Append("}}}");

                using (HttpContent httpContent = new StringContent(builder.ToString()))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return await new ElasticClient().ElasticGetBool(documentId + "/_search?pretty=true", httpContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

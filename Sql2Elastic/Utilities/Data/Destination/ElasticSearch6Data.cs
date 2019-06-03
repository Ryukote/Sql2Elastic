using Sql2Elastic.Utilities.Data.Source;
using Sql2Elastic.Contracts.Data;
using Sql2Elastic.Utilities.Factory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sql2Elastic.Utilities.Data.Destination
{
    /// <summary>
    /// Elasticsearch data.
    /// </summary>
    public class ElasticSearch6Data : IDataDestination
    {
        private string dbType = "";
        private string elasticHost = "";
        private string elasticIndex = "";
        private string elasticDocument = "";
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

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Elasticsearch data constructor.
        /// </summary>
        public ElasticSearch6Data(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            dbType = _configuration.GetValue<string>("DbType");
            elasticHost = _configuration.GetValue<string>("ElasticHost");
            elasticIndex = _configuration.GetValue<string>("ElasticIndex");
            elasticDocument = _configuration.GetValue<string>("ElasticDocument");

            url = elasticHost + "/" + elasticIndex;
            documentUrl = url + "/" + elasticDocument;
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
            try
            {
                IEnumerable<Tuple<string, string>> result;

                if (dbType.Equals("Postgres"))
                {
                    result = await new PostgresData(_configuration).GetDataTypes();
                }

                else if (dbType.Equals("SQLServer"))
                {
                    result = await new SQLServerData(_configuration).GetDataTypes();
                }

                else
                {
                    result = null;
                }

                var content = new Index().CreateIndexJson(result, _configuration);

                using (HttpContent httpContent = new StringContent(content))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    await new ElasticClient().ElasticPut(url, httpContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Converting source data to Elasticsearch data and saving it if the data is new.
        /// </summary>
        /// <param name="sqlData">DataTable representing data from SQL database.</param>
        public async Task InsertIntoDocument(DataTable sqlData)
        {
            builder.Clear();
            recordBuilder.Clear();
            jsonBuilder.Clear();

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
        private async Task GetDocumentId()
        {
            try
            {
                string response = "";

                StringBuilder builder = new StringBuilder();
                response = await new ElasticClient().ElasticGet(documentUrl + "/_search");
                documentId = JsonConvert.DeserializeObject<dynamic>(response)._id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Check if hash exist in the document to avoid storing duplicate records.
        /// </summary>
        /// <param name="documentId">Name of document.</param>
        /// <param name="hash">Hash to check in the document.</param>
        /// <returns>Returns if hash exist or not.</returns>
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
                    return await new ElasticClient().ElasticGetBool(documentUrl + documentId + "?pretty=true", httpContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
    }
}

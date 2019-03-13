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
    /// 
    /// </summary>
    public class ElasticSearch6Data : IDataDestination
    {
        private string elasticHost = "";
        private string elasticIndex = "";
        private string url = "";
        private string documentUrl = "";
        private string documentIdQuery = "";
        private string hashRecord = "";
        private string documentId = "";
        private byte[] hash;

        /// <summary>
        /// 
        /// </summary>
        public ElasticSearch6Data()
        {
            elasticHost = Environment.GetEnvironmentVariable("ElasticHost");
            elasticIndex = Environment.GetEnvironmentVariable("ElasticIndex");
            url = elasticHost + "/" + elasticIndex;
            documentUrl = url + "/" + Environment.GetEnvironmentVariable("ElasticDocument");
            documentIdQuery = documentUrl + "/_search?pretty=true";
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task CreateIndex()
        {
            var content = new Index().CreateIndexJson(await new SQLServerData().GetDataTypes());

            using (HttpContent httpContent = new StringContent(content))
            {
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await new ElasticClient().ElasticPut(url, httpContent);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlData"></param>
        public async Task InsertIntoDocument(DataTable sqlData)
        {
            try
            {
                List<dynamic> list = new List<dynamic>();

                var rows = sqlData.Rows;
                var columns = sqlData.Columns;

                StringBuilder builder = new StringBuilder();
                StringBuilder recordBuilder = new StringBuilder();

                //{elasticIndex}
                builder.Append("{\"testindex\":[");

                for (int i = 0; i < rows.Count; i++)
                {
                    builder.Append("{");

                    StringBuilder columnBuilder = new StringBuilder();

                    for (int j = 0; j < columns.Count + 1; j++)
                    {
                        columnBuilder.Append($"\"{columns[j].ColumnName.ToLower()}\":\"{rows[i].ItemArray[j]}\"");
                        recordBuilder.Append($"{rows[i].ItemArray[j]}");

                        if (j != columns.Count - 1)
                        {
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

                            if(await HashExist(documentId, hash.ToString()))
                            {
                                columnBuilder.Append("}");
                                builder.Append(columnBuilder.ToString());
                                builder.Append($"\"hash\" : \"{hash.ToString()}\"");
                            }
                        }
                    }

                    if (i != rows.Count - 1)
                    {
                        builder.Append(",");
                    }

                    else
                    {
                        builder.Append("]}");
                    }
                }

                using (HttpContent httpContent = new StringContent(builder.ToString()))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    await new ElasticClient().ElasticPost(documentUrl, httpContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong.");
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetDocumentId()
        {
            string response = "";

            StringBuilder builder = new StringBuilder();
            builder.Append("\"size\":1,");
            builder.Append("\"query\":{");
            builder.Append("\"match_all\":{}");
            builder.Append("}");

            using (HttpContent httpContent = new StringContent(builder.ToString()))
            {
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await new ElasticClient().ElasticGet(documentIdQuery, httpContent);
                documentId = JsonConvert.DeserializeObject<dynamic>(response)[3]._id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        private async Task<bool> HashExist(string documentId, string hash)
        {
            try
            {
                await GetDocumentId();

                StringBuilder builder = new StringBuilder();

                builder.Append("{\"query\": { \"match\": {\"hash\" : \"");
                builder.Append($"{hash.ToString()}");

                using (HttpContent httpContent = new StringContent(builder.ToString()))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return await new ElasticClient().ElasticGetBool(documentIdQuery, httpContent);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

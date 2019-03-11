using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// Class for Elasticsearch.
    /// </summary>
    public class Elasticsearch
    {
        private string elasticHost = "";
        private string elasticIndex = "";
        private string url = "";
        private string documentUrl = "";

        /// <summary>
        /// Elasticsearch constructor.
        /// </summary>
        public Elasticsearch()
        {
            elasticHost = Environment.GetEnvironmentVariable("ElasticHost");
            elasticIndex = Environment.GetEnvironmentVariable("ElasticIndex");
            url = elasticHost + "/" + elasticIndex;
            documentUrl = url + "/" + Environment.GetEnvironmentVariable("ElasticDocument");
        }

        /// <summary>
        /// Creating Elasticsearch index based on given SQL Server table.
        /// </summary>
        /// <param name="sqlResult">Data selected from ADO.NET query.</param>
        public async Task CreateIndexAsync(DataTable sqlResult)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpContent httpContent = new StringContent(DynamicObjects.ElasticIndexMappingReflection(sqlResult)))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage responseMessage = client.PutAsync(url, httpContent).Result;

                        string message = await responseMessage.Content.ReadAsStringAsync();

                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            Console.WriteLine("Elasticsearch index is created.");
                            Console.WriteLine(message);
                        }

                        else
                        {
                            Console.WriteLine("Something went wrong. Check the response message from Elasticsearch.");
                            Console.WriteLine(message);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Something went wrong.");
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong.");
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Writting data to Elasticsearch index.
        /// </summary>
        /// <param name="sqlResult">Data selected from ADO.NET query.</param>
        public async Task WriteToElasticsearchAsync(DataTable sqlResult)
        {
            try
            {
                string jsonResult = JsonConvert.SerializeObject(sqlResult);

                List<dynamic> list = new List<dynamic>();

                var rows = sqlResult.Rows;
                var columns = sqlResult.Columns;

                string json = "{\"" + $"{elasticIndex}" + ":[";

                for (int i = 0; i < rows.Count - 1; i++)
                {
                    json += "{";

                    for (int j = 0; j < columns.Count - 1; j++)
                    {
                        json += $"\"{columns[j].ColumnName.ToLower()}\":\"{rows[i].ItemArray[j]}\"";

                        if (j != columns.Count - 2)
                        {
                            json += ",";
                        }

                        else
                        {
                            json += "}";
                        }
                    }

                    if (i != rows.Count - 2)
                    {
                        json += ",";
                    }

                    else
                    {
                        json += "]}";
                    }
                }

                using (HttpClient client = new HttpClient())
                {
                    using (HttpContent httpContent = new StringContent(json))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = client.PostAsync(documentUrl, httpContent).Result;
                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Something went wrong.");
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong.");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// Class for Elasticsearch.
    /// </summary>
    public class Elasticsearch
    {
        private string elasticHost = "";

        /// <summary>
        /// Elasticsearch constructor.
        /// </summary>
        public Elasticsearch()
        {
            elasticHost = Environment.GetEnvironmentVariable("ElasticHost");
        }

        /// <summary>
        /// Creating Elasticsearch index based on given SQL Server table. This method is private.
        /// </summary>
        /// <param name="sqlResult">Data selected from raw sql.</param>
        /// <returns></returns>
        private string CreateIndex(IEnumerable<dynamic> sqlResult)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    using (Stream data = client.OpenRead(new Uri(DynamicObjects.ElasticIndexMappingReflection(sqlResult))))
                    {
                        client.UploadData(new Uri(elasticHost), "PUT", StreamToByteArray(data));
                    }
                }

                return "Elasticsearch index is created.";
            }
            catch(Exception ex)
            {
                return ex.StackTrace;
            }
        }

        /// <summary>
        /// Writting data to Elasticsearch index.
        /// </summary>
        /// <param name="sqlResult">Data selected from raw sql.</param>
        /// <returns></returns>
        private string WriteToElasticsearch(IEnumerable<dynamic> sqlResult)
        {
            try
            {
                string jsonResult = JsonConvert.SerializeObject(sqlResult);

                using (WebClient client = new WebClient())
                {
                    using (Stream data = client.OpenRead(new Uri(jsonResult)))
                    {
                        client.UploadData(new Uri(elasticHost), "PUT", StreamToByteArray(data));
                    }
                }

                return "Data from SQL Server is posted to Elasticsearch.";
            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }
        }

        /// <summary>
        /// Converting stream to byte array. This method is private.
        /// </summary>
        /// <param name="input">Stream representation of json data from SQL Server result.</param>
        /// <returns></returns>
        private byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Asynchronous version of writting data to Elasticsearch index.
        /// </summary>
        /// <param name="sqlResult">Data selected from raw sql.</param>
        /// <returns></returns>
        public async Task<string> WriteToElasticsearchAsync(dynamic sqlResult)
        {
            return await Task.Run(() => WriteToElasticsearchAsync(sqlResult));
        }

        /// <summary>
        /// Asynchronous version of creating Elasticsearch index.
        /// </summary>
        /// <param name="sqlResult">Data selected from raw sql.</param>
        /// <returns></returns>
        public async Task<string> CreateIndexAsync(IEnumerable<dynamic> sqlResult)
        {
            return await Task.Run(() => CreateIndex(sqlResult));
        }
    }
}

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// 
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
        
        private string CreateElasticIndex()
        {
            return "";
        }

        private string WriteToElasticsearch(dynamic sqlResult)
        {
            try
            {
                string jsonResult = JsonConvert.SerializeObject(sqlResult);

                using (WebClient client = new WebClient())
                {
                    using (Stream data = client.OpenRead(new Uri(jsonResult)))
                    {
                        client.UploadData(new Uri(elasticHost), StreamToByteArray(data));
                    }
                }

                return "Data from SQL Server is posted to Elasticsearch.";
            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }
        }

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
        /// 
        /// </summary>
        /// <param name="sqlResult"></param>
        /// <returns></returns>
        public async Task<string> WriteToElasticsearchAsync(dynamic sqlResult)
        {
            return await Task.Run(() => WriteToElasticsearchAsync(sqlResult));
        }
    }
}

using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ElasticSQLServer.Contracts.Data;

namespace ElasticSQLServer.Utilities.Data.Destination
{
    class ElasticSearch6Data : IDataDestination
    {
        private string elasticHost = "";
        private string elasticIndex = "";
        private string url = "";
        private string documentUrl = "";

        public ElasticSearch6Data()
        {
            elasticHost = Environment.GetEnvironmentVariable("ElasticHost");
            elasticIndex = Environment.GetEnvironmentVariable("ElasticIndex");
            url = elasticHost + "/" + elasticIndex;
            documentUrl = url + "/" + Environment.GetEnvironmentVariable("ElasticDocument");
        }

        public Task CreateDocument()
        {
            throw new System.NotImplementedException();
        }

        public async Task CreateIndex(DataTable sqlData)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpContent httpContent = new StringContent(DynamicObjects.ElasticIndexMappingReflection(sqlData)))
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

        public Task DeleteDocument()
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteIndex()
        {
            throw new System.NotImplementedException();
        }

        public Task InsertIntoDocument()
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateDocument()
        {
            throw new System.NotImplementedException();
        }
    }
}

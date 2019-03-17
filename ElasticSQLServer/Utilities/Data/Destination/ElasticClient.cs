using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities.Data.Destination
{
    /// <summary>
    /// Elasticsearch client.
    /// </summary>
    public class ElasticClient
    {
        /// <summary>
        /// Elasticsearch with get method.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ElasticGet(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    string value = await response.Content.ReadAsStringAsync();

                    return value;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    return "";
                }
            }
        }

        /// <summary>
        /// Checking if record exist in Elasticsearch document.
        /// </summary>
        /// <param name="url">Where method will check if record exist.</param>
        /// <param name="httpContent">Record that will be checked for existance.</param>
        /// <returns></returns>
        public async Task<bool> ElasticGetBool(string url, HttpContent httpContent)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string newUrl = url.Split('?')[0];

                    HttpResponseMessage response = await client.PostAsync(newUrl, httpContent);

                    var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                    return result.hits.total == 0 ? false : true;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    return true;
                }
            }
        }

        /// <summary>
        /// Elasticsearch post method.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ElasticPost(string url, HttpContent httpContent)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, httpContent);

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    return "";
                }
            }
        }

        /// <summary>
        /// Elasticsearch put method.
        /// </summary>
        /// <returns></returns>
        public async Task ElasticPut(string url, HttpContent httpContent)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PutAsync(url, httpContent);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }
    }
}

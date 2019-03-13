using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities.Data.Destination
{
    /// <summary>
    /// 
    /// </summary>
    public class ElasticClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ElasticGet(string url, HttpContent httpContent)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Content = httpContent;

                    //Environment.GetEnvironmentVariable("ElasticIndex")
                    HttpResponseMessage response = client.SendAsync(request).Result;

                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ElasticGetBool(string url, HttpContent httpContent)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Content = httpContent;

                    //Environment.GetEnvironmentVariable("ElasticIndex")
                    HttpResponseMessage response = client.SendAsync(request).Result;

                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ElasticPost(string url, HttpContent httpContent)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //Environment.GetEnvironmentVariable("ElasticIndex")
                    HttpResponseMessage response = client.PostAsync(url, httpContent).Result;

                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ElasticPut(string url, HttpContent httpContent)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //Environment.GetEnvironmentVariable("ElasticIndex")
                    HttpResponseMessage response = client.PutAsync(url, httpContent).Result;

                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ElasticDelete(string url, HttpContent httpContent)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //Environment.GetEnvironmentVariable("ElasticIndex")
                    var content = new HttpRequestMessage(HttpMethod.Delete, new Uri(url));
                    content.Content = httpContent;

                    HttpResponseMessage response = client.SendAsync(content).Result;

                    Console.WriteLine(await response.Content.ReadAsStringAsync());
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

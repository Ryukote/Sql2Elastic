using ElasticSQLServer.Utilities;
using System;
using System.Timers;

namespace ElasticSQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int hookInterval = Convert.ToInt32(Environment.GetEnvironmentVariable("HookInterval"));

            Timer timer = new Timer
            {
                Interval = hookInterval,
                Enabled = true
            };

            timer.Elapsed += async(sender, e) =>
            {
                Database database = new Database();
                dynamic dynamicData = await database.GetDynamicDataAsync();
                Elasticsearch elasticSearch = new Elasticsearch();
                Console.WriteLine(await elasticSearch.CreateIndexAsync(dynamicData));
                Console.WriteLine(await elasticSearch.WriteToElasticsearchAsync(dynamicData));
            };
        }
    }
}

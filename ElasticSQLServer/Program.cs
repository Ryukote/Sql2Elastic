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

            Timer timer = new Timer();

            timer.Interval = hookInterval;

            timer.Enabled = true;
            
            timer.Elapsed += async(sender, e) =>
            {
                var database = new Database();
                var elasticSearch = new Elasticsearch();
                await elasticSearch.WriteToElasticsearchAsync(await database.GetDynamicDataAsync());
            };
        }
    }
}

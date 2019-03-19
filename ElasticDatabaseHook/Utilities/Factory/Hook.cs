using ElasticSQLServer.Utilities.Data.Destination;
using ElasticSQLServer.Utilities.Data.Source;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities.Factory
{
    /// <summary>
    /// Hook with database and Elasticsearch.
    /// </summary>
    public class Hook
    {
        SQLServerData serverData;
        ElasticSearch6Data elasticSearch;

        /// <summary>
        /// Hook constructor.
        /// </summary>
        public Hook(IConfiguration configuration)
        {
            serverData = new SQLServerData(configuration);
            elasticSearch = new ElasticSearch6Data(configuration);
        }

        /// <summary>
        /// Iteration process for inserting records into document.
        /// </summary>
        /// <returns></returns>
        public async Task IterationProcess()
        {
            try
            {
                await elasticSearch.InsertIntoDocument(await serverData.GetDatabaseDataAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Index creation process.
        /// </summary>
        /// <returns></returns>
        public async Task StartProcess()
        {
            try
            {
                await elasticSearch.CreateIndex();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}

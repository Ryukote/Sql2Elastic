using ElasticSQLServer.Utilities.Data.Destination;
using ElasticSQLServer.Utilities.Data.Source;
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
        public Hook()
        {
            serverData = new SQLServerData();
            elasticSearch = new ElasticSearch6Data();
        }

        /// <summary>
        /// Iteration process for inserting records into document.
        /// </summary>
        /// <returns></returns>
        public async Task IterationProcess()
        {
            await elasticSearch.InsertIntoDocument(await serverData.GetDatabaseDataAsync());
        }

        /// <summary>
        /// Index creation process.
        /// </summary>
        /// <returns></returns>
        public async Task StartProcess()
        {
            await elasticSearch.CreateIndex();
        }
    }
}

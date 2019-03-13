using ElasticSQLServer.Utilities.Data.Destination;
using ElasticSQLServer.Utilities.Data.Source;
using System.Data;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class Hook
    {
        SQLServerData serverData;
        DataTable dataTable;
        ElasticSearch6Data elasticSearch;

        /// <summary>
        /// 
        /// </summary>
        public Hook()
        {
            serverData = new SQLServerData();
            dataTable = new DataTable();
            elasticSearch = new ElasticSearch6Data();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task IterationProcess()
        {
            await elasticSearch.InsertIntoDocument(await serverData.GetDatabaseDataAsync());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task StartProcess()
        {
            await elasticSearch.CreateIndex();
        }
    }
}

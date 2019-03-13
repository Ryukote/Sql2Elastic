using System.Data;
using System.Threading.Tasks;

namespace ElasticSQLServer.Contracts.Data
{
    /// <summary>
    /// Elasticsearch index and document operations.
    /// </summary>
    public interface IDataDestination
    {
        /// <summary>
        /// Creating Elasticsearch index.
        /// </summary>
        /// <returns></returns>
        Task CreateIndex();

        /// <summary>
        /// Inserting new records in the Elasticsearch document in the index.
        /// </summary>
        /// <returns></returns>
        Task InsertIntoDocument(DataTable sqlData);
    }
}

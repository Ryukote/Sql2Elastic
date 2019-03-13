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
        Task CreateIndex(DataTable sqlData);

        /// <summary>
        /// Delete existing Elasticsearch index.
        /// </summary>
        /// <returns></returns>
        Task DeleteIndex();

        /// <summary>
        /// Creating Elasticsearch document in the index.
        /// </summary>
        /// <returns></returns>
        Task CreateDocument();

        /// <summary>
        /// Delete existing Elasticsearch document in the index.
        /// </summary>
        /// <returns></returns>
        Task DeleteDocument();

        /// <summary>
        /// Updating existing Elasticsearch document in the index.
        /// </summary>
        /// <returns></returns>
        Task UpdateDocument();

        /// <summary>
        /// Inserting new records in the Elasticsearch document in the index.
        /// </summary>
        /// <returns></returns>
        Task InsertIntoDocument();
    }
}

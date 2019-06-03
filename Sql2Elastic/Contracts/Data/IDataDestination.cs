using System.Data;
using System.Threading.Tasks;

namespace Sql2Elastic.Contracts.Data
{
    /// <summary>
    /// Elasticsearch index and document operations.
    /// </summary>
    public interface IDataDestination
    {
        /// <summary>
        /// Creating Elasticsearch index.
        /// </summary>
        Task CreateIndex();

        /// <summary>
        /// Inserting new records in the Elasticsearch document in the index.
        /// </summary>
        Task InsertIntoDocument(DataTable sqlData);
    }
}

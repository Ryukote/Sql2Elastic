using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ElasticSQLServer.Contracts.Data
{
    /// <summary>
    /// Getting table column names, data types and values.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Get data types from table in a database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tuple<string, string>>> GetDataTypes();

        /// <summary>
        /// Get values from table in a database.
        /// </summary>
        /// <returns></returns>
        Task<DataTable> GetDatabaseDataAsync();
    }
}

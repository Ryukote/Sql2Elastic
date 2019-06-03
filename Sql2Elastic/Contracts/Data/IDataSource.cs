using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Sql2Elastic.Contracts.Data
{
    /// <summary>
    /// Getting table column names, data types and values.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Get data types from table in a database.
        /// </summary>
        /// <returns>Returns collection of string Tuple object representing data type for each column.</returns>
        Task<IEnumerable<Tuple<string, string>>> GetDataTypes();

        /// <summary>
        /// Get values from table in a database.
        /// </summary>
        /// <returns>Returns new DataTable object filled with data from database.</returns>
        Task<DataTable> GetDatabaseDataAsync();
    }
}

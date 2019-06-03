using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Sql2Elastic.Contracts.Data
{
    /// <summary>
    /// Create json for creating index.
    /// </summary>
    public interface IIndexJson
    {
        /// <summary>
        /// Create json for creating index.
        /// </summary>
        /// <returns>Returns collection of string Tuple object that represent index parameters.</returns>
        string CreateIndexJson(IEnumerable<Tuple<string, string>> indexData, IConfiguration configuration);
    }
}

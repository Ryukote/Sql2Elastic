using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ElasticSQLServer.Contracts.Data
{
    /// <summary>
    /// Create json for creating index.
    /// </summary>
    public interface IIndexJson
    {
        /// <summary>
        /// Create json for creating index.
        /// </summary>
        /// <returns></returns>
        string CreateIndexJson(IEnumerable<Tuple<string, string>> indexData, IConfiguration configuration);
    }
}

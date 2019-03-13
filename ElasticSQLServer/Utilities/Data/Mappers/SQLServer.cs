using ElasticSQLServer.Contracts.Mapping;
using System.Collections.Generic;

namespace ElasticSQLServer.Utilities.Data.Mappers
{
    /// <summary>
    /// Mapping SQL Server data type to Elasticsearch data type.
    /// </summary>
    public class SQLServer : IMapper
    {
        /// <summary>
        /// Method for mapping SQL Server data type to Elasticsearch data type.
        /// </summary>
        /// <param name="key">Value that represents SQL Server data type reflected into C# data type.</param>
        /// <returns>Returs transformed data type from reflected SQL Server column in the table to Elasticsearch equivalent data type.</returns>
        public string GetMappedType(string key)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("varchar", "text");
            dictionary.Add("int", "integer");
            dictionary.Add("decimal", "double");
            dictionary.Add("numeric", "double");
            dictionary.Add("smallint", "short");
            dictionary.Add("tinyint", "byte");
            dictionary.Add("bigint", "long");
            dictionary.Add("bit", "boolean");
            dictionary.Add("float", "float");
            dictionary.Add("date", "date");
            dictionary.Add("datetime", "date");
            dictionary.Add("time", "date");
            dictionary.Add("text", "text");
            dictionary.Add("char", "text");
            dictionary.Add("nvarchar", "text");
            dictionary.Add("ntext", "text");
            dictionary.Add("binary", "byte");
            dictionary.Add("varbinary", "byte");

            if (dictionary.ContainsKey(key))
            {
                return dictionary.GetValueOrDefault(key);
            }

            return "";
        }
    }
}

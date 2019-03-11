using System.Collections.Generic;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// Class for mapping SQL Server data types to Elasticsearch data types.
    /// </summary>
    public static class Mapping
    {
        /// <summary>
        /// Method for mapping SQL Server data type to Elasticsearch data type.
        /// </summary>
        /// <param name="key">Value that represents SQL Server data type reflected into C# data type.</param>
        /// <returns>Returs transformed data type from reflected SQL Server column in the table to Elasticsearch equivalent data type.</returns>
        public static string GetMappedTypeValue(string key)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("String", "text");
            dictionary.Add("Int", "integer");

            if (dictionary.ContainsKey(key))
            {
                return dictionary.GetValueOrDefault(key);
            }

            return "";
        }
    }
}

using Sql2Elastic.Contracts.Mapping;
using System.Collections.Generic;

namespace Sql2Elastic.Utilities.Data.Mappers
{
    /// <summary>
    /// Mapping Postgres data type to Elasticsearch data type.
    /// </summary>
    public class Postgres : IMapper
    {
        /// <summary>
        /// Method for mapping Postgres data type to Elasticsearch data type.
        /// </summary>
        /// <param name="key">Value that represents Postgres data type.</param>
        /// <returns>Returs transformed data type from Postgres column in the table to Elasticsearch equivalent data type.</returns>
        public string GetMappedType(string key)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("smallint", "short");
            dictionary.Add("int", "integer");
            dictionary.Add("integer", "integer");
            dictionary.Add("bigint", "long");
            dictionary.Add("decimal", "double");
            dictionary.Add("numeric", "double");
            dictionary.Add("real", "double");
            dictionary.Add("double", "double");
            dictionary.Add("smallserial", "short");
            dictionary.Add("serial", "integer");
            dictionary.Add("bigserial", "long");
            dictionary.Add("money", "float");
            dictionary.Add("character", "text");
            dictionary.Add("varying", "text");
            dictionary.Add("varchar", "text");
            dictionary.Add("char", "text");
            dictionary.Add("text", "text");
            dictionary.Add("bytea", "byte");
            dictionary.Add("timestamp", "date");
            dictionary.Add("date", "date");
            dictionary.Add("time", "date");
            dictionary.Add("boolean", "boolean");
            dictionary.Add("bool", "boolean");
            dictionary.Add("enum", "text");
            dictionary.Add("cidr", "ip");
            dictionary.Add("inet", "ip");
            dictionary.Add("macaddr", "text");
            dictionary.Add("bit", "text");
            dictionary.Add("tsvector", "text");
            dictionary.Add("uuid", "text");
            dictionary.Add("xml", "text");
            dictionary.Add("json", "object");

            if (dictionary.ContainsKey(key))
            {
                return dictionary.GetValueOrDefault(key);
            }

            return "";
        }
    }
}

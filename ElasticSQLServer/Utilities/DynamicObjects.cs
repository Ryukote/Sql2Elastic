using System;
using System.Collections.Generic;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// Class for working with dynamic objects.
    /// </summary>
    public static class DynamicObjects
    {
        /// <summary>
        /// Method for getting database table column names and mapping their data types to valid Elasticsearch data types using reflection. This method is preparing json for creating a new index in Elasticsearch.
        /// </summary>
        /// <param name="obj">Data from SQL Server raw query result that needs to be reflected for data types mapping.</param>
        /// <returns></returns>
        public static string ElasticIndexMappingReflection(IEnumerable<object> obj)
        {
            var properties = obj.GetType().GetProperties();

            string mapping = "\"settings\":{\"number_of_shards\"" +
                $"{Environment.GetEnvironmentVariable("ShardNum")}" +
                "}," +
                "\"mappings\" : {" +
                "\"_doc\" : {" +
                "\"properties\" : {";

            for(int i = 0; i < properties.Length; i++)
            {
                mapping += $"\"{properties[i].Name.ToLower()}\" : " + "{ \"type\" : " + 
                    $"{Mapping.GetMappedTypeValue(properties[i].GetType().ToString())}";

                if (i != properties.Length - 1)
                {
                    mapping += ",";    
                }
            }

            return mapping += "}}}}";
        }
    }
}

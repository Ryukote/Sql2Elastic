using System;
using System.Data;

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
        /// <param name="obj">Data from SQL Server data table result that needs to be reflected for data types mapping.</param>
        /// <returns>Returns prepared json string that defines mapping for specific document in index.</returns>
        public static string ElasticIndexMappingReflection(DataTable obj)
        {
            var properties = obj.Columns;

            string mapping = "\"settings\":{\"number_of_shards\"" +
                $"{Environment.GetEnvironmentVariable("ShardNum")}" +
                "}," +
                "\"mappings\" : {" +
                $"\"{Environment.GetEnvironmentVariable("ElasticDocument")}\"" + " : {" +
                "\"properties\" : {";

            for(int i = 0; i < properties.Count; i++)
            {
                mapping += $"\"{properties[i].ColumnName.ToLower()}\" : " + "{ \"type\" : " + 
                    $"{Mapping.GetMappedTypeValue(properties[i].GetType().ToString())}";

                if (i != properties.Count - 1)
                {
                    mapping += ",";    
                }
            }

            return mapping += "}}}}";
        }
    }
}

using System;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class DynamicObjects
    {
        /// <summary>
        /// 
        /// </summary>
        public static void ElasticIndexMappingReflection(object obj)
        {
            string mapping = "\"settings\":{\"number_of_shards\"" +
                $"{Environment.GetEnvironmentVariable("ShardNum")}" +
                "}," +
                "\"mappings\" : {" +
                "\"_doc\" : {" +
                "\"properties\" : {";

            for(int i = 0; i < obj.GetType().GetProperties().Length; i++)
            {
                mapping += $"\"{obj.GetType().GetProperties()[i].Name.ToLower()}\" : " + "{ \"type\" : " + 
                    $"{Mapping.GetMappedTypeValue(obj.GetType().GetProperties()[i].GetType().ToString())}";

                if (!(i == obj.GetType().GetProperties().Length - 1))
                {
                    mapping += ",";    
                }
            }

            mapping += "}}}}";


        }
    }
}

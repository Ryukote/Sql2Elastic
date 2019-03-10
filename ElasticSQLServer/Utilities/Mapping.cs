using System.Collections.Generic;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class Mapping
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetMappedTypeValue(string key)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("String", "text");
            dictionary.Add("Int", "integer");
            
            if(dictionary.ContainsKey(key))
            {
                return dictionary.GetValueOrDefault(key);
            }

            else
            {
                return "";
            }
        }
    }
}

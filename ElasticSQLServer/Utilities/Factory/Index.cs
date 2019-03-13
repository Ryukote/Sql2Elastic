using ElasticSQLServer.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSQLServer.Utilities.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class Index : IIndexJson
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexData"></param>
        /// <returns></returns>
        public string CreateIndexJson(IEnumerable<Tuple<string, string>> indexData)
        {
            List<Tuple<string, string>> list = indexData.ToList();
            StringBuilder builder = new StringBuilder();

            string mapping = builder.Append("\"settings\":{\"number_of_shards\"")
                .Append($"{Environment.GetEnvironmentVariable("ShardNum")}")
                .Append("},")
                .Append("\"mappings\" : {")
                .Append($"\"{Environment.GetEnvironmentVariable("ElasticDocument")}\"")
                .Append(" : {")
                .Append("\"properties\" : {").ToString();

            for (int i = 0; i < list.Count - 1; i++)
            {
                mapping = builder.Append($"\"{list[i].Item1.ToLower()}\" : ")
                    .Append("{ \"type\" : ")
                    .Append($"{Mapping.GetMappedTypeValue(list[i].Item2)}").ToString();
                
                if (i != list.Count - 1)
                {
                    mapping = builder.Append(",").ToString();
                }
            }

            return mapping = builder.Append("}}}}").ToString();
        }
    }
}

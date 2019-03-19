using ElasticSQLServer.Contracts.Data;
using ElasticSQLServer.Utilities.Data.Mappers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSQLServer.Utilities.Factory
{
    /// <summary>
    /// Elasticsearch index.
    /// </summary>
    public class Index : IIndexJson
    {
        /// <summary>
        /// Creation of Elasticsearch index.
        /// </summary>
        /// <param name="indexData"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public string CreateIndexJson(IEnumerable<Tuple<string, string>> indexData, IConfiguration configuration)
        {
            List<Tuple<string, string>> list = indexData.ToList();
            StringBuilder builder = new StringBuilder();
            SQLServer sqlServer = new SQLServer();
            string document = configuration.GetValue<string>("ElasticDocument");
            builder.Append("{\"settings\":{\"number_of_shards\"")
                .Append(":1")
                .Append("},")
                .Append("\"mappings\" : {")
                .Append("\"" + document + "\" : {")
                .Append("\"properties\" : {");

            for (int i = 0; i < list.Count; i++)
            {
                builder.Append($"\"{list[i].Item1.ToLower()}\" : ")
                    .Append("{ \"type\" : ")
                    .Append($"\"{sqlServer.GetMappedType(list[i].Item2)}\"")
                    .Append("}");

                if (i != list.Count - 1)
                {
                    builder.Append(",");
                }

                else
                {
                    builder.Append("}");
                }
            }

            string result = builder.Append("}}}}").ToString();

            return result;
        }
    }
}

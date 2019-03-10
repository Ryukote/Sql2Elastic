using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// Database context with environment variables provided from Docker.
    /// </summary>
    public class Database : DbContext
    {
        private string connectionString = "";

        /// <summary>
        /// Constructor with filled connection string.
        /// </summary>
        public Database()
        {
            connectionString = $"Data Source={Environment.GetEnvironmentVariable("SqlHost")};Initial Catalog={Environment.GetEnvironmentVariable("DbName")};Persist Security Info=True;User ID={Environment.GetEnvironmentVariable("DbUser")};Password={Environment.GetEnvironmentVariable("DbPassword")}";
        }

        /// <summary>
        /// Overriding OnConfiguring with usage of SqlServer with provided connection string.
        /// </summary>
        /// <param name="optionsBuilder">Context configuration.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@connectionString);
        }

        /// <summary>
        /// Synchronous version of getting dynamic data. This method is private.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<dynamic> GetDynamicData()
        {
            IEnumerable<dynamic> queryResult = this.Set<dynamic>().FromSql($"select * from {Environment.GetEnvironmentVariable("DbTable")}");
            return queryResult;
        }

        /// <summary>
        /// Asynchronous version of getting dynamic data.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> GetDynamicDataAsync()
        {
            return await Task.Run(() => GetDynamicData());
        }
    }
}

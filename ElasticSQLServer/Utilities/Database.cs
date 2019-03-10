using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class Database : DbContext
    {
        private string connectionString = "";

        /// <summary>
        /// 
        /// </summary>
        public Database()
        {
            connectionString = $"Data Source={Environment.GetEnvironmentVariable("SqlHost")};Initial Catalog={Environment.GetEnvironmentVariable("DbName")};Persist Security Info=True;User ID={Environment.GetEnvironmentVariable("DbUser")};Password={Environment.GetEnvironmentVariable("DbPassword")}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@connectionString);
        }

        private dynamic GetDynamicData()
        {
            IEnumerable<dynamic> queryResult = this.Set<dynamic>().FromSql($"select * from {Environment.GetEnvironmentVariable("DbTable")}");
            return queryResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetDynamicDataAsync()
        {
            return await Task.Run(() => GetDynamicData());
        }
    }
}

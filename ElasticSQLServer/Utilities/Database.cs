using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities
{
    /// <summary>
    /// Database context with environment variables provided from Docker.
    /// </summary>
    public class Database
    {
        private string connectionString = "";
        private string dbTable = "";

        /// <summary>
        /// Constructor with filled connection string and database table that needs to be migrated to Elasticsearch.
        /// </summary>
        public Database()
        {
            connectionString = $"Data Source={Environment.GetEnvironmentVariable("SqlHost")};Initial Catalog={Environment.GetEnvironmentVariable("DbName")};Persist Security Info=True;User ID={Environment.GetEnvironmentVariable("DbUsername")};Password={Environment.GetEnvironmentVariable("DbPassword")}";

            dbTable = $"{Environment.GetEnvironmentVariable("DbTable")}";
        }

        /// <summary>
        /// Getting dynamic data.
        /// </summary>
        /// <returns>Filled data table that will be used for migration to Elasticsearch document.</returns>
        public async Task<DataTable> GetDynamicDataAsync()
        {
            using (SqlConnection connection = new SqlConnection(@connectionString))
            {
                SqlCommand command = new SqlCommand($"select * from {dbTable}", connection);
                await command.Connection.OpenAsync();

                DataTable dataTable = new DataTable();
                dataTable.Load(await command.ExecuteReaderAsync());

                command.Connection.Close();
                return dataTable;
            }
        }
    }
}

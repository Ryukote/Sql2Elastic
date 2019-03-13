using ElasticSQLServer.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSQLServer.Utilities.Data.Source
{
    /// <summary>
    /// Working with SQL Server data.
    /// </summary>
    public class SQLServerData : IDataSource
    {
        private string connectionString = "";
        private string dbTable = "";
        private string tableSchema = "";
        private string dbName = "";

        /// <summary>
        /// Initializing connection string, database table, table schema and database name.
        /// </summary>
        public SQLServerData()
        {
            connectionString = $"Data Source={Environment.GetEnvironmentVariable("SqlHost")};Initial Catalog={Environment.GetEnvironmentVariable("DbName")};Persist Security Info=True;User ID={Environment.GetEnvironmentVariable("DbUsername")};Password={Environment.GetEnvironmentVariable("DbPassword")}";

            dbTable = $"{Environment.GetEnvironmentVariable("DbTable")}";
            tableSchema = $"{Environment.GetEnvironmentVariable("TableSchema")}";
            dbName = $"{Environment.GetEnvironmentVariable("DbName")}";
        }

        /// <summary>
        /// Getting data types and names from the table columns.
        /// </summary>
        /// <returns>Returns list of tuple of column name and column data type.</returns>
        public async Task<IEnumerable<Tuple<string, string>>> GetDataTypes()
        {
            List<Tuple<string, string>> columnTypes = new List<Tuple<string, string>>();
            StringBuilder builder = new StringBuilder();

            builder.Append($"USE {dbName} ");
            builder.Append("SELECT COLUMN_NAME, DATA_TYPE ");
            builder.Append("FROM INFORMATION_SCHEMA.COLUMNS ");
            builder.Append($"WHERE TABLE_NAME = '{dbTable}' AND TABLE_SCHEMA = '{tableSchema}' ");

            using (SqlConnection connection = new SqlConnection(@connectionString))
            {
                SqlCommand command = new SqlCommand(builder.ToString());
                await command.Connection.OpenAsync();

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        columnTypes.Add(new Tuple<string, string>(reader[0].ToString(), reader[1].ToString()));
                    }
                }

                command.Connection.Close();

                return columnTypes;
            }
        }

        /// <summary>
        /// Getting dynamic data.
        /// </summary>
        /// <returns>Filled data table that will be used for migration to Elasticsearch document.</returns>
        public async Task<DataTable> GetDatabaseDataAsync()
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

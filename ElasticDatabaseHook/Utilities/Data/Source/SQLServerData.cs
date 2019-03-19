using ElasticSQLServer.Contracts.Data;
using Microsoft.Extensions.Configuration;
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
        private string sqlHost = "";
        private string dbUsername = "";
        private string dbPassword = "";

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializing connection string, database table, table schema and database name.
        /// </summary>
        public SQLServerData(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            dbTable = _configuration.GetValue<string>("DbTable");
            tableSchema = _configuration.GetValue<string>("DbSchema");
            dbName = _configuration.GetValue<string>("DbName");
            sqlHost = _configuration.GetValue<string>("SqlHost");
            dbUsername = _configuration.GetValue<string>("DbUsername");
            dbPassword = _configuration.GetValue<string>("DbPassword");

            connectionString = $"Data Source={sqlHost};Initial Catalog={dbName};Persist Security Info=True;User ID={dbUsername};Password={dbPassword}";
        }

        /// <summary>
        /// Getting data types and names from the table columns.
        /// </summary>
        /// <returns>Returns list of tuple of column name and column data type.</returns>
        public async Task<IEnumerable<Tuple<string, string>>> GetDataTypes()
        {
            try
            {
                List<Tuple<string, string>> columnTypes = new List<Tuple<string, string>>();
                StringBuilder builder = new StringBuilder();
                SqlCommand command;

                builder.Append($"USE {dbName} ");
                builder.Append("SELECT COLUMN_NAME, DATA_TYPE ");
                builder.Append("FROM INFORMATION_SCHEMA.COLUMNS ");
                builder.Append($"WHERE TABLE_NAME = '{dbTable}' AND TABLE_SCHEMA = '{tableSchema}'");

                using (SqlConnection connection = new SqlConnection(@connectionString))
                {
                    command = new SqlCommand(builder.ToString());
                    command.Connection = connection;
                    await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            columnTypes.Add(new Tuple<string, string>(reader[0].ToString(), reader[1].ToString()));
                        }
                    }
                }

                columnTypes.Add(new Tuple<string, string>("hash", "text"));

                command.Connection.Close();

                return columnTypes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Getting dynamic data.
        /// </summary>
        /// <returns>Filled data table that will be used for migration to Elasticsearch document.</returns>
        public async Task<DataTable> GetDatabaseDataAsync()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}

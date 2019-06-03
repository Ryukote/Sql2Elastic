using Microsoft.Extensions.Configuration;
using Npgsql;
using Sql2Elastic.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sql2Elastic.Utilities.Data.Source
{
    /// <summary>
    /// Working with Postgres data.
    /// </summary>
    public class PostgresData : IDataSource
    {
        private string connectionString = "";
        private string dbTable = "";
        private string tableSchema = "";
        private string dbName = "";
        private string sqlHost = "";
        private string dbPort = "";
        private string dbUsername = "";
        private string dbPassword = "";

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializing connection string, database table, table schema and database name.
        /// </summary>
        public PostgresData(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            dbTable = _configuration.GetValue<string>("DbTable");
            tableSchema = _configuration.GetValue<string>("DbSchema");
            dbName = _configuration.GetValue<string>("DbName");
            sqlHost = _configuration.GetValue<string>("SqlHost");
            dbPort = _configuration.GetValue<string>("DbPort");
            dbUsername = _configuration.GetValue<string>("DbUsername");
            dbPassword = _configuration.GetValue<string>("DbPassword");

            connectionString = $"Host={sqlHost};Port={dbPort};User Id={dbUsername};Password={dbPassword};Database={dbName}";
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

                builder.Append($"USE {dbName} ");
                builder.Append("SELECT COLUMN_NAME, DATA_TYPE ");
                builder.Append("FROM INFORMATION_SCHEMA.COLUMNS ");
                builder.Append($"WHERE TABLE_NAME = '{dbTable}' AND TABLE_SCHEMA = '{tableSchema}'");

                using (NpgsqlConnection connection = new NpgsqlConnection(@connectionString))
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(builder.ToString()))
                    {
                        command.Connection = connection;
                        await command.Connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
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
                    }
                }

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
        /// Getting database data.
        /// </summary>
        /// <returns>Filled data table that will be used for migration to Elasticsearch document.</returns>
        public async Task<DataTable> GetDatabaseDataAsync()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(@connectionString))
                {
                    using (NpgsqlCommand command = new NpgsqlCommand($"select * from {dbTable}", connection))
                    {
                        await command.Connection.OpenAsync();

                        DataTable dataTable = new DataTable();
                        dataTable.Load(await command.ExecuteReaderAsync());

                        command.Connection.Close();

                        return dataTable;
                    }
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

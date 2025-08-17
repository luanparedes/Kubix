using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Services.Interfaces;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace Kubix.Services.Classes
{
    public class DataService : IDataService
    {
        private const string CommandsTable = "Commands";
        private readonly string Database = "Kubix.db";

        private readonly IAppInfo _appInfo;

        public DataService()
        {
            _appInfo = Ioc.Default.GetService<IAppInfo>();
        }

        public void CreateDatabaseIfNotExists()
        {
            string dbPath = GetDBPath();

            if (File.Exists(dbPath))
                return;

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
            }
        }

        public void CreateTerminalCommandsTable()
        {
            string dbPath = GetDBPath();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string createTableQuery = @$"
                    CREATE TABLE IF NOT EXISTS {CommandsTable} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Command TEXT NOT NULL,
                        Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                        OrderIndex INTEGER NOT NULL
                    );
                ";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = createTableQuery;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertIntoCommands(string command)
        {
            string dbPath = GetDBPath();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    string updateIndexQuery = @$"
                        UPDATE {CommandsTable}
                        SET OrderIndex = OrderIndex + 1;
                    ";

                    using (var updateCommand = connection.CreateCommand())
                    {
                        updateCommand.CommandText = updateIndexQuery;
                        updateCommand.ExecuteNonQuery();
                    }

                    string insertCommandQuery = @$"
                        INSERT INTO {CommandsTable} (Command, OrderIndex)
                        VALUES (@command, 0);
                    ";

                    using (var insertCommand = connection.CreateCommand())
                    {
                        insertCommand.CommandText = insertCommandQuery;
                        insertCommand.Parameters.AddWithValue("@command", command);
                        insertCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public List<string> GetDBCommands()
        {
            string dbPath = GetDBPath();

            List<string> commands = new List<string>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @$"
                    SELECT Command
                    FROM {CommandsTable}
                    ORDER BY OrderIndex ASC;
                ";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            commands.Add(reader["Command"].ToString());
                        }
                    }
                }
            }

            return commands;
        }

        public string GetDBPath()
        {
            string folderPath = ApplicationData.Current.LocalFolder.Path;

            Directory.CreateDirectory(folderPath);

            return Path.Combine(folderPath, Database);
        }
    }
}

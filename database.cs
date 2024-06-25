using System.Data.Common;
using Microsoft.Data.Sqlite;
using MySqlConnector;

namespace BlogApi.Database
{
    public class Database
    {
        //Database types 
        public enum DatabaseType { Postgress, Sqlite , Firebird , MySql, None}

        // Database connection info
        private string connectionString = "Server=localhost;Database=blog;User Id=root;Password=password;";
        private DatabaseType dbType;

        // Drapper connection variable 
        public DbConnection connection;  

        public Database (){
            
        }
        public Database (string connectionString, DatabaseType dbType) : base()
        {
            this.connectionString = connectionString;
            this.dbType = dbType;
        }

        public Database (IConfiguration config ) : this(
            config.GetConnectionString("Database")??"",
            config.GetSection("DbType").Get<string>() switch
                {
                    "Postgress" => DatabaseType.Postgress,
                    "Sqlite" => DatabaseType.Sqlite,
                    "Firebird" => DatabaseType.Firebird,
                    "MySql" => DatabaseType.MySql,
                    _ => DatabaseType.None,
                }
            ) {}

        public void Connect(){
            connection = dbType switch
            {
                DatabaseType.Postgress => new Npgsql.NpgsqlConnection(connectionString),
                DatabaseType.Sqlite => (DbConnection)new SqliteConnection(connectionString),
                DatabaseType.Firebird => (DbConnection)new FirebirdSql.Data.FirebirdClient.FbConnection(connectionString),
                DatabaseType.MySql => (DbConnection)new MySqlConnection(connectionString),
                _ => throw new Exception("Database type not supported"),
            };
            connection.Open();
        }

        public void Close(){
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }


}

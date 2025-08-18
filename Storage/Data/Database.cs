using System.Data.SQLite;

namespace Storage
{
    public static class DataBase
    {
        private static SQLiteConnection? _connection;
        private static readonly string _dbPath =
            "/home/dev/Biblioteca/Storage/Data/biblioteca.db";
            //Encaminhando o local exato da pasta com o banco de dados

        public static SQLiteConnection GetConnection()
        {
            if (_connection == null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_dbPath)!);

                var connectionString = $"Data Source={_dbPath};Version=3;";
                _connection = new SQLiteConnection(connectionString);
                _connection.Open();
            }

            return _connection;
        }
    }
}

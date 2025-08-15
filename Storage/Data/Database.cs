using System;
using System.Data.SQLite;

namespace Biblioteca.Storage
{
    public static class Database
    {
        private static SQLiteConnection _connection = null!;
        // null! força a conxeão começar.
        //pode ser usado o ? para dizer que é nullable
        private static readonly string _connectionString =
            $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "biblioteca.db")};Version=3;";

        public static SQLiteConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SQLiteConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }
    }
}

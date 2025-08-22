using System.Data.SQLite;
using Biblioteca.Domain.Interfaces;
using Domain;

namespace Storage
{
    public class LanguageRepository : ILanguageRepository
    {
        public void Add(Language language)
        {
            var connection = DataBase.GetConnection();
            string query = "INSERT INTO Language (Name, Created_At, Updated_At) VALUES (@Name, @Created_At, @Updated_At)";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", language.Name);
                command.Parameters.AddWithValue("@Created_At", language.Created_At);
                command.Parameters.AddWithValue("@Updated_At", language.Updated_At);
                command.ExecuteNonQuery();
            }
        }

        public Language GetById(int ID)
        {
            var connection = DataBase.GetConnection();
            var command = new SQLiteCommand("SELECT * FROM Language WHERE ID = @ID", connection);
            command.Parameters.AddWithValue("@ID", ID);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Language
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = Convert.ToString(reader["Name"]) ?? "",
                        Created_At = Convert.ToDateTime(reader["Created_At"]),
                        Updated_At = Convert.ToDateTime(reader["Updated_At"])
                    };
                }
            }

            return null!;
        }

        public IEnumerable<Language> GetAll()
        {
            var languages = new List<Language>();
            var connection = DataBase.GetConnection();
            var command = new SQLiteCommand("SELECT * FROM Language", connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    languages.Add(new Language
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = reader["Name"].ToString() ?? "",
                        Created_At = Convert.ToDateTime(reader["Created_At"]),
                        Updated_At = Convert.ToDateTime(reader["Updated_At"])
                    });
                }
            }

            return languages;
        }

        public void Update(Language language)
        {
            var connection = DataBase.GetConnection();
            string query = @"UPDATE Language 
                             SET Name = @Name, Updated_At = @Updated_At
                             WHERE ID = @ID";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", language.ID);
                command.Parameters.AddWithValue("@Name", language.Name);
                command.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int ID)
        {
            var connection = DataBase.GetConnection();
            string query = "DELETE FROM Language WHERE ID = @ID";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", ID);
                command.ExecuteNonQuery();
            }
        }
    }
}

using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class LanguageRepository
    {
        private string _connectionString = "Data Source=biblioteca.db";

        public void Add(Language language)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Language (ID, Name, LanguageID CreatedAt, UpdatedAt) VALUES (@ID, @Name, @LanguageID, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", language.ID);
                    command.Parameters.AddWithValue("@language", language.Name);
                    command.Parameters.AddWithValue("@LanguageID", language.LanguageID);

                    command.Parameters.AddWithValue("@CreatedAt", language.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", language.UpdatedAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Language> GetAll()
        {

            var languages = new List<Language>();

            using (var connection = new SQLiteConnection("Data Source=biblioteca.db"))
            {
                connection.Open();

                // query
                var command = new SQLiteCommand("SELECT * FROM Language", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var language = new Language
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            Name = reader["Name"].ToString() ?? "",
                            LanguageID = Guid.Parse(reader["LanguageID"].ToString() ?? Guid.Empty.ToString()),

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                        languages.Add(language);
                    }
                }
            }

            return languages;
        }


        public Language GetById(Guid ID)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Language WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Language
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            Name = Convert.ToString(reader["language"]) ?? "",
                            LanguageID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                    }
                }
            }

            return null!;
        }

        public void Update(Language language)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var query = @"UPDATE Language 
                            SET ID = @ID, Name = @Name, LanguageID = @LanguageID
                            UpdatedAt = @UpdatedAt
                            WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", language.ID.ToString());
                    command.Parameters.AddWithValue("@Name", language.Name);
                    command.Parameters.AddWithValue("@LanguageID", language.LanguageID);

                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Guid ID)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Language WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
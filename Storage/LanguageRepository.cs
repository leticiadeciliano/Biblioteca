using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class LanguageRepository
    {
        public void Add(Language language)
        {
            var connection = DataBase.GetConnection();
            {
                string query = "INSERT INTO Language (Name, LanguageID CreatedAt, UpdatedAt) VALUES (@Name, @LanguageID, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
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

            var connection = DataBase.GetConnection();
            {
                // query
                var command = new SQLiteCommand("SELECT * FROM Language", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var language = new Language
                        {
                            ID = Convert.ToInt32(reader["ID"]),
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


        public Language GetById(int ID)
        {
            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Language WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Language
                        {
                            ID = Convert.ToInt32(reader["ID"]),
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
            var connection = DataBase.GetConnection();
            {
                var query = @"UPDATE Language 
                            SET Name = @Name, LanguageID = @LanguageID
                            UpdatedAt = @UpdatedAt
                            WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", language.ID);
                    command.Parameters.AddWithValue("@Name", language.Name);
                    command.Parameters.AddWithValue("@LanguageID", language.LanguageID);

                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int ID)
        {
            var connection = DataBase.GetConnection();
            {
                var query = "DELETE FROM Language WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
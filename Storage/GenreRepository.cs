using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class GenreRepository
    {
        public void Add(Genre genre)
        {
            var connection = DataBase.GetConnection();
            {
                string query = "INSERT INTO Genre (Name_genre, Created_At, Updated_At) VALUES (@Name_genre, @Created_At, @Updated_At)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name_genre", genre.Name_genre);

                    command.Parameters.AddWithValue("@Created_At", genre.Created_At);
                    command.Parameters.AddWithValue("@Updated_At", genre.Updated_At);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Genre> GetAll()
        {

            var genres = new List<Genre>();

            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Genre", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var genre = new Genre
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name_genre = Convert.ToString(reader["Name_genre"]) ?? "",

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };

                        genres.Add(genre);
                    }
                }
            }

            return genres;
        }

        public Genre GetById(int ID)
        {
            var connection = DataBase.GetConnection();
            {

                var command = new SQLiteCommand("SELECT * FROM Genre WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Genre
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name_genre = Convert.ToString(reader["Name_genre"]) ?? "",

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };
                    }
                }
            }

            return null!;
        }

        public void Update(Genre genre)
        {
            var connection = DataBase.GetConnection();
            {
                var query = @"UPDATE Genre
                            SET Name_genre = @Name_genre,
                            Updated_At = @Updated_At
                            WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", genre.ID);
                    command.Parameters.AddWithValue("@Name_genre", genre.Name_genre);

                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void Delete(int ID)
        {
            var connection = DataBase.GetConnection();
            {
                var query = "DELETE FROM Genre WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}
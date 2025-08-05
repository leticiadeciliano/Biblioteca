using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class GenreRepository
    {
        private string _connectionString = "Data Source=biblioteca.db";

        public void Add(Genre genre)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Genre (ID, Title, CreatedAt, UpdatedAt) VALUES (@ID, @Name_genre, @Description, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", genre.ID);
                    command.Parameters.AddWithValue("@Name_genre", genre.Name_genre);

                    command.Parameters.AddWithValue("@CreatedAt", genre.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", genre.UpdatedAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Genre> GetAll()
        {
            //criando lista
            var genres = new List<Genre>();

            //abrindo conexão com o banco
            using (var connection = new SQLiteConnection("Data Source=biblioteca.db"))
            {
                connection.Open();

                //query
                var command = new SQLiteCommand("SELECT * FROM Genre", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var genre = new Genre
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            Name_genre = Convert.ToString(reader["Name_genre"]) ?? "",

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };

                        genres.Add(genre);
                    }
                }
            }

            return genres;
        }

        public Genre GetById(Guid id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Genre WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Genre
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            Name_genre = Convert.ToString(reader["Name_genre"]) ?? "",

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                    }
                }
            }

            return null!;
        }

        public void Update(Genre genre)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var query = @"UPDATE Genre
                            SET ID = @ID, Name_genre = @Name_genre,
                            UpdatedAt = @UpdatedAt
                            WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", genre.ID.ToString());
                    command.Parameters.AddWithValue("@Title", genre.Name_genre);

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
                var query = "DELETE FROM Genre WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}
using System.Data.SQLite;
using Biblioteca.Domain.Interfaces;
using Domain;

namespace Storage
{
    public class GenreRepository : IGenreRepository
    {
        public void Add(Genre genre)
        {
            using (var connection = DataBase.GetConnection())
            {
                string query = @"INSERT INTO Genre (Name_genre, Created_At, Updated_At) 
                                 VALUES (@Name_genre, @Created_At, @Updated_At)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name_genre", genre.Name_genre);
                    command.Parameters.AddWithValue("@Created_At", genre.Created_At);
                    command.Parameters.AddWithValue("@Updated_At", genre.Updated_At);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Genre GetById(Guid id)
        {
            using (var connection = DataBase.GetConnection())
            {
                var command = new SQLiteCommand("SELECT * FROM Genre WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id.ToString());

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
            throw new Exception($"Gênero com ID {id} não encontrado.");
        }

        public IEnumerable<Genre> GetAll()
        {
            var genres = new List<Genre>();

            using (var connection = DataBase.GetConnection())
            {
                var query = "SELECT * FROM Genre";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        genres.Add(new Genre
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name_genre = Convert.ToString(reader["Name_genre"]) ?? "",
                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        });
                    }
                }
            }
            return genres;
        }

        public void Update(Genre genre)
        {
            using (var connection = DataBase.GetConnection())
            {
                var query = @"UPDATE Genre
                              SET Name_genre = @Name_genre,
                                  Updated_At = @Updated_At
                              WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", genre.ID.ToString());
                    command.Parameters.AddWithValue("@Name_genre", genre.Name_genre);
                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Guid ID)
        {
            using (var connection = DataBase.GetConnection())
            {
                var query = "DELETE FROM Genre WHERE ID = @ID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

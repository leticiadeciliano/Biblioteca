using System;
using Domain; 
using System.Data.SQLite;
using System.Collections.Generic;


namespace Storage
{
    public class PublisherRepository
    {
        private string _connectionString = "Data Source=biblioteca.db";

        public void Add(Publisher publisher)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Publisher (ID, Name_Publisher, CreatedAt, UpdatedAt) VALUES (@Id, @Name_Publisher, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", publisher.ID);
                    command.Parameters.AddWithValue("@Name_Publisher", publisher.Name_Publisher);

                    command.Parameters.AddWithValue("@CreatedAt", publisher.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", publisher.UpdatedAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Publisher> GetAll()
        {
            var publishers = new List<Publisher>();

            using (var connection = new SQLiteConnection("Data Source=biblioteca.db"))
            {
                connection.Open();

                //query
                var command = new SQLiteCommand("SELECT * FROM Publisher", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var publisher = new Publisher
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            Name_Publisher = Convert.ToString(reader["Name_Publisher"]) ?? "",
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };

                        publishers.Add(publisher);
                    }
                }
            }

            return publishers;
        }


        //Classe GetById
        public Publisher GetById(Guid ID)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Publisher WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@Id", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Publisher
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            Name_Publisher = Convert.ToString(reader["Name_Publisher"]) ?? "",
                        
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                    }
                }
            }

            return null!;
        }

        public void Update(Publisher publisher)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var query = @"UPDATE Publisher
                            SET ID = @ID, Name_Publisher = @Name_Publisher
                            UpdatedAt = @UpdatedAt
                            WHERE Id = @Id";
                            
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", publisher.ID.ToString());
                    command.Parameters.AddWithValue("@Name_Publisher", publisher.Name_Publisher);

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
                var query = "DELETE FROM Publisher WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", ID.ToString());
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

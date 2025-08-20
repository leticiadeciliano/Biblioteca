using System;
using Domain; 
using System.Data.SQLite;
using System.Collections.Generic;
using Biblioteca.Domain.Interfaces;


namespace Storage
{
    public class PublisherRepository : IPublisherRepository
    {
        public void Add(Publisher publisher)
        {
            var connection = DataBase.GetConnection();
            {
                string query = "INSERT INTO Publisher (Name_Publisher, CreatedAt, UpdatedAt) VALUES (@Name_Publisher, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name_Publisher", publisher.Name_Publisher);

                    command.Parameters.AddWithValue("@CreatedAt", publisher.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", publisher.UpdatedAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Publisher> GetAll()
        {
            var publishers = new List<Publisher>();

            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Publisher", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var publisher = new Publisher
                        {
                            ID = Convert.ToInt32(reader["ID"]),
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

        public Publisher GetById(int ID)
        {
            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Publisher WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Publisher
                        {
                            ID = Convert.ToInt32(reader["ID"]),
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
            var connection = DataBase.GetConnection();
            {
                var query = @"UPDATE Publisher
                            SET Name_Publisher = @Name_Publisher
                            UpdatedAt = @UpdatedAt
                            WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", publisher.ID);
                    command.Parameters.AddWithValue("@Name_Publisher", publisher.Name_Publisher);

                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void Delete(int ID)
        {
            var connection = DataBase.GetConnection();
            {
                var query = "DELETE FROM Publisher WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}

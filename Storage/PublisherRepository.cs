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
                string query = "INSERT INTO Publisher (Name, Created_At, Updated_At) VALUES (@Name, @Created_At, @Updated_At)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", publisher.Name);

                    command.Parameters.AddWithValue("@Created_At", publisher.Created_At);
                    command.Parameters.AddWithValue("@Updated_At", publisher.Updated_At);

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
                            Name = Convert.ToString(reader["Name"]) ?? "",
                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
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
                            Name = Convert.ToString(reader["Name"]) ?? "",

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
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
                            SET Name = @Name
                            Updated_At = @Updated_At
                            WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", publisher.ID);
                    command.Parameters.AddWithValue("@Name", publisher.Name);

                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);

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

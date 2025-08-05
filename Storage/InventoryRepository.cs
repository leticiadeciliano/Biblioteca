using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class InventoryRepository
    {
        private string _connectionString = "Data Source=biblioteca.db";

        public void Add(Inventory inventory)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Inventory (ID, Condition, is_foreign, CreatedAt, UpdatedAt) VALUES (@ID, @Condition, @is_foreign, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", inventory.ID);
                    command.Parameters.AddWithValue("@Condition", inventory.Condition);
                    command.Parameters.AddWithValue("@is_foreign", inventory.is_foreign);
                    command.Parameters.AddWithValue("@CatalogID", inventory.CatalogID);

                    command.Parameters.AddWithValue("@CreatedAt", inventory.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", inventory.UpdatedAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Inventory> GetAll()
        {
            var inventory = new List<Inventory>();

            using (var connection = new SQLiteConnection("Data Source=biblioteca.db"))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Inventory", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Inventory
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            Condition = Convert.ToInt32(reader["Condition"]),
                            is_foreign = Convert.ToBoolean(reader["is_foreign"]),
                            CatalogID = Guid.Parse(reader["CatalogID"].ToString() ?? Guid.Empty.ToString()),

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };

                        inventory.Add(item);
                    }
                }
            }

            return inventory;
        }

        public Inventory GetById(Guid id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Inventory WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Inventory
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            Condition = Convert.ToInt32(reader["Condition"]),
                            is_foreign = Convert.ToBoolean(reader["is_foreign"]),
                            CatalogID = Guid.Parse(reader["CatalogID"].ToString() ?? Guid.Empty.ToString()),

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                    }
                }
            }

            return null!;
        }

        public void Update(Inventory inventory)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var query = @"UPDATE Inventory 
                            SET ID = @ID, Condition = @Condition, is_foreign = @is_foreign, CatalogID = @CatalogID,
                            UpdatedAt = @UpdatedAt
                            WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", inventory.ID.ToString());
                    command.Parameters.AddWithValue("@Title", inventory.Condition);
                    command.Parameters.AddWithValue("@Author", inventory.is_foreign);
                    command.Parameters.AddWithValue("@Year", inventory.CatalogID);

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
                var query = "DELETE FROM Inventory WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", ID.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}
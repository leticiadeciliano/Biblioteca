using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class InventoryRepository
    {
        public void Add(Inventory inventory)
        {
            var connection = DataBase.GetConnection();
            {
                string query = "INSERT INTO Inventory (Condition, Is_available, Created_At, Updated_At) VALUES (@Condition, @Is_available, @Created_At, @Updated_At)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Condition", inventory.Condition);
                    command.Parameters.AddWithValue("@Is_available", inventory.Is_available);
                    command.Parameters.AddWithValue("@CatalogID", inventory.CatalogID);

                    command.Parameters.AddWithValue("@Created_At", inventory.Created_At);
                    command.Parameters.AddWithValue("@Updated_At", inventory.Updated_At);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Inventory> GetAll()
        {
            var inventory = new List<Inventory>();

            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Inventory", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Inventory
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Condition = Convert.ToInt32(reader["Condition"]),
                            Is_available = Convert.ToBoolean(reader["Is_available"]),
                            CatalogID = Guid.Parse(reader["CatalogID"].ToString() ?? Guid.Empty.ToString()),

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };

                        inventory.Add(item);
                    }
                }
            }

            return inventory;
        }

        public Inventory GetById(int ID)
        {
            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Inventory WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Inventory
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Condition = Convert.ToInt32(reader["Condition"]),
                            Is_available = Convert.ToBoolean(reader["Is_available"]),
                            CatalogID = Guid.Parse(reader["CatalogID"].ToString() ?? Guid.Empty.ToString()),

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };
                    }
                }
            }

            return null!;
        }

        public void Update(Inventory inventory)
        {
            var connection = DataBase.GetConnection();
            {
                //query
                var query = @"UPDATE Inventory 
                            SET Condition = @Condition, Is_available = @Is_available, CatalogID = @CatalogID,
                            Updated_At = @Updated_At
                            WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", inventory.ID);
                    command.Parameters.AddWithValue("@Title", inventory.Condition);
                    command.Parameters.AddWithValue("@Author", inventory.Is_available);
                    command.Parameters.AddWithValue("@Year", inventory.CatalogID);

                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Guid ID)
        {
            var connection = DataBase.GetConnection();
            {
                var query = "DELETE FROM Inventory WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.ExecuteNonQuery();
                } 
            }
        }

        internal void Delete(int iD)
        {
            throw new NotImplementedException();
        }
    }
}
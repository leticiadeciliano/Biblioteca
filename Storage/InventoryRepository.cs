using System.Data.SQLite;
using Biblioteca.Domain.Interfaces;
using Domain;

namespace Storage
{
    public class InventoryRepository : IInventoryRepository
    {
        public void Add(Inventory inventory)
        {
            using (var connection = DataBase.GetConnection())
            {
                string query = @"INSERT INTO Inventory Condition, Is_available, Created_At, Updated_At) VALUES (@Condition, @Is_available, @Created_At, @Updated_At)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name_inventory", inventory.Condition);
                    command.Parameters.AddWithValue("@Created_At", inventory.Is_available);

                    command.Parameters.AddWithValue("@Updated_At", inventory.Created_At);
                    command.Parameters.AddWithValue("@Updated_At", inventory.Updated_At);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Inventory GetById(int ID)
        {
            using (var connection = DataBase.GetConnection())
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
            throw new Exception($"Inventário com ID {ID} não encontrado.");
        }

        public IEnumerable<Inventory> GetAll()
        {
            var inventory = new List<Inventory>();

            using (var connection = DataBase.GetConnection())
            {
                var query = "SELECT * FROM Inventory";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventory.Add(new Inventory
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Condition = Convert.ToInt32(reader["Condition"]),
                            Is_available = Convert.ToBoolean(reader["Is_available"]),
                            CatalogID = Guid.Parse(reader["CatalogID"].ToString() ?? Guid.Empty.ToString()),

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        });
                    }
                }
            }
            return inventory;
        }

        public void Update(Inventory inventory)
        {
            using (var connection = DataBase.GetConnection())
            {
                var query = @"UPDATE Inventory
                            SET Condition = @Condition, Is_available = @Is_available, CatalogID = @CatalogID,
                            Updated_At = @Updated_At
                            WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", inventory.ID.ToString());
                    command.Parameters.AddWithValue("@Condition", inventory.Condition);
                    command.Parameters.AddWithValue("Is_avaiable", inventory.Is_available);

                    command.Parameters.AddWithValue("@Created_At", DateTime.Now);
                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int ID)
        {
            using (var connection = DataBase.GetConnection())
            {
                var query = "DELETE FROM Inventory WHERE ID = @ID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

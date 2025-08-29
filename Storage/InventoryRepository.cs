using System.Data.SQLite;
using Biblioteca.Domain.Interfaces;
using Domain;

namespace Storage
{
    public class InventoryRepository : IInventoryRepository
    {
        public void Add(Inventory inventory)
        {
            var connection = DataBase.GetConnection();
            {
                string query = @"INSERT INTO Inventory (Catalog_ID, Condition, Created_At, Updated_At) VALUES (@Catalog_ID, @Condition, @Created_At, @Updated_At)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Catalog_ID", inventory.Catalog_ID.ToString());
                    command.Parameters.AddWithValue("@Condition", inventory.Condition);

                    command.Parameters.AddWithValue("@Created_At", inventory.Created_At);
                    command.Parameters.AddWithValue("@Updated_At", inventory.Updated_At);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Inventory> GetAll()
        {
            var inventory = new List<Inventory>();
            var connection = DataBase.GetConnection();

            var query = "SELECT * FROM Inventory";
            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Guid catalog_ID = Guid.Empty;

                    // Verificando se Catalog_ID é Null
                    if (reader["Catalog_ID"] != DBNull.Value)
                    {
                        var catalogValue = reader["Catalog_ID"].ToString()?.Trim();

                        if (!string.IsNullOrEmpty(catalogValue))
                        {
                            if (!Guid.TryParse(catalogValue, out catalog_ID))
                            {
                                Console.WriteLine($"[ERRO] Catalog_ID inválido encontrado no banco: {catalogValue}");
                            }
                        }
                    }

                    inventory.Add(new Inventory
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Catalog_ID = catalog_ID,
                        Condition = Convert.ToInt32(reader["Condition"]),
                        Created_At = Convert.ToDateTime(reader["Created_At"]),
                        Updated_At = Convert.ToDateTime(reader["Updated_At"])
                    });
                }
            }

            return inventory;
        }


        
        public void Update(Inventory inventory)
        {
            var connection = DataBase.GetConnection();
            {
                var query = @"UPDATE Inventory
                            SET  Catalog_ID = @Catalog_ID, Condition = @Condition,
                            Updated_At = @Updated_At
                            WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", inventory.ID);
                    command.Parameters.AddWithValue("@Catalog_ID", inventory.Catalog_ID.ToString());
                    command.Parameters.AddWithValue("@Condition", inventory.Condition);

                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int ID)
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
    }
}

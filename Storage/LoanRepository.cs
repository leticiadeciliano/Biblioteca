using Domain;
using System.Data.SQLite;
using Biblioteca.Domain.Interfaces;
using CLI.Helpers;

namespace Storage
{

    public class LoanRepository : ILoanRepository
    {
        public void Add(Loan loan)
        {
            var connection = DataBase.GetConnection();

            string query = "INSERT INTO Loan (ID, Days_to_expire, Client_ID, Inventory_ID, Return_At, Created_At, Updated_At) VALUES (@ID, @Days_to_expire, @Client_ID, @Inventory_ID, @Return_At, @Created_At, @Updated_At)";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", GuidHelper.ToDb(loan.ID, asBlob: false));
                command.Parameters.AddWithValue("@Days_to_expire", loan.Days_to_expire);
                command.Parameters.AddWithValue("@Client_ID", GuidHelper.ToDb(loan.Client_ID, asBlob: false));
                command.Parameters.AddWithValue("@Inventory_ID", loan.Inventory_ID);
                command.Parameters.AddWithValue("@Return_At", loan.Return_At);
                command.Parameters.AddWithValue("@Created_At", loan.Created_At);
                command.Parameters.AddWithValue("@Updated_At", loan.Updated_At);

                command.ExecuteNonQuery();

            }
        }

        public IEnumerable<Loan> GetAll()
        {
            var loans = new List<Loan>();
            var connection = DataBase.GetConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using (var command = new SQLiteCommand(
                "SELECT ID, Client_ID, Inventory_ID, Days_to_expire, Created_At, Updated_At, Return_At FROM Loan", connection))
            using (var reader = command.ExecuteReader())
            {
                int ordId = reader.GetOrdinal("ID");
                int ordClientId = reader.GetOrdinal("Client_ID");
                int ordInventoryId = reader.GetOrdinal("Inventory_ID");
                int ordDaysToExpire = reader.GetOrdinal("Days_to_expire");
                int ordCreatedAt = reader.GetOrdinal("Created_At");
                int ordUpdatedAt = reader.GetOrdinal("Updated_At");
                int ordReturnAt = reader.GetOrdinal("ReturnAt");

                while (reader.Read())
                {
                    //ID
                    Guid id = Guid.Empty;
                    if (!reader.IsDBNull(ordId))
                        id = GuidHelper.FromDb(reader.GetValue(ordId));

                    //Client_ID
                    Guid clientId = Guid.Empty;
                    if (!reader.IsDBNull(ordClientId))
                        clientId = GuidHelper.FromDb(reader.GetValue(ordClientId));

                    //Inventory_ID
                    int inventoryId = 0;
                    if (!reader.IsDBNull(ordInventoryId))
                    {
                        var raw = reader.GetValue(ordInventoryId);
                        if (raw is long l) inventoryId = Convert.ToInt32(l);
                        else if (raw is int i) inventoryId = i;
                        else int.TryParse(raw.ToString(), out inventoryId);
                    }

                    //Days_to_expire
                    int daysToExpire = 30;
                    if (!reader.IsDBNull(ordDaysToExpire))
                    {
                        var raw = reader.GetValue(ordDaysToExpire);
                        if (raw is long l2) daysToExpire = Convert.ToInt32(l2);
                        else if (raw is int i2) daysToExpire = i2;
                        else int.TryParse(raw.ToString(), out daysToExpire);
                    }

                    DateTime createdAt = reader.IsDBNull(ordCreatedAt) ? DateTime.MinValue : Convert.ToDateTime(reader.GetValue(ordCreatedAt));
                    DateTime updatedAt = reader.IsDBNull(ordUpdatedAt) ? DateTime.MinValue : Convert.ToDateTime(reader.GetValue(ordUpdatedAt));
                    DateTime returnAt  = reader.IsDBNull(ordReturnAt) ? DateTime.MinValue : Convert.ToDateTime(reader.GetValue(ordReturnAt));

                    loans.Add(new Loan
                    {
                        ID = id,
                        Client_ID = clientId,
                        Inventory_ID = inventoryId,
                        Days_to_expire = daysToExpire,
                        Created_At = createdAt,
                        Updated_At = updatedAt,
                        Return_At = returnAt
                    });
                }
            }

            return loans;
        }

        public Loan GetById(Guid ID)
        {
            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Loan WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Loan
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            Days_to_expire = Convert.ToInt32(reader["days_to_expire"]),
                            Client_ID = Guid.Parse(reader["Client_ID"].ToString() ?? Guid.Empty.ToString()),
                            Inventory_ID = Convert.ToInt32(reader["Inventory_ID"]),

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"]),
                            Return_At = Convert.ToDateTime(reader["ReturndAt"])
                        };
                    }
                }
            }

            return null!;
        }

        public void Update(Loan loan)
        {
            var connection = DataBase.GetConnection();
            {
                var query = @"UPDATE Loan
                            SET days_to_expire = @days_to_expire, Client_ID = @Client_ID, Inventory_ID = @Inventory_ID,
                            Updated_At = @Updated_At
                            WHERE ID = @ID";


                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", GuidHelper.ToDb(loan.Client_ID, asBlob: false));
                    command.Parameters.AddWithValue("@days_to_expire", loan.Days_to_expire);
                    command.Parameters.AddWithValue("@Client_ID", GuidHelper.ToDb(loan.Client_ID, asBlob: false));
                    command.Parameters.AddWithValue("@Inventory_ID", loan.Inventory_ID);
                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);

                    
                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Guid ID)
        {
            var connection = DataBase.GetConnection();
            {
                var query = "DELETE FROM Loan WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}

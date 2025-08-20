using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;
using Biblioteca.Domain.Interfaces;

namespace Storage
{

    public class LoanRepository : ILoanRepository
    {
        public void Add(Loan loan)
        {
            var connection = DataBase.GetConnection();
            {
                string query = "INSERT INTO Loan (ID, days_to_expire, ClientID, InventoryID, ReturnAt, CreatedAt, UpdatedAt) VALUES (@ID, @days_to_expire, @ClientID, @InventoryID, @ReturnAt, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", loan.ID);
                    command.Parameters.AddWithValue("@days_to_expire", loan.Days_to_expire);
                    command.Parameters.AddWithValue("@CLientID", loan.ClientID);
                    command.Parameters.AddWithValue("@InventoryID", loan.InventoryID);

                    command.Parameters.AddWithValue("@CreatedAt", loan.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", loan.UpdatedAt);
                    command.Parameters.AddWithValue("@ReturnAt", loan.ReturnAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Loan> GetAll()
        {
            var loans = new List<Loan>();

            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Loan", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var loan = new Loan
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            Days_to_expire = Convert.ToInt32(reader["days_to_expire"]),
                            ClientID = Guid.Parse(reader["ClientID"].ToString() ?? Guid.Empty.ToString()),
                            InventoryID = Guid.Parse(reader["InventoryID"].ToString() ?? Guid.Empty.ToString()),

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                            ReturnAt = Convert.ToDateTime(reader["ReturnAt"])
                        };

                        loans.Add(loan);
                    }
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
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            Days_to_expire = Convert.ToInt32(reader["days_to_expire"]),
                            ClientID = Guid.Parse(reader["ClientID"].ToString() ?? Guid.Empty.ToString()),
                            InventoryID = Guid.Parse(reader["InventoryID"].ToString() ?? Guid.Empty.ToString()),

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                            ReturnAt = Convert.ToDateTime(reader["ReturndAt"])
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
                            SET days_to_expire = @days_to_expire, ClientID = @ClientID, InventoryID = @InventoryID,
                            UpdatedAt = @UpdatedAt
                            WHERE ID = @ID";


                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", loan.ID);
                    command.Parameters.AddWithValue("@days_to_expire", loan.Days_to_expire);
                    command.Parameters.AddWithValue("@ClientID", loan.ClientID);
                    command.Parameters.AddWithValue("@InventoryID", loan.InventoryID);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

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

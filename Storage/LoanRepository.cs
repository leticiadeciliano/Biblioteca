using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{

    public class LoanRepository
    {
        private string _connectionString = "Data Source=biblioteca.db";

        public void Add(Loan loan)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Loan (ID, days_to_expire, ClientID, InventoryID, ReturnAt, CreatedAt, UpdatedAt) VALUES (@ID, @days_to_expire, @ClientID, @InventoryID, @ReturnAt, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", loan.ID);
                    command.Parameters.AddWithValue("@days_to_expire", loan.days_to_expire);
                    command.Parameters.AddWithValue("@CLientID", loan.ClientID);
                    command.Parameters.AddWithValue("@InventoryID", loan.InventoryID);

                    command.Parameters.AddWithValue("@CreatedAt", loan.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", loan.UpdatedAt);
                    command.Parameters.AddWithValue("@ReturnAt", loan.ReturnAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Loan> GetAll()
        {
            var loans = new List<Loan>();

            using (var connection = new SQLiteConnection("Data Source=biblioteca.db"))
            {
                connection.Open();

                //query
                var command = new SQLiteCommand("SELECT * FROM Loan", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var loan = new Loan
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            days_to_expire = Convert.ToInt32(reader["days_to_expire"]),
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



        //Classe GetById
        public Loan GetById(Guid ID)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Loan WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@Id", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Loan
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            days_to_expire = Convert.ToInt32(reader["days_to_expire"]),
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
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var query = @"UPDATE Loan
                            SET days_to_expire = @days_to_expire, ClientID = @ClientID, InventoryID = @InventoryID,
                            UpdatedAt = @UpdatedAt
                            WHERE ID = @ID";


                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", loan.ID);
                    command.Parameters.AddWithValue("@days_to_expire", loan.days_to_expire);
                    command.Parameters.AddWithValue("@ClientID", loan.ClientID);
                    command.Parameters.AddWithValue("@InventoryID", loan.InventoryID);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        //Class DELETE
        public void Delete(Guid ID)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Loan WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", ID.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}

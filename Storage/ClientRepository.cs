using System;
using System.Data.SQLite;
using Biblioteca.Domain.Interfaces;
using Domain;

namespace Storage
{
    public class ClientRepository : IClientRepository
    {
        //Classe Adicionar 
        public void Add(Client client)
        {
            var connection = DataBase.GetConnection();
            {
                //Metódo para inserir
                string query = "INSERT INTO Clients (ID, Name, Email, Phone, Created_At, Updated_At) VALUES (@ID, @Name, @Email, @Phone, @Created_At, @Updated_At)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", client.ID);
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@Phone", client.Phone);

                    command.Parameters.AddWithValue("@Created_At", client.Created_At);
                    command.Parameters.AddWithValue("@Updated_At", client.Updated_At);

                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Client> GetAll()
        {
            //lista
            var client = new List<Client>();

            var connection = DataBase.GetConnection();
            {
                //query
                var command = new SQLiteCommand("SELECT * FROM Clients", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var clients = new Client
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string
                            Name = Convert.ToString(reader["Name"]) ?? "",
                            Email = Convert.ToString(reader["Email"]) ?? "",
                            Phone = Convert.ToString(reader["Phone"]) ?? "",

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };

                        client.Add(clients);
                    }
                }
            }

            return client;
        }



        public Client GetById(Guid ID)
        {
            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Clients WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Client
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            Name = Convert.ToString(reader["Name"]) ?? "",
                            Email = Convert.ToString(reader["Email"]) ?? "",
                            Phone = Convert.ToString(reader["Phone"]) ?? "",

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };
                    }
                }
            }
            return null!;
            //caso não encontre o ID, retornará null
        }

       
        public void Update(Client client)
        {
            var connection = DataBase.GetConnection();
            {
                var query = @"UPDATE Clients
                            SET Name = @Name, Email = @Email, Phone = @Phone,
                                Updated_At = @Updated_At
                            WHERE Id = @Id";


                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", client.ID.ToString());
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@Phone", client.Phone);

                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                    //como a classe é SOMENTE para atualizar, não é necessário colocar Created_At

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void Delete(Guid ID)
        {
            var connection = DataBase.GetConnection();
            {
                var query = "DELETE FROM Clients WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}
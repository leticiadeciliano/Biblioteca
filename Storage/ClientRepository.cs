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
                string query = "INSERT INTO Client (ID, Name, Email, Phone, CreatedAt, UpdatedAt) VALUES (@ID, @Name, @Email, @Phone, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", client.ID);
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@Phone", client.Phone);

                    command.Parameters.AddWithValue("@CreatedAt", client.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", client.UpdatedAt);

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
                var command = new SQLiteCommand("SELECT * FROM Client", connection);
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

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
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
                var command = new SQLiteCommand("SELECT * FROM Client WHERE ID = @ID", connection);
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

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
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
                var query = @"UPDATE Client
                            SET Name = @Name, Email = @Email, Phone = @Phone,
                                UpdatedAt = @UpdatedAt
                            WHERE Id = @Id";


                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", client.ID.ToString());
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@Phone", client.Phone);

                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    //como a classe é SOMENTE para atualizar, não é necessário colocar CreatedAt

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void Delete(Guid ID)
        {
            var connection = DataBase.GetConnection();
            {
                var query = "DELETE FROM Client WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}
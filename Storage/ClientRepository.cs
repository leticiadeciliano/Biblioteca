using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class ClientRepository
    {
        private string _connectionString = "Data Source=biblioteca.db";

        //Classe Adicionar 
        public void Add(Client client)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //Metódo para inserir
                string query = "INSERT INTO Client (Id, Name, Email, Phone, CreatedAt, UpdatedAt) VALUES (@Id, @Name, @Email, @Phone, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", client.Id);
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@Phone", client.Phone);

                    command.Parameters.AddWithValue("@CreatedAt", client.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", client.UpdatedAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Client> GetAll()
        {
            //lista
            var client = new List<Client>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var command = new SQLiteCommand("SELECT * FROM Client", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var clients = new Client
                        {
                            Id = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
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
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Client WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Client
                        {
                            Id = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
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
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var query = @"UPDATE Client
                            SET Name = @Name, Email = @Email, Phone = @Phone,
                                UpdatedAt = @UpdatedAt
                            WHERE Id = @Id";


                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", client.Id.ToString());
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@Phone", client.Phone);

                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    //como a classe é SOMENTE para atualizar, não é necessário colocar CreatedAt

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void Delete(Guid id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Client WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id.ToString());
                    command.ExecuteNonQuery();
                }  
            }
        }
    }
}
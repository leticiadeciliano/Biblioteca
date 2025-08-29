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
                    command.Parameters.AddWithValue("@ID", client.ID.ToString());
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
            var clients = new List<Client>();

            var connection = DataBase.GetConnection();
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                    //garante que a conexão esteja aberta

                using (var command = new SQLiteCommand(
                    "SELECT ID, Name, Email, Phone, Created_At, Updated_At FROM Clients", connection))
                using (var reader = command.ExecuteReader())
                {
                    int ordId = reader.GetOrdinal("ID");
                    int ordName = reader.GetOrdinal("Name");
                    int ordEmail = reader.GetOrdinal("Email");
                    int ordPhone = reader.GetOrdinal("Phone");
                    
                    int ordCreatedAt = reader.GetOrdinal("Created_At");
                    int ordUpdatedAt = reader.GetOrdinal("Updated_At");

                    while (reader.Read())
                    {
                        Guid id = Guid.Empty;
                        if (!reader.IsDBNull(ordId))
                        {
                            var idStr = reader.GetString(ordId);
                            Guid.TryParse(idStr, out id);
                        }

                        string name = reader.IsDBNull(ordName) ? "" : reader.GetString(ordName);
                        string email = reader.IsDBNull(ordEmail) ? "" : reader.GetString(ordEmail);
                        string phone = reader.IsDBNull(ordPhone) ? "" : reader.GetString(ordPhone);
                        //verifica se a variável é NULL, se for, retorna somente ""

                        DateTime createdAt = DateTime.MinValue;
                        if (!reader.IsDBNull(ordCreatedAt))
                            createdAt = Convert.ToDateTime(reader.GetValue(ordCreatedAt));

                        DateTime updatedAt = DateTime.MinValue;
                        if (!reader.IsDBNull(ordUpdatedAt))
                            updatedAt = Convert.ToDateTime(reader.GetValue(ordUpdatedAt));

                        clients.Add(new Client
                        {
                            ID = id,
                            Name = name,
                            Email = email,
                            Phone = phone,
                            Created_At = createdAt,
                            Updated_At = updatedAt
                        });
                    }
                }
            }

            return clients; //retorna como IEnumerable<Client>
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
                            WHERE ID = @ID";


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
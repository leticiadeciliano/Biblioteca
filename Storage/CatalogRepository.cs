using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

//Primeira tentativa de fazer conexão com o Banco

namespace Storage
{

    //Classe para conectar com o ARQUIVO .bd (onde está o Banco de Dados SQLite)
    public class CatalogRepository
    {
        private string _connectionString = "Data Source=biblioteca.db";

        public void Add(Catalog catalog)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //Metódo para inserir no SQLite
                string query = "INSERT INTO Catalog (ID, Title, Author, Year, Number_pages, Description, Language_Id, Publisher_Id Created_At, Updated_At) VALUES (@ID, @Title, @Author, @Year, @Number_pages, @Description, @Language_Id, @Publisher_Id, @Created_At, @Updated_At)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", catalog.ID);
                    command.Parameters.AddWithValue("@Title", catalog.Title);
                    command.Parameters.AddWithValue("@Author", catalog.Author);
                    command.Parameters.AddWithValue("@Year", catalog.Year);
                    command.Parameters.AddWithValue("@Number_pages", catalog.Number_pages);
                    command.Parameters.AddWithValue("@Year", catalog.Year);
                    command.Parameters.AddWithValue("@Description", catalog.Description);
                    command.Parameters.AddWithValue("@Publisher_Id", catalog.Publisher_ID);
                    command.Parameters.AddWithValue("@Language_Id", catalog.Language_ID);

                    command.Parameters.AddWithValue("@Created_At", catalog.Created_At);
                    command.Parameters.AddWithValue("@Updated_At", catalog.Updated_At);

                    command.ExecuteNonQuery();
                }
            }
        }

        //Classe Listar
        public List<Catalog> GetAll()
        {
            //criando lista
            var catalogs = new List<Catalog>();

            //abrindo conexão com o banco
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var command = new SQLiteCommand("SELECT * FROM Catalog", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var catalog = new Catalog
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string
                            Title = Convert.ToString(reader["Title"]) ?? "",
                            Author = Convert.ToString(reader["Author"]) ?? "",
                            Description = Convert.ToString(reader["Description"]) ?? "",
                            Year = Convert.ToInt32(reader["Year"]),
                            Number_pages = Convert.ToInt32(reader["Number_pages"]),
                            Publisher_ID = Convert.ToInt32(reader["Publisher_ID"]),
                            Language_ID = Convert.ToString(reader["Language_ID"]) ?? "",

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };

                        catalogs.Add(catalog);
                    }
                }
            }

            return catalogs;
        }


        //Classe GetById
        public Catalog GetById(Guid ID)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Catalog WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@Id", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Catalog
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            Title = Convert.ToString(reader["Title"]) ?? "",
                            Author = Convert.ToString(reader["Author"]) ?? "",
                            Description = Convert.ToString(reader["Description"]) ?? "",
                            Year = Convert.ToInt32(reader["Year"]),
                            Number_pages = Convert.ToInt32(reader["Number_pages"]),
                            Publisher_ID = Convert.ToInt32(reader["Publisher_ID"]),
                            Language_ID = Convert.ToString(reader["Language_ID"]) ?? "",

                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };
                    }
                }
            }

            return null!;
        }

        //Class UPDATE
        public void Update(Catalog catalog)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var query = @"UPDATE Catalog 
                            SET Title = @Title, Author = @Author, Year = @Year,
                                Number_pages = @Number_pages, Description = @Description,
                                Publisher_ID = @Publisher_ID, Language_ID = @Language_ID,
                                Updated_At = @Updated_At
                            WHERE ID = @ID";
                            //WHERE utilizado para fazer alteração SOMENTE naquele ID, evitando que outros livros sejam editados

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", catalog.ID.ToString());
                    command.Parameters.AddWithValue("@Title", catalog.Title);
                    command.Parameters.AddWithValue("@Author", catalog.Author);
                    command.Parameters.AddWithValue("@Year", catalog.Year);
                    command.Parameters.AddWithValue("@Number_pages", catalog.Number_pages);
                    command.Parameters.AddWithValue("@Description", catalog.Description);
                    command.Parameters.AddWithValue("Publisher_ID", catalog.Publisher_ID);
                    command.Parameters.AddWithValue("Language_ID", catalog.Language_ID);

                    command.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                    //como a classe é SOMENTE para atualizar, não é necessário colocar Created_At

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
                var query = "DELETE FROM Catalog WHERE ID = @ID";
                //Por enquanto será deleado o ID, que está conectado com os outros elementos de uma tabela

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}

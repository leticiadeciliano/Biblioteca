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
                string query = "INSERT INTO Catalog (ID, Title, Author, Year, Number_pages, Description, CreatedAt, UpdatedAt) VALUES (@ID, @Title, @Author, @Year, @Number_pages, @Description, @CreatedAt, @UpdatedAt)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", catalog.ID);
                    command.Parameters.AddWithValue("@Title", catalog.Title);
                    command.Parameters.AddWithValue("@Author", catalog.Author);
                    command.Parameters.AddWithValue("@Year", catalog.Year);
                    command.Parameters.AddWithValue("@Number_pages", catalog.Number_pages);
                    command.Parameters.AddWithValue("@Year", catalog.Year);
                    command.Parameters.AddWithValue("@Description", catalog.Description);

                    command.Parameters.AddWithValue("@CreatedAt", catalog.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", catalog.UpdatedAt);

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

                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };

                        catalogs.Add(catalog);
                    }
                }
            }

            return catalogs;
        }


        //Classe GetById
        public Catalog GetById(Guid id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Catalog WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Catalog
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            Title = Convert.ToString(reader["Title"]) ?? "",
                            Author = Convert.ToString(reader["Author"]) ?? "",
                            Description = Convert.ToString(reader["Description"]) ?? "",
                            Year = Convert.ToInt32(reader["Year"]),
                            Number_pages = Convert.ToInt32(reader["Number_pages"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
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
                                UpdatedAt = @UpdatedAt
                            WHERE Id = @Id";
                            //WHERE utilizado para fazer alteração SOMENTE naquele ID, evitando que outros livros sejam editados

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", catalog.ID.ToString());
                    command.Parameters.AddWithValue("@Title", catalog.Title);
                    command.Parameters.AddWithValue("@Author", catalog.Author);
                    command.Parameters.AddWithValue("@Year", catalog.Year);
                    command.Parameters.AddWithValue("@Number_pages", catalog.Number_pages);
                    command.Parameters.AddWithValue("@Description", catalog.Description);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    //como a classe é SOMENTE para atualizar, não é necessário colocar CreatedAt

                    command.ExecuteNonQuery();
                }
            }
        }

        //Class DELETE
        public void Delete(Guid id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Catalog WHERE Id = @Id";
                //Por enquanto será deleado o ID, que está conectado com os outros elementos de uma tabela

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id.ToString());
                    command.ExecuteNonQuery();
                } //Tratar caso o ID não exista, SOMENTE na camada de CLI
            }
        }
    }
}

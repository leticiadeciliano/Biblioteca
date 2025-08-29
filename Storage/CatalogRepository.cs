using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;
using Biblioteca.Domain.Interfaces;

//Primeira tentativa de fazer conexão com o Banco

namespace Storage
{

    //Classe para conectar com o ARQUIVO .bd (onde está o Banco de Dados SQLite)
    public class CatalogRepository : ICatalogRepository
    {
        public void Add(Catalog catalog)
        {
            var connection = DataBase.GetConnection();

            string queryCatalog = "INSERT INTO Catalog (ID, Title, Author, Number_pages, Year, Description, Publisher_ID, Language_ID, Created_At, Updated_At) VALUES (@ID, @Title, @Author, @Number_pages, @Year, @Description, @Publisher_ID, @Language_ID, @Created_At, @Updated_At)";
            using (var command = new SQLiteCommand(queryCatalog, connection))
            {
                command.Parameters.AddWithValue("@ID", catalog.ID.ToString());
                command.Parameters.AddWithValue("@Title", catalog.Title);
                command.Parameters.AddWithValue("@Author", catalog.Author);
                command.Parameters.AddWithValue("@Number_pages", catalog.Number_pages);
                command.Parameters.AddWithValue("@Year", catalog.Year);
                command.Parameters.AddWithValue("@Description", catalog.Description);
                command.Parameters.AddWithValue("@Publisher_ID", catalog.Publisher_ID);
                command.Parameters.AddWithValue("@Language_ID", catalog.Language_ID);
                command.Parameters.AddWithValue("@Created_At", catalog.Created_At);
                command.Parameters.AddWithValue("@Updated_At", catalog.Updated_At);

                command.ExecuteNonQuery();
            }

            // Inserir no Inventory automaticamente
            string queryInventory = "INSERT INTO Inventory (Catalog_ID, Condition, Created_At, Updated_At) VALUES (@Catalog_ID, @Condition, @Created_At, @Updated_At)";
            using (var commandInv = new SQLiteCommand(queryInventory, connection))
            {
                commandInv.Parameters.AddWithValue("@Catalog_ID", catalog.ID.ToString());
                commandInv.Parameters.AddWithValue("@Condition", 1); // 1 = bem conservado
                commandInv.Parameters.AddWithValue("@Created_At", catalog.Created_At);
                commandInv.Parameters.AddWithValue("@Updated_At", catalog.Updated_At);

                commandInv.ExecuteNonQuery();
            }
        }


        //Classe List
        public IEnumerable<Catalog> GetAll()
        {
            var catalogs = new List<Catalog>();

            var connection = DataBase.GetConnection();
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                using (var command = new SQLiteCommand(
                    "SELECT ID, Title, Author, Number_pages, Year, Description, Publisher_ID, Language_ID, Created_At, Updated_At FROM Catalog", connection))
                using (var reader = command.ExecuteReader())
                {
                    int ordId = reader.GetOrdinal("ID");
                    int ordTitle = reader.GetOrdinal("Title");
                    int ordAuthor = reader.GetOrdinal("Author");
                    int ordNumber_pages = reader.GetOrdinal("Number_pages");
                    int ordYear = reader.GetOrdinal("Year");
                    int ordDescription = reader.GetOrdinal("Description");
                    int ordPublisher_ID = reader.GetOrdinal("Publisher_ID");
                    int ordLanguage_ID = reader.GetOrdinal("Language_ID");
                    
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

                        string title = reader.IsDBNull(ordTitle) ? "" : reader.GetString(ordTitle);
                        string author = reader.IsDBNull(ordAuthor) ? "" : reader.GetString(ordAuthor);
                        int number_pages = reader.IsDBNull(ordNumber_pages) ? 0 : reader.GetInt32(ordNumber_pages); //Adaptado para int
                        int year = reader.IsDBNull(ordYear) ? 0 : reader.GetInt32(ordYear);
                        string description = reader.IsDBNull(ordDescription) ? "" : reader.GetString(ordDescription);
                        int publisher_ID = reader.IsDBNull(ordPublisher_ID) ? 0 : reader.GetInt32(ordPublisher_ID);
                        int language_ID = reader.IsDBNull(ordLanguage_ID) ? 0 : reader.GetInt32(ordLanguage_ID);
                        //verifica se a variável é NULL, se for, retorna somente ""

                        DateTime createdAt = DateTime.MinValue;
                        if (!reader.IsDBNull(ordCreatedAt))
                            createdAt = Convert.ToDateTime(reader.GetValue(ordCreatedAt));

                        DateTime updatedAt = DateTime.MinValue;
                        if (!reader.IsDBNull(ordUpdatedAt))
                            updatedAt = Convert.ToDateTime(reader.GetValue(ordUpdatedAt));

                        catalogs.Add(new Catalog
                        {
                            ID = id,
                            Title = title,
                            Author = author,
                            Number_pages = number_pages,
                            Year = year,
                            Description = description,
                            Publisher_ID = publisher_ID,
                            Language_ID = language_ID,

                            Created_At = createdAt,
                            Updated_At = updatedAt
                        });
                    }
                }
            }

            return catalogs; //retorna como IEnumerable<Client>
        }


        //Classe GetById
        public Catalog GetById(Guid ID)
        {
            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM Catalog WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

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
                            Language_ID = Convert.ToInt32(reader["Language_ID"]),

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
            var connection = DataBase.GetConnection();
            {
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
            var connection = DataBase.GetConnection();
            {
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

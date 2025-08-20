using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;
using Biblioteca.Domain.Interfaces;

namespace Storage
{
    public class CatalogGenreRepository : ICatalogGenreRepository
    {
        public void Add(CatalogGenre item)
        {
           var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("INSERT INTO CatalogGenre (ID, CatalogID, GenreID, Created_At, Updated_At) VALUES (@ID, @CatalogID, @GenreID, @Created_At, @Updated_At)", connection);

                command.Parameters.AddWithValue("@ID", item.ID.ToString());
                command.Parameters.AddWithValue("@CatalogID", item.CatalogID.ToString());
                command.Parameters.AddWithValue("@GenreID", item.GenreID.ToString());
                command.Parameters.AddWithValue("@Created_At", item.Created_At.ToString("s"));
                command.Parameters.AddWithValue("@Updated_At", item.Updated_At.ToString("s"));

                command.ExecuteNonQuery();
            }
        }
        public CatalogGenre GetById(Guid ID)
        {
            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM CatalogGenre WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", ID.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CatalogGenre
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            CatalogID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            GenreID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            
                            Created_At = Convert.ToDateTime(reader["Created_At"]),
                            Updated_At = Convert.ToDateTime(reader["Updated_At"])
                        };
                    }
                }
            }

            return null!;
        }
        public IEnumerable<CatalogGenre> GetAll()
        {
            var items = new List<CatalogGenre>();

            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM CatalogGenre", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new CatalogGenre
                        {
                            ID = Guid.Parse(reader["ID"].ToString() ?? Guid.Empty.ToString()),
                            CatalogID = Guid.Parse(reader["CatalogID"].ToString() ?? Guid.Empty.ToString()),
                            GenreID = Guid.Parse(reader["GenreID"].ToString() ?? Guid.Empty.ToString()),
                            Created_At = DateTime.Parse(reader["Created_At"].ToString() ?? DateTime.MinValue.ToString()),
                            Updated_At = DateTime.Parse(reader["Updated_At"].ToString() ?? DateTime.MinValue.ToString())
                        };

                        items.Add(item);
                    }
                }
            }

            return items;
        }


        //Class UPDATE
        public void Update(CatalogGenre catalogGenre)
        {
            var connection = DataBase.GetConnection();
            {
                //query
                var query = @"UPDATE CatalogGenre
                            SET CatalogID = @CatalogID, GenreID = @GenreID,
                            Updated_At = @Updated_At
                            WHERE ID = @ID";
                            //WHERE utilizado para fazer alteração SOMENTE naquele ID, evitando que outros livros sejam editados

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", catalogGenre.ID.ToString());
                    command.Parameters.AddWithValue("@CatalogID", catalogGenre.CatalogID.ToString());
                    command.Parameters.AddWithValue("GenreID", catalogGenre.GenreID.ToString());
                    
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
                connection.Open();
                var query = "DELETE FROM CatalogGenre WHERE ID = @ID";
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

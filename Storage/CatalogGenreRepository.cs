using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class CatalogGenreRepository
    {
        public void Add(CatalogGenre item)
        {
           var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("INSERT INTO CatalogGenre (ID, CatalogID, GenreID, CreatedAt, UpdatedAt) VALUES (@ID, @CatalogID, @GenreID, @CreatedAt, @UpdatedAt)", connection);

                command.Parameters.AddWithValue("@ID", item.ID.ToString());
                command.Parameters.AddWithValue("@CatalogID", item.CatalogID.ToString());
                command.Parameters.AddWithValue("@GenreID", item.GenreID.ToString());
                command.Parameters.AddWithValue("@CreatedAt", item.CreatedAt.ToString("s"));
                command.Parameters.AddWithValue("@UpdatedAt", item.UpdatedAt.ToString("s"));

                command.ExecuteNonQuery();
            }
        }

        public List<CatalogGenre> GetAll()
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
                            CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.MinValue.ToString()),
                            UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString() ?? DateTime.MinValue.ToString())
                        };

                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public CatalogGenre GetById(Guid Id)
        {
            var connection = DataBase.GetConnection();
            {
                var command = new SQLiteCommand("SELECT * FROM CatalogGenre WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", Id.ToString());

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
                            
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                    }
                }
            }

            return null!;
        }

        //Class UPDATE
        public void Update(CatalogGenre catalogGenre)
        {
            var connection = DataBase.GetConnection();
            {
                //query
                var query = @"UPDATE CatalogGenre
                            SET CatalogID = @CatalogID, GenreID = @GenreID,
                            UpdatedAt = @UpdatedAt
                            WHERE Id = @Id";
                            //WHERE utilizado para fazer alteração SOMENTE naquele ID, evitando que outros livros sejam editados

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", catalogGenre.ID.ToString());
                    command.Parameters.AddWithValue("@CatalogID", catalogGenre.CatalogID.ToString());
                    command.Parameters.AddWithValue("GenreID", catalogGenre.GenreID.ToString());
                    
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    //como a classe é SOMENTE para atualizar, não é necessário colocar CreatedAt

                    command.ExecuteNonQuery();
                }
            }
        }

        //Class DELETE
        public void Delete(Guid Id)
        {
            var connection = DataBase.GetConnection();
            {
                connection.Open();
                var query = "DELETE FROM CatalogGenre WHERE ID = @ID";
                //Por enquanto será deleado o ID, que está conectado com os outros elementos de uma tabela

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", Id.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}

using System;
using Domain;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Storage
{
    public class CatalogGenreRepository
    {
        private string _connectionString = "Data Source=/home/dev/Biblioteca/Storage/Data/biblioteca.db";
        public void Add(CatalogGenre item)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO CatalogGenre (Id, CatalogId, GenreId, CreatedAt, UpdatedAt) VALUES (@Id, @CatalogId, @GenreId, @CreatedAt, @UpdatedAt)", connection);

                command.Parameters.AddWithValue("@Id", item.ID.ToString());
                command.Parameters.AddWithValue("@CatalogId", item.CatalogID.ToString());
                command.Parameters.AddWithValue("@GenreId", item.GenreID.ToString());
                command.Parameters.AddWithValue("@CreatedAt", item.CreatedAt.ToString("s"));
                command.Parameters.AddWithValue("@UpdatedAt", item.UpdatedAt.ToString("s"));

                command.ExecuteNonQuery();
            }
        }

        public List<CatalogGenre> GetAll()
        {
            var items = new List<CatalogGenre>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM CatalogGenre", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new CatalogGenre
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            CatalogID = Guid.Parse(reader["CatalogId"].ToString() ?? Guid.Empty.ToString()),
                            GenreID = Guid.Parse(reader["GenreId"].ToString() ?? Guid.Empty.ToString()),
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
            using (var connection = new SQLiteConnection(_connectionString))
            {
                //query
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM CatalogGenre WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", Id.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CatalogGenre
                        {
                            ID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            // convertendo ID do banco de TEXT para tipo string e verificando se está NULL
                            CatalogID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            GenreID = Guid.Parse(reader["Id"].ToString() ?? Guid.Empty.ToString()),
                            
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
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                //query
                var query = @"UPDATE CatalogGenre
                            SET CatalogID = @CatalogID, GenreID = @GenreID,
                            UpdatedAt = @UpdatedAt
                            WHERE Id = @Id";
                            //WHERE utilizado para fazer alteração SOMENTE naquele ID, evitando que outros livros sejam editados

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", catalogGenre.ID.ToString());
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
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM CatalogGenre WHERE Id = @Id";
                //Por enquanto será deleado o ID, que está conectado com os outros elementos de uma tabela

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id.ToString());
                    command.ExecuteNonQuery();
                } 
            }
        }
    }
}

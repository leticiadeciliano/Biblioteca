using System;
using System.Collections.Generic;
using Domain;
using Storage;

namespace Service
{
    public class GenreService
    {
        private readonly GenreRepository _genreRepository;

        public GenreService()
        {
            _genreRepository = new GenreRepository();
        }

        public List<Genre> GetAll()
        {
            return (List<Genre>)_genreRepository.GetAll();
        }

        public Genre? GetById(int ID)
        {
            if (ID < 0)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var genre = _genreRepository.GetById(ID);

            if (genre == null)
            {
                Console.WriteLine("Gênero não encontrado.");
                return null;
            }

            return genre;
        }

        public void Create(string name_genre)
        {
            var genre = new Genre
            {
                Name_genre = name_genre,
                
                Created_At = DateTime.Now,
                Updated_At = DateTime.Now
            };

            _genreRepository.Add(genre);
            Console.WriteLine("Gênero adicionado com sucesso!");
        }

        public void Update(int ID, string Name_genre)
        {
            var existingGenre = _genreRepository.GetById(ID);
            if (existingGenre == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            existingGenre.Name_genre = Name_genre;
            
            existingGenre.Updated_At = DateTime.Now;

            _genreRepository.Update(existingGenre);
            Console.WriteLine("Cliente atualizado com sucesso!");
        }

        public void Delete(int ID)
        {
            var existingGenre = _genreRepository.GetById(ID);
            if (existingGenre == null)
            {
                Console.WriteLine("Gênero não encontrado.");
                return;
            }

            _genreRepository.Delete(ID);
            Console.WriteLine("Gênero removido com sucesso!");
        }
    }
}

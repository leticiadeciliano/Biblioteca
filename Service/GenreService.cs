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
            return _genreRepository.GetAll();
        }

        public Genre? GetById(Guid ID)
        {
            if (ID == Guid.Empty)
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
                ID = Guid.NewGuid(),
                Name_genre = name_genre,
                
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _genreRepository.Add(genre);
            Console.WriteLine("Gênero adicionado com sucesso!");
        }

        public void Update(Guid Id, string Name_genre)
        {
            var existingGenre = _genreRepository.GetById(Id);
            if (existingGenre == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            existingGenre.Name_genre = Name_genre;
            
            existingGenre.UpdatedAt = DateTime.Now;

            _genreRepository.Update(existingGenre);
            Console.WriteLine("Cliente atualizado com sucesso!");
        }

        public void Delete(Guid Id)
        {
            var existingGenre = _genreRepository.GetById(Id);
            if (existingGenre == null)
            {
                Console.WriteLine("Gênero não encontrado.");
                return;
            }

            _genreRepository.Delete(Id);
            Console.WriteLine("Gênero removido com sucesso!");
        }
    }
}

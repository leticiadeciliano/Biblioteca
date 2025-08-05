using System;
using System.Collections.Generic;
using Domain;
using Storage;

namespace Service
{
    public class CatalogGenreService
    {
        private readonly CatalogGenreRepository _catalogGenreRepository;

        public CatalogGenreService()
        {
            _catalogGenreRepository = new CatalogGenreRepository();
        }

        public List<CatalogGenre> GetAll()
        {
            return _catalogGenreRepository.GetAll();
        }

        public CatalogGenre? GetById(Guid Id)
        // ? solicita para retornar NULL caso não encontre
        {
            if (Id == Guid.Empty)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var catalogGenre = _catalogGenreRepository.GetById(Id);

            if (catalogGenre == null)
            {
                Console.WriteLine("Gênero de Catálogo não encontrado.");
                return null;
            }

            return catalogGenre;
        }

        public void Create(Guid catalogID, Guid genreID)
        {
            var catalogGenre = new CatalogGenre
            {
                ID = Guid.NewGuid(),
                CatalogID = catalogID,
                GenreID = genreID,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _catalogGenreRepository.Add(catalogGenre);
            Console.WriteLine("Catálogo-Gênero adicionado com sucesso!");
        }

        public void Update(Guid id, Guid catalogID, Guid genreID)
        {
            var existingCatalogGenre = _catalogGenreRepository.GetById(id);
            if (existingCatalogGenre == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return;
            }

            existingCatalogGenre.CatalogID = catalogID;
            existingCatalogGenre.GenreID = genreID;
            

            existingCatalogGenre.UpdatedAt = DateTime.Now;

            _catalogGenreRepository.Update(existingCatalogGenre);
            Console.WriteLine("Gênero de Catálogo atualizado com sucesso!");
        }

        public void Delete(Guid Id)
        {
            var existingCatalogGenre = _catalogGenreRepository.GetById(Id);
            if (existingCatalogGenre == null)
            {
                Console.WriteLine("Gênero de Catálogo não encontrado.");
                return;
            }

            _catalogGenreRepository.Delete(Id);
            Console.WriteLine("Gênero de Catálogo removido com sucesso!");
        } 
    }
}

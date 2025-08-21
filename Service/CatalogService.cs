using System;
using System.Collections.Generic;
using Domain;
using Storage;

//Primeira tentativa

namespace Service
{
    public class CatalogService
    {
        // CatalogRepository chama o arquivo com as informações de conexão com o banco
        // _catalogRepository nome para ser usado dentro da camada Service
        // assim como na lista, um está ligado ao outro
        private readonly CatalogRepository _catalogRepository;

        public CatalogService()
        {
            _catalogRepository = new CatalogRepository();
        }

        public List<Catalog> GetAll()
        {
            return (List<Catalog>)_catalogRepository.GetAll();
        }

        public Catalog? GetById(Guid ID)
        // ? solicita para retornar NULL caso não encontre
        {
            if (ID == Guid.Empty)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var catalog = _catalogRepository.GetById(ID);

            if (catalog == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return null;
            }

            return catalog;
        }

        //Created_At e Updated_At não entram como parâmetro por não ser necessário o Usuário 
        // definir a criação e atualização dos dados
        public void Create(string title, string author, int number_pages, int year, string description, string publisher_ID, string language_ID)
        {
            var catalog = new Catalog
            {
                ID = Guid.NewGuid(),
                Title = title,
                Author = author,
                Year = year,
                Description = description,
                Publisher_ID = publisher_ID,
                Language_ID = language_ID,

                Created_At = DateTime.Now,
                Updated_At = DateTime.Now
            };

            _catalogRepository.Add(catalog);
            Console.WriteLine("Catálogo adicionado com sucesso!");
        }

        public void Update(Guid ID, string title, string author, int number_pages, int year, string description, string publisher_ID, string language_ID)
        {
            var existingCatalog = _catalogRepository.GetById(ID);
            if (existingCatalog == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return;
            }

            existingCatalog.Title = title;
            existingCatalog.Author = author;
            existingCatalog.Number_pages = number_pages;
            existingCatalog.Year = year;
            existingCatalog.Description = description;
            existingCatalog.Publisher_ID = publisher_ID;
            existingCatalog.Language_ID = language_ID;

            existingCatalog.Updated_At = DateTime.Now;

            _catalogRepository.Update(existingCatalog);
            Console.WriteLine("Cliente atualizado com sucesso!");
        }

        public void Delete(Guid ID)
        {
            var existingCatalog = _catalogRepository.GetById(ID);
            if (existingCatalog == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return;
            }

            _catalogRepository.Delete(ID);
            Console.WriteLine("Catálogo removido com sucesso!");
        } //colocar depois condições para excluir a tabela

    }
}

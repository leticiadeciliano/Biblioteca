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
            return _catalogRepository.GetAll();
        }

        public Catalog? GetById(Guid id)
        // ? solicita para retornar NULL caso não encontre
        {
            if (id == Guid.Empty)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var catalog = _catalogRepository.GetById(id);

            if (catalog == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return null;
            }

            return catalog;
        }

        //CreatedAt e UpdatedAt não entram como parâmetro por não ser necessário o Usuário 
        // definir a criação e atualização dos dados
        public void Create(string title, string author, int number_pages, int year, string description, bool is_foreign)
        {
            var catalog = new Catalog
            {
                ID = Guid.NewGuid(),
                Title = title,
                Author = author,
                Year = year,
                Description = description,
                is_foreign = is_foreign,

                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _catalogRepository.Add(catalog);
            Console.WriteLine("Catálogo adicionado com sucesso!");
        }

        public void Update(Guid id, string title, string author, int number_pages, int year, string description, bool is_foreign)
        {
            var existingCatalog = _catalogRepository.GetById(id);
            if (existingCatalog == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return;
            }

            existingCatalog.Title = title;
            existingCatalog.Author = author;
            existingCatalog.Number_pages = number_pages;

            existingCatalog.UpdatedAt = DateTime.Now;

            _catalogRepository.Update(existingCatalog);
            Console.WriteLine("Cliente atualizado com sucesso!");
        }

        public void Delete(Guid Id)
        {
            var existingCatalog = _catalogRepository.GetById(Id);
            if (existingCatalog == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return;
            }

            _catalogRepository.Delete(Id);
            Console.WriteLine("Catálogo removido com sucesso!");
        } //colocar depois condições para excluir a tabela
    }
}

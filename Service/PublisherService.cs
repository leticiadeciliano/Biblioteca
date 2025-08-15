using System;
using System.Collections.Generic;
using Domain;
using Storage;

namespace Service
{
    public class PublisherService
    {
        private readonly PublisherRepository _publisherRepository;

        public PublisherService()
        {
            _publisherRepository = new PublisherRepository();
        }

        public List<Publisher> GetAll()
        {
            return _publisherRepository.GetAll();
        }

        public Publisher? GetById(Guid ID)
        // ? solicita para retornar NULL caso não encontre
        {
            if (ID == Guid.Empty)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var publisher = _publisherRepository.GetById(ID);

            if (publisher == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return null;
            }

            return publisher;
        }
        
        public void Create(string name_publisher)
        {
            var publisher = new Publisher
            {
                ID = Guid.NewGuid(),
                Name_Publisher = name_publisher,

                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // USAR FUTURAMENTE
            //if (string.IsNullOrWhiteSpace(name_publisher))
            //{
            //    Console.WriteLine("Nome da editora não pode estar vazio.");
            //    return;
            //}



            _publisherRepository.Add(publisher);
            Console.WriteLine("Editora adicionada com sucesso!");
        }

        public void Update(Guid ID, string name_publisher)
        {
            var existingPublisher = _publisherRepository.GetById(ID);
            if (existingPublisher == null)
            {
                Console.WriteLine("Editora não encontrada.");
                return;
            }

            existingPublisher.Name_Publisher = name_publisher;

            existingPublisher.UpdatedAt = DateTime.Now;

            _publisherRepository.Update(existingPublisher);
            Console.WriteLine("Editora atualizada com sucesso!");
        }

        public void Delete(Guid ID)
        {
            var existingPublisher = _publisherRepository.GetById(ID);
            if (existingPublisher == null)
            {
                Console.WriteLine("Editora não encontrada.");
                return;
            }

            _publisherRepository.Delete(ID);
            Console.WriteLine("Editora removida com sucesso!");
        } 
    }
}

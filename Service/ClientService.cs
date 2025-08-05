using System;
using System.Collections.Generic;
using Domain;
using Storage;

namespace Service
{
    public class ClientService
    {
        private readonly ClientRepository _clientRepository;

        public ClientService()
        {
            _clientRepository = new ClientRepository();
        }

        public List<Client> GetAll()
        {
            return _clientRepository.GetAll();
        }

        public Client? GetById(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var client = _clientRepository.GetById(Id);

            if (client == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return null;
            }

            return client;
        }

        public void Create(string name, string email, string phone)
        {
            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Phone = phone,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _clientRepository.Add(client);
            Console.WriteLine("Cliente adicionado com sucesso!");
        }

        public void Update(Guid Id, string name, string email, string phone)
        {
            var existingClient = _clientRepository.GetById(Id);
            if (existingClient == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            existingClient.Name = name;
            existingClient.Email = email;
            existingClient.Phone = phone;
            existingClient.UpdatedAt = DateTime.Now;

            _clientRepository.Update(existingClient);
            Console.WriteLine("Cliente atualizado com sucesso!");
        }

        public void Delete(Guid Id)
        {
            var existingClient = _clientRepository.GetById(Id);
            if (existingClient == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            _clientRepository.Delete(Id);
            Console.WriteLine("Cliente removido com sucesso!");
        }
    }
}

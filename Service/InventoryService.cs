using System;
using System.Collections.Generic;
using Domain;
using Storage;

namespace Service
{
    public class InventoryService
    {
        private readonly InventoryRepository _inventoryRepository;

        public InventoryService()
        {
            _inventoryRepository = new InventoryRepository();
        }

        public List<Inventory> GetAll()
        {
            return (List<Inventory>)_inventoryRepository.GetAll();
        }

        public Inventory? GetById(int ID)
        {
            if (ID < 0)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var inventory = _inventoryRepository.GetById(ID);

            if (inventory == null)
            {
                Console.WriteLine("Inventário não encontrado.");
                return null;
            }

            return inventory;
        }

        public void Create(Guid catalogID, int Condition, bool Is_available )
        {
            var inventory = new Inventory
            {
                CatalogID = catalogID,
                Condition = Condition,
                Is_available = Is_available,
                
                Created_At = DateTime.Now,
                Updated_At = DateTime.Now
            };

            _inventoryRepository.Add(inventory);
            Console.WriteLine("Inventário adicionado com sucesso!");
        }

        public void Update(int ID, Guid catalogID, int condition, bool Is_available)
        {
            var existingInventory = _inventoryRepository.GetById(ID);
            if (existingInventory == null)
            {
                Console.WriteLine("Inventário não encontrado.");
                return;
            }

            existingInventory.Condition = condition;
            existingInventory.Is_available = Is_available;
            
            existingInventory.Updated_At = DateTime.Now;

            _inventoryRepository.Update(existingInventory);
            Console.WriteLine("Inventário atualizado com sucesso!");
        }

        public void Delete(int ID)
        {
            var existingInventory = _inventoryRepository.GetById(ID);
            if (existingInventory == null)
            {
                Console.WriteLine("Inventário não encontrado.");
                return;
            }

            _inventoryRepository.Delete(ID);
            Console.WriteLine("Inventário removido com sucesso!");
        }

        internal void Update(int iD, int condition1, string condition, string is_available)
        {
            throw new NotImplementedException();
        }
    }
}

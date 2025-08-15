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
            return _inventoryRepository.GetAll();
        }

        public Inventory? GetById(Guid ID)
        {
            if (ID == Guid.Empty)
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

        public void Create(Guid catalogID, int Condition, bool is_foreign )
        {
            var inventory = new Inventory
            {
                ID = Guid.NewGuid(),
                CatalogID = catalogID,
                Condition = Condition,
                Is_foreign = is_foreign,
                
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _inventoryRepository.Add(inventory);
            Console.WriteLine("Inventário adicionado com sucesso!");
        }

        public void Update(Guid ID, Guid catalogID, int condition, bool is_foreign)
        {
            var existingInventory = _inventoryRepository.GetById(ID);
            if (existingInventory == null)
            {
                Console.WriteLine("Inventário não encontrado.");
                return;
            }

            existingInventory.Condition = condition;
            existingInventory.Is_foreign = is_foreign;
            
            existingInventory.UpdatedAt = DateTime.Now;

            _inventoryRepository.Update(existingInventory);
            Console.WriteLine("Inventário atualizado com sucesso!");
        }

        public void Delete(Guid ID)
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
    }
}

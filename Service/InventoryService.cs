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
            // Evita InvalidCastException se o repo retornar IEnumerable
            return _inventoryRepository.GetAll().ToList();
        }

        public void Create(Guid Catalog_ID, int Condition)
        {
            var inventory = new Inventory
            {
                Catalog_ID = Catalog_ID,
                Condition = Condition,
                
                Created_At = DateTime.Now,
                Updated_At = DateTime.Now
            };

            _inventoryRepository.Add(inventory);
            Console.WriteLine("Inventário adicionado com sucesso!");
        }

        public void Update(int ID, Guid Catalog_ID, int Condition)
        {
            var inventories = _inventoryRepository.GetAll();
            var existingInventory = inventories.FirstOrDefault(i => i.ID == ID);

            if (existingInventory == null)
            {
                Console.WriteLine("Inventário não encontrado.");
                return;
            }

            existingInventory.Catalog_ID = Catalog_ID;
            existingInventory.Condition = Condition;
            existingInventory.Updated_At = DateTime.Now;

            _inventoryRepository.Update(existingInventory);
            Console.WriteLine("Inventário atualizado com sucesso!");
        }

        public void Delete(int ID)
        {
            var inventories = _inventoryRepository.GetAll();
            var existingInventory = inventories.FirstOrDefault(i => i.ID == ID);

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

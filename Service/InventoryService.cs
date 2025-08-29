using Domain;
using Storage;

namespace Service
{
    public class InventoryService
    {
        private readonly InventoryRepository _inventoryRepository;
        private readonly CatalogRepository catalogRepository;

        public InventoryService()
        {
            _inventoryRepository = new InventoryRepository();
            catalogRepository = new CatalogRepository();
        }

        //Básico para retornar somente a Lista
        // public List<Inventory> GetAll()
        // {
        //     // Evita InvalidCastException se o repo retornar IEnumerable
        //     return _inventoryRepository.GetAll().ToList();
        // }
        
        // Função para listar Inventory puro
        public List<(int ID, string Title, Guid Catalog_ID)> ListInventoriesWithCatalog()
        {
            var inventories = _inventoryRepository.GetAll().ToList();
            var result = new List<(int, string, Guid)>();

            foreach (var inv in inventories)
            {
                var catalog = catalogRepository.GetById(inv.Catalog_ID);
                result.Add((inv.ID, catalog?.Title ?? "Desconhecido", inv.Catalog_ID));
            }

            return result;
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

        public void DeleteAllByCatalog_ID(Guid catalog_ID)
        {
            _inventoryRepository.Delete(catalog_ID);
        }

    }
}

using Service;
using Domain;
using CLI.Helpers;

namespace CLI
{
    public static class InventoryCLI
    {
        public static void Menu()
        {
            var inventoryService = new InventoryService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Inventário =====");
                Console.WriteLine("1 - Listar Inventário");
                Console.WriteLine("2 - Adicionar Inventário (quantidade)");
                Console.WriteLine("3 - Procurar Inventário por ID");
                Console.WriteLine("4 - Atualizar Quantidade de um Inventário");
                Console.WriteLine("5 - Deletar Inventário");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListInventory(inventoryService);
                        break;
                    case "2":
                        CreateInventory(inventoryService);
                        break;
                    case "3":
                        GetByIdInventory(inventoryService);
                        break;
                    case "4":
                        UpdateInventory(inventoryService);
                        break;
                    case "5":
                        DeleteInventory(inventoryService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        static void ListInventory(InventoryService inventoryService)
        {
            var inventories = inventoryService.GetAll();
            Console.WriteLine("\n=== Lista de Inventários ===");

            foreach (var inventory in inventories)
            {
                Console.WriteLine(
                    $"ID: {inventory.ID} | CatalogID: {inventory.CatalogID} | {inventory.Is_available}"
                );
            }
        }

        static void CreateInventory(InventoryService inventoryService)
        {
            Console.Write("Digite o ID do Catálogo (Guid): ");
            var catalogID = Guid.Parse(Console.ReadLine() ?? "");

            Console.Write("Digite a condição do exemplar (ex: 1 a 5): ");
            var condition = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Está disponível? ");
            var isAvailable = Console.ReadLine()?.ToLower() == "s";

            inventoryService.Create(catalogID, condition, isAvailable);
            Console.WriteLine("Inventário criado com sucesso!");
        }

        static void GetByIdInventory(InventoryService inventoryService)
        {
            Console.Write("Digite o ID do Inventário: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var ID))
            {
                Console.WriteLine("ID inválido. Operação cancelada.");
                return;
            }

            var inventory = inventoryService.GetById(ID);

            if (inventory == null)
            {
                Console.WriteLine("Inventário não encontrado.");
                return;
            }

            Console.WriteLine("\n=== Inventário Encontrado ===");
            Console.WriteLine($"ID: {inventory.ID}");
            Console.WriteLine($"CatalogID: {inventory.CatalogID}");
            Console.WriteLine($"Condição: {inventory.Condition}");
            Console.WriteLine($"Disponível: {(inventory.Is_available ? "Sim" : "Não")}");
            Console.WriteLine($"Criado em: {inventory.Created_At}");
            Console.WriteLine($"Atualizado em: {inventory.Updated_At}");
        }

        static void UpdateInventory(InventoryService inventoryService)
        {
            var ID = PromptHelper.PromptInt("ID do Inventário a atualizar");
            var catalogID = PromptHelper.PromptGuid("Novo CatalogID");
            var condition = PromptHelper.PromptInt("Nova condição (ex: 1 a 5)");

            Console.Write("Está disponível? (s/n): ");
            var disponivelInput = Console.ReadLine()?.Trim().ToLower();
            bool isAvailable = disponivelInput == "s" || disponivelInput == "sim";

            inventoryService.Update(ID, catalogID, condition, isAvailable);

            Console.WriteLine("Inventário atualizado com sucesso!");
        }



        static void DeleteInventory(InventoryService inventoryService)
        {
            Console.Write("Digite o ID do Inventário a deletar: ");
            var ID = int.Parse(Console.ReadLine() ?? "0");

            inventoryService.Delete(ID);
            Console.WriteLine("Inventário deletado com sucesso!");
        }

    }
}

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
            try
            {
                var inventories = inventoryService.GetAll();
                Console.WriteLine("\n=== Lista de Inventário ===");
                foreach (var inv in inventories)
                {
                    Console.WriteLine($"{inv.ID} - Catálogo: {inv.CatalogID} - Condição: {inv.Condition} - Disponível: {inv.Is_available}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar inventário.");
                LogService.Write("ERROR", $"Erro ao listar inventário: {ex.Message}");
            }
        }

        static void CreateInventory(InventoryService inventoryService)
        {
            try
            {
                Console.Write("Digite o ID do Catálogo (Guid): ");
                var inputCatalogID = Console.ReadLine() ?? "";
                if (!Guid.TryParse(inputCatalogID, out Guid catalogID))
                {
                    Console.WriteLine("ID inválido! Operação cancelada.");
                    return;
                }

                Console.Write("Digite a condição do exemplar (1 a 5): ");
                var inputCondition = Console.ReadLine() ?? "";
                if (!int.TryParse(inputCondition, out int condition) || condition < 1 || condition > 5)
                {
                    Console.WriteLine("Condição inválida! Operação cancelada.");
                    return;
                }

                Console.Write("Está disponível? (s/n): ");
                var isAvailable = Console.ReadLine()?.ToLower() == "s";

                inventoryService.Create(catalogID, condition, isAvailable);

                Console.WriteLine("Inventário criado com sucesso!");
                LogService.Write("INFO", $"Inventário criado: CatalogID {catalogID}, Condição {condition}, Disponível: {isAvailable}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar inventário. Verifique os dados e tente novamente.");
                LogService.Write("ERROR", $"Erro ao criar inventário: {ex.Message}");
            }
        }


        static void GetByIdInventory(InventoryService inventoryService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Inventário");
                if (!int.TryParse(idInput, out int inventoryID))
                {
                    Console.WriteLine("ID inválido!");
                    return;
                }

                var inventory = inventoryService.GetById(inventoryID);
                if (inventory == null)
                {
                    Console.WriteLine("Inventário não encontrado.");
                    return;
                }

                Console.WriteLine("\n=== Inventário Encontrado ===");
                Console.WriteLine($"ID: {inventory.ID}");
                Console.WriteLine($"Catálogo: {inventory.CatalogID}");
                Console.WriteLine($"Condição: {inventory.Condition}");
                Console.WriteLine($"Disponível: {inventory.Is_available}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar inventário.");
                LogService.Write("ERROR", $"Erro ao buscar inventário: {ex.Message}");
            }
        }


        static void UpdateInventory(InventoryService inventoryService)
        {
            try
            {
                var ID = PromptHelper.PromptInt("ID do Inventário a atualizar");

                var inventory = inventoryService.GetById(ID);
                if (inventory == null)
                {
                    Console.WriteLine("Inventário não encontrado.");
                    return;
                }

                // Atualizando campos
                var catalogIDInput = PromptHelper.PromptRequired($"Novo CatalogID (GUID) [{inventory.CatalogID}]: ");
                if (!Guid.TryParse(catalogIDInput, out Guid catalogID))
                {
                    Console.WriteLine("CatalogID inválido!");
                    return;
                }

                var condition = PromptHelper.PromptInt($"Nova condição ({inventory.Condition}): ");
                var isAvailableInput = PromptHelper.PromptRequired($"Está disponível? ({(inventory.Is_available ? "S" : "N")}): ");
                var isAvailable = isAvailableInput.ToLower() == "s";

                // Chamada correta do Update
                inventoryService.Update(ID, catalogID, condition, isAvailable);

                Console.WriteLine("Inventário atualizado com sucesso!");
                LogService.Write("INFO", $"Inventário atualizado: {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar inventário.");
                LogService.Write("ERROR", $"Erro ao atualizar inventário: {ex.Message}");
            }
        }


        static void DeleteInventory(InventoryService inventoryService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Inventário a deletar");
                if (!int.TryParse(idInput, out int inventoryID))
                {
                    Console.WriteLine("ID inválido!");
                    return;
                }

                inventoryService.Delete(inventoryID);

                Console.WriteLine("Inventário deletado com sucesso!");
                LogService.Write("INFO", $"Inventário deletado: {inventoryID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao deletar inventário.");
                LogService.Write("ERROR", $"Erro ao deletar inventário: {ex.Message}");
            }
        }
    }
}

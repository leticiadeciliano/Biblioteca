using Service;
using Domain;
using CLI.Helpers;

namespace CLI
{
    public static class InventoryCLI
    {
        public static void Menu()
        {

            var catalogService = new CatalogService();
            var inventoryService = new InventoryService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Inventário =====");
                Console.WriteLine("1 - Listar Inventário");
                Console.WriteLine("2 - Adicionar Exemplares");
                Console.WriteLine("3 - Atualizar Exemplares");
                Console.WriteLine("4 - Deletar Exemplar");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                Console.Write("Escolha uma opção: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListInventory(inventoryService);
                        break;
                    case "2":
                        CreateInventory(inventoryService, catalogService);
                        break;
                    case "3":
                        UpdateInventory(inventoryService);
                        break;
                    case "4":
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
            var inventories = inventoryService.ListInventoriesWithCatalog();

            if (!inventories.Any())
            {
                Console.WriteLine("Nenhum inventário cadastrado.");
                return;
            }

            Console.WriteLine("\n=== Inventário da Biblioteca ===");
            var grouped = inventories.GroupBy(inv => inv.Catalog_ID);

            foreach (var group in grouped)
            {
                var title = group.First().Title;
                Console.WriteLine($"Livro: {title} (Catalog_ID: {group.Key})");
                Console.WriteLine($"   Exemplares: {group.Count()}");
            }
        }



        static void CreateInventory(InventoryService inventoryService, CatalogService catalogService)
        {
            try
            {
                while (true)
                {
                    var catalogs = catalogService.GetAll();
                    if (catalogs.Count == 0)
                    {
                        Console.WriteLine("Nenhum Livro Cadastrado no Catálogo. Cadastre um Livro primeiro.");
                        return;
                    }

                    Console.WriteLine("\n=== Catálogo de Livros (ID | Título) ===");
                    foreach (var catalog in catalogs)
                        Console.WriteLine($"{catalog.ID} | {catalog.Title}");

                    Console.Write("\nColoque o ID do Catálogo para adicionar Exemplares: ");
                    var inputCatalog = Console.ReadLine() ?? "";
                    if (!Guid.TryParse(inputCatalog, out Guid catalog_ID) || !catalogs.Any(c => c.ID == catalog_ID))
                    {
                        Console.WriteLine("Catalog_ID inválido! Operação cancelada.");
                        return;
                    }

                    Console.Write("Quantos exemplares deseja adicionar? ");
                    var inputQtd = Console.ReadLine() ?? "";
                    if (!int.TryParse(inputQtd, out int quantidade) || quantidade < 1)
                    {
                        Console.WriteLine("Quantidade inválida! Operação cancelada.");
                        return;
                    }

                    for (int i = 1; i <= quantidade; i++)
                    {
                        Console.WriteLine($"\nExemplar {i} de {quantidade}:");

                        Console.Write("Condição (1 a 5) onde 1 bem preservado e 5 pouco preservado: ");
                        var inputCondition = Console.ReadLine() ?? "";
                        if (!int.TryParse(inputCondition, out int condition) || condition < 1 || condition > 5)
                        {
                            Console.WriteLine("Condição inválida! Exemplar ignorado.");
                            continue;
                        }

                        inventoryService.Create(catalog_ID, condition);
                    }

                    Console.Write("\nDeseja adicionar exemplares para outro livro? (s/n): ");
                    var again = Console.ReadLine()?.Trim().ToLower();
                    if (again != "s") break;
                }

                Console.WriteLine("\nExemplares adicionados com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar inventário. Verifique os dados e tente novamente.");
                LogService.Write("ERROR", $"Erro ao criar inventário: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void UpdateInventory(InventoryService inventoryService)
        {
            try
            {
                var ID = PromptHelper.PromptInt("ID do Inventário a atualizar");

                var catalogIdInput = PromptHelper.PromptRequired("Novo Catalog_ID (GUID): ");
                if (!Guid.TryParse(catalogIdInput, out Guid catalog_ID))
                {
                    Console.WriteLine("Catalog_ID inválido!");
                    return;
                }

                var condition = PromptHelper.PromptInt("Nova condição: ");

                inventoryService.Update(ID, catalog_ID, condition);

                Console.WriteLine("Inventário atualizado com sucesso!");
                LogService.Write("INFO", $"Inventário atualizado: {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar inventário.");
                LogService.Write("ERROR", $"Erro ao atualizar inventário: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void DeleteInventory(InventoryService inventoryService)
        {
            var inventories = inventoryService.ListInventoriesWithCatalog();
            if (!inventories.Any())
            {
                Console.WriteLine("Nenhum inventário encontrado.");
                return;
            }

            var grouped = inventories
                .GroupBy(inv => inv.Catalog_ID)
                .Select(g => new { Catalog_ID = g.Key, Title = g.First().Title, Count = g.Count() })
                .ToList();

            Console.WriteLine("\nSelecione o inventário que deseja deletar:");
            for (int i = 0; i < grouped.Count; i++)
            {
                var g = grouped[i];
                Console.WriteLine($"{i + 1} - {g.Title} | Exemplares: {g.Count}");
            }

            var option = PromptHelper.PromptInt("Digite o número da opção");

            if (option < 1 || option > grouped.Count)
            {
                Console.WriteLine("Opção inválida! Operação cancelada.");
                return;
            }

            var selected = grouped[option - 1];

            var confirm = PromptHelper.PromptRequired($"Tem certeza que deseja deletar TODOS os exemplares de '{selected.Title}'? (s/n)");
            if (confirm.ToLower() != "s")
            {
                Console.WriteLine("Operação cancelada.");
                return;
            }

            inventoryService.DeleteAllByCatalog_ID(selected.Catalog_ID);
            Console.WriteLine($"Todos os exemplares de '{selected.Title}' foram removidos com sucesso!");
        }
    }
}

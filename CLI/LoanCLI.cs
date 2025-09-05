using Service;
using CLI.Helpers;

//OBS: Ajustar Create e Update pois o usuário NÃO DEVE ter acesso a criação de ID, Client_ID e Inventory_ID.

namespace CLI
{
    public static class LoanCLI
    {
        public static void Menu()
        {
            var loanService = new LoanService();
            var inventoryService = new InventoryService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Empréstimos =====");
                Console.WriteLine("1 - Listar Empréstimo");
                Console.WriteLine("2 - Adicionar Empréstimo");
                Console.WriteLine("3 - Procurar pelo ID do Empréstimo");
                Console.WriteLine("4 - Atualizar Empréstimo");
                Console.WriteLine("5 - Deletar Empréstimo");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                Console.Write("Escolha uma opção: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListLoan(loanService);
                        break;
                    case "2":
                        CreateLoan(loanService, inventoryService);
                        break;
                    case "3":
                        GetByIdLoan(loanService);
                        break;
                    case "4":
                        UpdateLoan(loanService);
                        break;
                    case "5":
                        DeleteLoan(loanService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        static void ListLoan(LoanService loanService)
        {
            try
            {
                var loans = loanService.GetAll();
                if (loans == null || loans.Count == 0)
                {
                    Console.WriteLine("Nenhum empréstimo cadastrado.");
                    return;
                }

                Console.WriteLine("\n=== Empréstimos Registrados ===");
                foreach (var loan in loans)
                {
                    Console.WriteLine($"\nID do Empréstimo: {loan.ID}");
                    Console.WriteLine($"Cliente (ID): {loan.Client_ID}");
                    Console.WriteLine($"Inventário (ID): {loan.Inventory_ID}");
                    Console.WriteLine($"Data do Empréstimo: {loan.Created_At:dd/MM/yyyy}");
                    Console.WriteLine($"Data de Devolução: {loan.Return_At:dd/MM/yyyy}");

                    string status = loan.IsLate ? "Atrasado" : "Dentro do prazo";
                    Console.WriteLine($"Status: {status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar empréstimos.");
                LogService.Write("ERROR", $"Erro ao listar empréstimos: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void CreateLoan(LoanService loanService, InventoryService inventoryService)
        {
            var inventories = inventoryService.ListInventoriesWithCatalog();

            if (!inventories.Any())
            {
                Console.WriteLine("Nenhum livro disponível no inventário.");
                return;
            }

            // Pega somente a primeira adição de exemplar para usar Inventory_ID
            var grouped = inventories.GroupBy(inv => new { inv.Catalog_ID, inv.Title })
                                    .Select(g => new
                                    {
                                        Catalog_ID = g.Key.Catalog_ID.ToString(),
                                        Title = g.Key.Title,
                                        Inventory_ID = g.First().ID,
                                        Available = g.Count()
                                    }).ToList();

            Console.WriteLine("\n=== Livros disponíveis para empréstimo ===");
            foreach (var item in grouped)
            {
                Console.WriteLine($"Livro: {item.Title} | Catalog_ID: {item.Catalog_ID} | Inventory_ID: {item.Inventory_ID} | Disponíveis: {item.Available}");
            }

            Console.WriteLine("\nDigite o Inventory_ID do livro que deseja emprestar:");
            if (!int.TryParse(Console.ReadLine(), out int chosenId))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var selected = grouped.FirstOrDefault(i => i.Inventory_ID == chosenId);
            if (selected == null)
            {
                Console.WriteLine("Livro não encontrado ou nenhum exemplar disponível.");
                return;
            }

            if (selected.Available <= 0)
            {
                Console.WriteLine("Nenhum exemplar disponível para empréstimo.");
                return;
            }

            Console.WriteLine("Digite o Client_ID (Guid) do cliente:");
            if (!Guid.TryParse(Console.ReadLine(), out Guid clientId))
            {
                Console.WriteLine("Client_ID inválido.");
                return;
            }

            // Cria Loan com 30 dias fixo
            int daysToExpire = 30;
            loanService.CreateLoan(clientId, selected.Inventory_ID, daysToExpire);
            Console.WriteLine($"Empréstimo criado com sucesso para o livro '{selected.Title}'! Retorno em {daysToExpire} dias.");
        }

        static void GetByIdLoan(LoanService loanService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Empréstimo");
                if (!Guid.TryParse(idInput, out Guid ID))
                {
                    Console.WriteLine("ID inválido!");
                    return;
                }

                var loan = loanService.GetById(ID);
                if (loan == null)
                {
                    Console.WriteLine("Empréstimo não encontrado.");
                    return;
                }

                Console.WriteLine($"\nID: {loan.ID}");
                Console.WriteLine($"Cliente: {loan.Client_ID}");
                Console.WriteLine($"Inventário: {loan.Inventory_ID}");
                Console.WriteLine($"Dias até expirar: {loan.Days_to_expire}");
                Console.WriteLine($"Data do Empréstimo: {loan.Created_At}");
                Console.WriteLine($"Data de Devolução: {loan.Return_At}");

                if (loan.IsLate)
                    Console.WriteLine("Status: Atrasado para devolução!");
                else
                    Console.WriteLine("Status: Dentro do prazo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar empréstimo.");
                LogService.Write("ERROR", $"Erro ao buscar empréstimo: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }
        static void UpdateLoan(LoanService loanService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Empréstimo a atualizar");
                if (!Guid.TryParse(idInput, out Guid id))
                {
                    Console.WriteLine("ID inválido!");
                    return;
                }

                var loan = loanService.GetById(id);
                if (loan == null)
                {
                    Console.WriteLine("Empréstimo não encontrado.");
                    return;
                }

                var days_to_expire = PromptHelper.PromptInt($"Novo número de dias ({loan.Days_to_expire}): ");

                loanService.Update(id, days_to_expire);

                Console.WriteLine("Empréstimo atualizado com sucesso!");
                LogService.Write("INFO", $"Empréstimo atualizado: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar empréstimo.");
                LogService.Write("ERROR", $"Erro ao atualizar empréstimo: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }


        static void DeleteLoan(LoanService loanService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Empréstimo a deletar");
                if (!Guid.TryParse(idInput, out Guid ID))
                {
                    Console.WriteLine("ID inválido!");
                    return;
                }

                loanService.Delete(ID);
                Console.WriteLine("Empréstimo deletado com sucesso!");
                LogService.Write("INFO", $"Empréstimo deletado: {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao deletar empréstimo.");
                LogService.Write("ERROR", $"Erro ao deletar empréstimo: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}

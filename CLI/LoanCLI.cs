using Service;
using Domain;
using CLI.Helpers;

//OBS: Ajustar Create e Update pois o usuário NÃO DEVE ter acesso a criação de ID, ClientID e InventoryID.

namespace CLI
{
    public static class LoanCLI
    {
        public static void Menu()
        {
            var loanService = new LoanService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Gênero =====");
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
                        CreateLoan(loanService);
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
                Console.WriteLine("\n=== Lista de Empréstimos ===");
                foreach (var loan in loans)
                {
                    Console.WriteLine($"{loan.ID} - ClientID: {loan.ClientID} - InventoryID: {loan.InventoryID} - Dias até expirar: {loan.Days_to_expire} - Data Empréstimo: {loan.CreatedAt}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar empréstimos.");
                LogService.Write("ERROR", $"Erro ao listar empréstimos: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void CreateLoan(LoanService loanService)
        {
            try
            {
                var clientIDInput = PromptHelper.PromptRequired("ClientID: ");
                if (!Guid.TryParse(clientIDInput, out Guid clientID))
                {
                    Console.WriteLine("ClientID inválido! Operação cancelada.");
                    return;
                }

                var inventoryIDInput = PromptHelper.PromptRequired("InventoryID: ");
                if (!Guid.TryParse(inventoryIDInput, out Guid inventoryID))
                {
                    Console.WriteLine("InventoryID inválido! Operação cancelada.");
                    return;
                }

                var daysToExpire = PromptHelper.PromptInt("Dias para expiração: ");
                if (daysToExpire <= 0)
                {
                    Console.WriteLine("Número de dias inválido! Operação cancelada.");
                    return;
                }

                // Cria empréstimo
                loanService.Create(clientID, inventoryID, daysToExpire);

                Console.WriteLine("Empréstimo criado com sucesso!");
                LogService.Write("INFO", $"Empréstimo criado: ClientID {clientID}, InventoryID {inventoryID}, Dias para expiração {daysToExpire}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar empréstimo. Verifique os dados e tente novamente.");
                LogService.Write("ERROR", $"Erro ao criar empréstimo: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void GetByIdLoan(LoanService loanService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Empréstimo");
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

                Console.WriteLine($"\nID: {loan.ID}\nClientID: {loan.ClientID}\nInventoryID: {loan.InventoryID}\nDias até expirar: {loan.Days_to_expire}\nData Empréstimo: {loan.CreatedAt}");
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

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
            var loans = loanService.GetAll();
            Console.WriteLine("\n=== Lista de Empréstimos ===");
            foreach (var loan in loans)
            {
                Console.WriteLine($"{loan.ID} - {loan.ClientID} - {loan.InventoryID} - {loan.Days_to_expire} - {loan.ReturnAt}");
            }
        }

        static void CreateLoan(LoanService loanService)
        {
            // Recebe ClientID e InventoryID como string e converte para Guid
            var clientIDInput = PromptHelper.PromptRequired("ClientID: ");
            var inventoryIDInput = PromptHelper.PromptRequired("InventoryID: ");

            if (!Guid.TryParse(clientIDInput, out Guid clientID))
            {
                Console.WriteLine("ClientID inválido!");
                return;
            }

            if (!Guid.TryParse(inventoryIDInput, out Guid inventoryID))
            {
                Console.WriteLine("InventoryID inválido!");
                return;
            }

            // Recebe o número de dias de expiração
            var days_to_expire = PromptHelper.PromptInt("Days_to_expire: ");

            loanService.Create(clientID, inventoryID, days_to_expire);

            Console.WriteLine("Empréstimo criado com sucesso!");
        }



        static void GetByIdLoan(LoanService loanService)
        {
            var idInput = PromptHelper.PromptRequired("ID do Empréstimo");

            if (!Guid.TryParse(idInput, out Guid ID))
            {
                Console.WriteLine("ID Inválido! Certifique-se de digitar um número inteiro.");
                return;
            }

            var genre = loanService.GetById(ID);

            if (genre == null)
            {
                Console.WriteLine("Empréstimo não encontrado.");
                return;
            }

            Console.WriteLine("\n=== EMpréstimo Encontrado ===");
            Console.WriteLine($"ID: {genre.ID}");
            Console.WriteLine($"ClientID: {genre.ClientID}");
            Console.WriteLine($"InventoryID: {genre.InventoryID}");
            Console.WriteLine($"Days_to_expire: {genre.Days_to_expire}");
            Console.WriteLine($"ReturnAt: {genre.ReturnAt}");
        }



        static void UpdateLoan(LoanService loanService)
        {
            Console.Write("Digite o ID do Empréstimo a atualizar: ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out Guid ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            var existingLoan = loanService.GetById(ID);
            if (existingLoan == null)
            {
                Console.WriteLine("Empréstimo não encontrado.");
                return;
            }

            var newDays = PromptHelper.PromptInt("Novo Days_to_expire: ");
            loanService.Update(newDays);
        } //OBS: O usuário não deve ter acesso a edição do ID, ClientID e InventoryID por serem informações geradas pelo
        //código


        static void DeleteLoan(LoanService loanService)
        {
            Console.Write("Digite o ID do Gênero a Deletar: ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out Guid ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            loanService.Delete(ID);
            Console.WriteLine("Gênero Deletado com Sucesso!");
        }
    }
}

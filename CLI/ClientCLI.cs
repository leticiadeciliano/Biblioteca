using Service;
using Domain;
using CLI.Helpers;

namespace CLI
{
    public static class ClientCLI
    {
        public static void Menu()
        {
            var clientService = new ClientService();

            while (true)
            {
                Console.WriteLine("\n===== MENU CLIENTES ====="); // \n para pular linha.
                Console.WriteLine("1 - Listar Clientes");
                Console.WriteLine("2 - Adicionar Cliente");
                Console.WriteLine("3 - Procurar pelo ID");
                Console.WriteLine("4 - Atualizar Cliente");
                Console.WriteLine("5 - Deletar Cliente");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                Console.Write("Escolha uma opção: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListClient(clientService);
                        break;
                    case "2":
                        CreateClient(clientService);
                        break;
                    case "3":
                        GetByIdClient(clientService);
                        break;
                    case "4":
                        UpdateClient(clientService);
                        break;
                    case "5":
                        DeleteClient(clientService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        static void ListClient(ClientService clientService)
        {
            try
            {
                var clients = clientService.GetAll();
                Console.WriteLine("\n=== Lista de Clientes ===");
                foreach (var client in clients)
                {
                    Console.WriteLine($"{client.ID} - {client.Name} - {client.Email} - {client.Phone}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar clientes.");
                LogService.Write("ERROR", $"Erro ao listar clientes: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }


        static void CreateClient(ClientService clientService)
        {
            try
            {
                var name = PromptHelper.PromptRequired("Nome: ");
                var email = PromptHelper.PromptRequired("Email: ");
                var phone = PromptHelper.PromptRequired("Telefone: ");

                var newClient = new Client
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    Phone = phone
                };

                clientService.Create(name, email, phone);
                Console.WriteLine("Cliente criado com Sucesso!");

                LogService.Write("INFO", $"Cliente criado: {newClient.ID} - {newClient.Name} - {newClient.Email} - {newClient.Phone}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar cliente. Verifique os dados e tente novamente.");
                LogService.Write("ERROR", $"Erro ao criar cliente: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
                }
        }


        static void GetByIdClient(ClientService clientService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Cliente");

                if (!Guid.TryParse(idInput, out Guid ID))
                {
                    Console.WriteLine("ID inválido!");
                    return;
                }

                var client = clientService.GetById(ID);
                if (client == null)
                {
                    Console.WriteLine("Cliente não encontrado.");
                    return;
                }

                Console.WriteLine("\n=== Cliente Encontrado ===");
                Console.WriteLine($"ID: {client.ID}");
                Console.WriteLine($"Nome: {client.Name}");
                Console.WriteLine($"Email: {client.Email}");
                Console.WriteLine($"Telefone: {client.Phone}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar cliente.");
                LogService.Write("ERROR", $"Erro ao buscar cliente: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void UpdateClient(ClientService clientService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Cliente a atualizar");

                if (!Guid.TryParse(idInput, out Guid ID))
                {
                    Console.WriteLine("ID inválido!");
                    return;
                }

                var client = clientService.GetById(ID);
                if (client == null)
                {
                    Console.WriteLine("Cliente não encontrado.");
                    return;
                }

                var name = PromptHelper.PromptRequired($"Novo nome ({client.Name}): ");
                var email = PromptHelper.PromptRequired($"Novo email ({client.Email}): ");
                var phone = PromptHelper.PromptRequired($"Novo telefone ({client.Phone}): ");

                clientService.Update(ID, name, email, phone);

                Console.WriteLine("Cliente atualizado com sucesso!");
                LogService.Write("INFO", $"Cliente atualizado: {ID} - {name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar cliente.");
                LogService.Write("ERROR", $"Erro ao atualizar cliente: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }


        static void DeleteClient(ClientService clientService)
        {
            try
            {
                var idInput = PromptHelper.PromptRequired("ID do Cliente a deletar");

                if (!Guid.TryParse(idInput, out Guid ID))
                {
                    Console.WriteLine("ID inválido!");
                    return;
                }

                clientService.Delete(ID);
                Console.WriteLine("Cliente deletado com sucesso!");
                LogService.Write("INFO", $"Cliente deletado: {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao deletar cliente.");
                LogService.Write("ERROR", $"Erro ao deletar cliente: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}

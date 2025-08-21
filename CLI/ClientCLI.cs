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

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListClients(clientService);
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

        static void ListClients(ClientService clientService)
        {
            var clients = clientService.GetAll();
            Console.WriteLine("\n=== Lista de Clientes ===");
            foreach (var client in clients)
            {
                Console.WriteLine($"{client.ID} - {client.Name} - {client.Email} - {client.Phone}");
            }
        }

        static void CreateClient(ClientService clientService)
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
        }


        static void GetByIdClient(ClientService clientService)
        {
            var idInput = PromptHelper.PromptRequired("ID do Cliente");

            //Convertendo de ID para GUID
            if (!Guid.TryParse(idInput, out Guid ID))
            {
                Console.WriteLine("ID Inválido! Certifique-se de digitar um GUID correto.");
                return;
            }

            var client = clientService.GetById(ID);

            if (client == null)
            {
                Console.WriteLine("Cliente não Encontrado.");
                return;
            }

            Console.WriteLine("\n=== Cliente Encontrado ===");
            Console.WriteLine($"ID: {client.ID}");
            Console.WriteLine($"Nome: {client.Name}");
            Console.WriteLine($"Email: {client.Email}");
            Console.WriteLine($"Telefone: {client.Phone}");
        }


        static void UpdateClient(ClientService clientService)
        {
            Console.Write("Digite o ID do Cliente a Atualizar: ");
            var input = Console.ReadLine();

            //validando o ID
            if (!Guid.TryParse(input, out var ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            Console.Write("Novo Nome: ");
            var name = PromptHelper.PromptRequired("Nome");
            Console.Write("Novo Email: ");
            var email = PromptHelper.PromptRequired("Email");
            Console.Write("Novo Telefone: ");
            var phone = PromptHelper.PromptRequired("Telefone");

            clientService.Update(ID, name, email, phone);
            Console.WriteLine("Cliente Atualizado com Sucesso!");
        }

        static void DeleteClient(ClientService clientService)
        {
            Console.Write("Digite o ID do Cliente a Deletar: ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out var ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            clientService.Delete(ID);
            Console.WriteLine("Cliente Deletado com Sucesso!");
        }
    }
}

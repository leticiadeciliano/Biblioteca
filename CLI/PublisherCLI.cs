using Service;
using Domain;
using CLI.Helpers;

namespace CLI
{
    public static class PublisherCLI
    {
        public static void Menu()
        {
            var publisherService = new PublisherService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Editora =====");
                Console.WriteLine("1 - Listar Editora");
                Console.WriteLine("2 - Adicionar Editora");
                Console.WriteLine("3 - Procurar pelo ID da Editora");
                Console.WriteLine("4 - Atualizar Editora do Livro");
                Console.WriteLine("5 - Deletar Editora");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListPublisher(publisherService);
                        break;
                    case "2":
                        CreatePublisher(publisherService);
                        break;
                    case "3":
                        GetByIdPublisher(publisherService);
                        break;
                    case "4":
                        UpdatePublisher(publisherService);
                        break;
                    case "5":
                        DeletePublisher(publisherService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        static void ListPublisher(PublisherService publisherService)
        {
            var publishers = publisherService.GetAll();
            Console.WriteLine("\n=== Lista de Editoras ===");
            foreach (var publisher in publishers)
            {
                Console.WriteLine($"{publisher.ID} - {publisher.Name_Publisher}");
            }
        }

        static void CreatePublisher(PublisherService PublisherService)
        {
            var ID = PromptHelper.PromptInt("ID: ");
            var name_Publisher = PromptHelper.PromptRequired("Name_Publisher: ");

            var newpublisher = new Publisher
            {
                ID = ID,
                Name_Publisher = name_Publisher,

            };

            PublisherService.Create(ID, name_Publisher);
            Console.WriteLine("Editora Criada com Sucesso!");
        }


        static void GetByIdPublisher(PublisherService PublisherService)
        {
            var idInput = PromptHelper.PromptRequired("ID da Editora");

            if (!int.TryParse(idInput, out int ID))
            {
                Console.WriteLine("ID Inválido! Certifique-se de digitar um número inteiro.");
                return;
            }

            var publisher = PublisherService.GetById(ID);

            if (publisher == null)
            {
                Console.WriteLine("Editora não encontrada.");
                return;
            }

            Console.WriteLine("\n=== Editora Encontrada ===");
            Console.WriteLine($"ID: {publisher.ID}");
            Console.WriteLine($"Nome: {publisher.Name_Publisher}");
        }



        static void UpdatePublisher(PublisherService PublisherService)
        {
            Console.Write("Digite o ID da Editora a atualizar: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            var existingGenre = PublisherService.GetById(ID);
            if (existingGenre == null)
            {
                Console.WriteLine("Editora não encontrada.");
                return;
            }

            Console.Write("Novo Name_Publisher: ");
            var Name_Publisher = PromptHelper.PromptRequired("Name_Publisher");

            PublisherService.Update(ID, Name_Publisher);
            Console.WriteLine("Editora atualizada com sucesso!");
        } 

        static void DeletePublisher(PublisherService PublisherService)
        {
            Console.Write("Digite o ID da Editora a Deletar: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            PublisherService.Delete(ID);
            Console.WriteLine("Editora Deletada com Sucesso!");
        }
    }
}

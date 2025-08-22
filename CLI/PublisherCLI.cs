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

                Console.Write("Escolha uma opção: ");
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
            try
            {
                var publishers = publisherService.GetAll();
                Console.WriteLine("\n=== Lista de Editoras ===");
                foreach (var pub in publishers)
                {
                    Console.WriteLine($"{pub.ID} - {pub.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar editoras.");
                LogService.Write("ERROR", $"Erro ao listar editoras: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void CreatePublisher(PublisherService publisherService)
        {
            try
            {
                var ID = PromptHelper.PromptInt("ID: ");
                var name = PromptHelper.PromptRequired("Name: ");

                var newPublisher = new Publisher
                {
                    ID = ID,
                    Name = name
                };

                publisherService.Create(ID, name);
                Console.WriteLine("Editora criada com sucesso!");
                LogService.Write("INFO", $"Editora criada: {ID} - {name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar editora. Verifique os dados e tente novamente.");
                LogService.Write("ERROR", $"Erro ao criar editora: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }


        static void GetByIdPublisher(PublisherService publisherService)
        {
            try
            {
                var ID = PromptHelper.PromptInt("ID da Editora");
                var pub = publisherService.GetById(ID);
                if (pub == null)
                {
                    Console.WriteLine("Editora não encontrada.");
                    return;
                }

                Console.WriteLine($"ID: {pub.ID}\nNome: {pub.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar Editora.");
                LogService.Write("ERROR", $"Erro ao buscar editora: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void UpdatePublisher(PublisherService publisherService)
        {
            try
            {
                var ID = PromptHelper.PromptInt("ID da Editora a atualizar");
                var pub = publisherService.GetById(ID);
                if (pub == null)
                {
                    Console.WriteLine("Editora não encontrada.");
                    return;
                }

                var newName = PromptHelper.PromptRequired($"Novo Nome ({pub.Name}): ");
                publisherService.Update(ID, newName);

                Console.WriteLine("Editora atualizada com sucesso!");
                LogService.Write("INFO", $"Editora atualizada: {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar Editora.");
                LogService.Write("ERROR", $"Erro ao atualizar editora: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void DeletePublisher(PublisherService publisherService)
        {
            try
            {
                var ID = PromptHelper.PromptInt("ID da Editora a deletar");
                publisherService.Delete(ID);
                Console.WriteLine("Editora deletada com sucesso!");
                LogService.Write("INFO", $"Editora deletada: {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao deletar editora.");
                LogService.Write("ERROR", $"Erro ao deletar editora: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}

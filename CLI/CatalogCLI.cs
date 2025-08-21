using Service;
using Domain;
using CLI.Helpers;

namespace CLI
{
    public static class CatalogCLI
    {
        public static void Menu()
        {
            var catalogService = new CatalogService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Catálogo =====");
                Console.WriteLine("1 - Listar Catálogos");
                Console.WriteLine("2 - Adicionar Catálogo");
                Console.WriteLine("3 - Procurar pelo ID do Catálogo");
                Console.WriteLine("4 - Atualizar Catálogo de Livro");
                Console.WriteLine("5 - Deletar Catálogo");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListCatalog(catalogService);
                        break;
                    case "2":
                        CreateCatalog(catalogService);
                        break;
                    case "3":
                        GetByIdCatalog(catalogService);
                        break;
                    case "4":
                        UpdateCatalog(catalogService);
                        break;
                    case "5":
                        DeleteCatalog(catalogService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        static void ListCatalog(CatalogService catalogService)
        {
            var catalogs = catalogService.GetAll();
            Console.WriteLine("\n=== Lista de cataloges ===");
            foreach (var catalog in catalogs)
            {
                Console.WriteLine($"{catalog.ID} - {catalog.Title} - {catalog.Author} - {catalog.Number_pages}  - {catalog.Year} - {catalog.Description} - {catalog.Publisher_ID} - {catalog.Language_ID}");
            }
        }

        static void CreateCatalog(CatalogService catalogService)
        {
            var title = PromptHelper.PromptRequired("Title: ");
            var author = PromptHelper.PromptRequired("Author: ");
            int number_pages = PromptHelper.PromptInt("Number_pages: ");
            int year = PromptHelper.PromptInt("Year: ");
            var description = PromptHelper.PromptRequired("Description: ");
            var publisher_ID = PromptHelper.PromptRequired("Publisher_ID: ");
            var language_ID = PromptHelper.PromptRequired("Language_ID: ");

            var newcatalog = new Catalog
            {
                ID = Guid.NewGuid(),
                Title = title,
                Author = author,
                Number_pages = number_pages,
                Year = year,
                Description = description,
                Publisher_ID = publisher_ID,
                Language_ID = language_ID 

            };

            catalogService.Create(title, author, number_pages, year, description, publisher_ID, language_ID );
            Console.WriteLine("Catalogo Criado com Sucesso!");
        }


        static void GetByIdCatalog(CatalogService catalogService)
        {
            var idInput = PromptHelper.PromptRequired("ID do Catálogo");

            if (!Guid.TryParse(idInput, out Guid ID))
            {
                Console.WriteLine("ID Inválido! Certifique-se de digitar um GUID correto.");
                return;
            }

            var catalog = catalogService.GetById(ID);

            if (catalog == null)
            {
                Console.WriteLine("Catálogo não Encontrado.");
                return;
            }

            Console.WriteLine("\n=== cataloge Encontrado ===");
            Console.WriteLine($"ID: {catalog.ID}");
            Console.WriteLine($"Nome: {catalog.Title}");
            Console.WriteLine($"Email: {catalog.Author}");
            Console.WriteLine($"Telefone: {catalog.Number_pages}");
            Console.WriteLine($"Nome: {catalog.Year}");
            Console.WriteLine($"Nome: {catalog.Description}");
            Console.WriteLine($"Nome: {catalog.Publisher_ID}");
            Console.WriteLine($"Nome: {catalog.Language_ID}");
        }


        static void UpdateCatalog(CatalogService catalogService)
        {
            Console.Write("Digite o ID do Catálogo a Atualizar: ");
            var input = Console.ReadLine();

            //validando o ID
            if (!Guid.TryParse(input, out var ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            Console.Write("Novo Título: ");
            var title = PromptHelper.PromptRequired("Título");
            Console.Write("Novo Author: ");
            var author = PromptHelper.PromptRequired("Author");
            Console.Write("Novo Número de Páginas: ");
            var number_pages = PromptHelper.PromptInt("Número de Páginas");
            Console.Write("Novo Ano: ");
            var year = PromptHelper.PromptInt("Ano");
            Console.Write("Nova Descrição: ");
            var description = PromptHelper.PromptRequired("Descrição");
            Console.Write("Novo Descrição: ");
            var  publisher_ID = PromptHelper.PromptRequired("Publisher_ID");
            Console.Write("Novo Publisher_ID: ");
            var language_ID = PromptHelper.PromptRequired("Language_ID");
            Console.Write("Novo Language_ID: ");


            catalogService.Update(ID, title, author, number_pages, year, description, publisher_ID, language_ID);
            Console.WriteLine("Catálogo Atualizado com Sucesso!");
        }

        static void DeleteCatalog(CatalogService catalogService)
        {
            Console.Write("Digite o ID do Catálogo a Deletar: ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out var ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            catalogService.Delete(ID);
            Console.WriteLine("Catálogo Deletado com Sucesso!");
        }
    }
}

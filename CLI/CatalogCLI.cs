using Service;
using Domain;
using CLI.Helpers;
using System.Data.Common;

namespace CLI
{
    public static class CatalogCLI
    {
        public static void Menu()
        {
            var catalogService = new CatalogService();
            var publisherService = new PublisherService();
            var genreService = new GenreService();
            var languageService = new LanguageService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Catálogo =====");
                Console.WriteLine("1 - Listar Livros");
                Console.WriteLine("2 - Cadastrar Livro");
                Console.WriteLine("3 - Procurar pelo ID");
                Console.WriteLine("4 - Atualizar Livro");
                Console.WriteLine("5 - Deletar Livro");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                Console.Write("Escolha uma opção: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListCatalog(catalogService);
                        break;

                    case "2":
                        CreateCatalog(catalogService, publisherService, genreService, languageService);
                        break;

                    case "3":
                        GetByIdCatalog(catalogService);
                        break;

                    case "4":
                        UpdateCatalog(catalogService); //futuramente passar publisherService, genreService, languageService p/ ser compatível com o CreateCatalog.
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
            try
            {
                var catalogs = catalogService.GetAll();
                Console.WriteLine("\n=== Lista de Livros ===");
                foreach (var catalog in catalogs)
                {
                    Console.WriteLine($"{catalog.ID} - {catalog.Title} - {catalog.Author} - {catalog.Number_pages} - {catalog.Year} - {catalog.Description} - {catalog.Publisher_ID} - {catalog.Language_ID}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar catálogo.");
                LogService.Write("ERROR", $"Erro ao listar catálogo: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        //OBS: Fazendo função a mais para, na hora de adicionar um livro, ele tenha a opção de já cadastrar a 
        //Editora (publisher) e o Gênero (Genre) sem precisar ir no Menu de ambos.
        static void CreateCatalog(
            CatalogService catalogService, 
            PublisherService publisherService, 
            GenreService genreService, 
            LanguageService languageService)
        {
            Console.WriteLine("=== Criar Novo Livro ===");

            var title = PromptHelper.PromptRequired("Título");
            var author = PromptHelper.PromptRequired("Autor");
            var numberPages = PromptHelper.PromptInt("Número de páginas");
            var year = PromptHelper.PromptInt("Ano");
            var description = PromptHelper.PromptRequired("Descrição");

            //Publisher
            Console.WriteLine("Editoras já cadastradas: ");
            foreach (var pub in publisherService.GetAll())
                Console.WriteLine($"{pub.ID} - {pub.Name}");

            string publisherChoice = PromptHelper.PromptRequired("Digite o ID da Editora existente ou digite 'novo' para criar");
            int publisherID;

            if (publisherChoice.ToLower() == "novo")
            {
                int ID = publisherService.GetAll().Any() ? publisherService.GetAll().Max(p => p.ID) + 1 : 1;
                string newPublisherName = PromptHelper.PromptRequired("Nome da Editora: ");

                publisherService.Create(ID, newPublisherName);

                var created = publisherService.GetAll().FirstOrDefault(p => p.Name == newPublisherName);
                if (created == null)
                {
                    Console.WriteLine("Erro ao criar Publisher.");
                    return;
                }
                publisherID = created.ID;
            }
            else
            {
                publisherID = int.Parse(publisherChoice);
            }

            //Genre
            Console.WriteLine("Gêneros já Cadastrados:");
            foreach (var g in genreService.GetAll())
                Console.WriteLine($"{g.ID} - {g.Name_genre}");

            string genreChoice = PromptHelper.PromptRequired("Digite o ID do Genre existente ou 'novo' para criar");
            int genreID;

            if (genreChoice.ToLower() == "novo")
            {
                string newGenreName = PromptHelper.PromptRequired("Nome do Gênero: ");
                genreService.Create(newGenreName);

                var createdGenre = genreService.GetAll().FirstOrDefault(g => g.Name_genre == newGenreName);
                if (createdGenre == null)
                {
                    Console.WriteLine("Erro ao criar Genre.");
                    return;
                }
                genreID = createdGenre.ID;
            }
            else
            {
                genreID = int.Parse(genreChoice);
            }

            //Language
            Console.WriteLine("Idiomas existentes:");
            foreach (var lang in languageService.GetAll())
                Console.WriteLine($"{lang.ID} - {lang.Name}");

            string languageChoice = PromptHelper.PromptRequired("Digite o ID do Language existente ou 'novo' para criar");
            int languageID;

            if (languageChoice.ToLower() == "novo")
            {
                int newLanguageID = languageService.GetAll().Any()
                    ? languageService.GetAll().Max(l => l.ID) + 1
                    : 1;

                string newLanguageName = PromptHelper.PromptRequired("Nome do novo Language: ");
                languageService.Create(newLanguageID, newLanguageName, Guid.NewGuid());

                var createdLang = languageService.GetAll().FirstOrDefault(l => l.Name == newLanguageName);
                if (createdLang == null)
                {
                    Console.WriteLine("Erro ao criar Language.");
                    return;
                }
                languageID = createdLang.ID;
            }
            else
            {
                languageID = int.Parse(languageChoice);
            }

            //Criar Catalog
            try
            {
                Guid catalogID = Guid.NewGuid();

                catalogService.Create(
                    catalogID,
                    title,
                    author,
                    numberPages,
                    year,
                    description,
                    publisherID,
                    languageID
                );

                Console.WriteLine("Livro criado com sucesso!");
                LogService.Write("INFO", $"Livro criado: {title} (ID: {catalogID})");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao criar o livro.");
                LogService.Write("ERROR", $"Erro ao criar livro: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

            static void GetByIdCatalog(CatalogService catalogService)
            {
                try
                {
                    var idInput = PromptHelper.PromptRequired("ID do Catálogo");

                    if (!Guid.TryParse(idInput, out Guid ID))
                    {
                        Console.WriteLine("ID inválido!");
                        return;
                    }

                    var catalog = catalogService.GetById(ID);
                    if (catalog == null)
                    {
                        Console.WriteLine("Catálogo não encontrado.");
                        return;
                    }

                    Console.WriteLine("\n=== Catálogo Encontrado ===");
                    Console.WriteLine($"ID: {catalog.ID}");
                    Console.WriteLine($"Título: {catalog.Title}");
                    Console.WriteLine($"Autor: {catalog.Author}");
                    Console.WriteLine($"Número de Páginas: {catalog.Number_pages}");
                    Console.WriteLine($"Ano: {catalog.Year}");
                    Console.WriteLine($"Descrição: {catalog.Description}");
                    Console.WriteLine($"Publisher ID: {catalog.Publisher_ID}");
                    Console.WriteLine($"Language ID: {catalog.Language_ID}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao buscar catálogo.");
                    LogService.Write("ERROR", $"Erro ao buscar catálogo: {ex.Message}");
                    LogHelper.Error($"StackTrace: {ex.StackTrace}");
                }
            }

            static void UpdateCatalog(CatalogService catalogService)
            {
                try
                {
                    var idInput = PromptHelper.PromptRequired("ID do Catálogo a atualizar");

                    if (!Guid.TryParse(idInput, out Guid ID))
                    {
                        Console.WriteLine("ID inválido!");
                        return;
                    }

                    var catalog = catalogService.GetById(ID);
                    if (catalog == null)
                    {
                        Console.WriteLine("Catálogo não encontrado.");
                        return;
                    }

                    var title = PromptHelper.PromptRequired($"Novo título ({catalog.Title}): ");
                    var author = PromptHelper.PromptRequired($"Novo autor ({catalog.Author}): ");
                    var numberPages = PromptHelper.PromptInt($"Novo número de páginas ({catalog.Number_pages}): ");
                    var year = PromptHelper.PromptInt($"Novo ano ({catalog.Year}): ");
                    var description = PromptHelper.PromptRequired($"Nova descrição ({catalog.Description}): ");

                    // Mantemos PublisherID e LanguageID originais
                    catalogService.Update(ID, title, author, numberPages, year, description);

                    Console.WriteLine("Catálogo atualizado com sucesso!");
                    LogService.Write("INFO", $"Catálogo atualizado: {ID} - {title}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao atualizar catálogo.");
                    LogService.Write("ERROR", $"Erro ao atualizar catálogo: {ex.Message}");
                    LogHelper.Error($"StackTrace: {ex.StackTrace}");
                }
            }




            static void DeleteCatalog(CatalogService catalogService)
            {
                try
                {
                    var idInput = PromptHelper.PromptRequired("ID do Catálogo a deletar");

                    if (!Guid.TryParse(idInput, out Guid ID))
                    {
                        Console.WriteLine("ID inválido!");
                        return;
                    }

                    catalogService.Delete(ID);
                    Console.WriteLine("Catálogo deletado com sucesso!");
                    LogService.Write("INFO", $"Catálogo deletado: {ID}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao deletar catálogo.");
                    LogService.Write("ERROR", $"Erro ao deletar catálogo: {ex.Message}");
                    LogHelper.Error($"StackTrace: {ex.StackTrace}");
                }
            }
        }
    }

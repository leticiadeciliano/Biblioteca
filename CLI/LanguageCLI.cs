using Service;
using Domain;
using CLI.Helpers;

namespace CLI
{
    public static class LanguageCLI
    {
        public static void Menu()
        {
            var languageService = new LanguageService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Gênero =====");
                Console.WriteLine("1 - Listar Idioma");
                Console.WriteLine("2 - Adicionar Idioma");
                Console.WriteLine("3 - Procurar pelo ID do Idioma");
                Console.WriteLine("4 - Atualizar Idioma");
                Console.WriteLine("5 - Deletar Idioma");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                Console.Write("Escolha uma opção: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListLanguage(languageService);
                        break;
                    case "2":
                        CreateLanguage(languageService);
                        break;
                    case "3":
                        GetByIdLanguage(languageService);
                        break;
                    case "4":
                        UpdateLanguage(languageService);
                        break;
                    case "5":
                        DeleteLanguage(languageService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        static void ListLanguage(LanguageService languageService)
        {
            try
            {
                var languages = languageService.GetAll();
                Console.WriteLine("\n=== Lista de Idiomas ===");
                foreach (var lang in languages)
                {
                    Console.WriteLine($"{lang.ID} - {lang.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar idiomas.");
                LogService.Write("ERROR", $"Erro ao listar idiomas: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void CreateLanguage(LanguageService languageService)
        {
            try
            {
                string name = PromptHelper.PromptRequired("Nome do Idioma: ");

                int newID = languageService.GetAll().Any()
                    ? languageService.GetAll().Max(l => l.ID) + 1
                    : 1;

                Guid newGuid = Guid.NewGuid();

                // Chama o método existente no Service
                languageService.Create(newID, name, newGuid);

                Console.WriteLine("Idioma Criado com Sucesso!");
                LogHelper.Info($"Idioma criado: {name} (ID: {newID}, GUID: {newGuid})");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"Erro ao criar idioma: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }


        static void GetByIdLanguage(LanguageService languageService)
        {
            try
            {
                var input = PromptHelper.PromptInt("Digite o ID do idioma");
                var lang = languageService.GetById(input);

                if (lang == null)
                {
                    Console.WriteLine("Idioma não encontrado.");
                    return;
                }

                Console.WriteLine("\n=== Idioma Encontrado ===");
                Console.WriteLine($"ID: {lang.ID}");
                Console.WriteLine($"Nome: {lang.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar idioma.");
                LogService.Write("ERROR", $"Erro ao buscar idioma: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void UpdateLanguage(LanguageService languageService)
        {
            try
            {
                var ID = PromptHelper.PromptInt("ID do idioma a atualizar");
                var lang = languageService.GetById(ID);

                if (lang == null)
                {
                    Console.WriteLine("Idioma não encontrado.");
                    return;
                }

                var name = PromptHelper.PromptRequired($"Novo nome ({lang.Name}): ");

                // Mantemos LanguageID original
                languageService.Update(ID, name);

                Console.WriteLine("Idioma atualizado com sucesso!");
                LogService.Write("INFO", $"Idioma atualizado: {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar idioma.");
                LogService.Write("ERROR", $"Erro ao atualizar idioma: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
} //OBS: O usuário não deve ter acesso a edição do ID para não ter conflito de dados duplicados, por exemplo.

        static void DeleteLanguage(LanguageService languageService)
        {
            try
            {
                var id = PromptHelper.PromptInt("ID do idioma a deletar");
                languageService.Delete(id);

                Console.WriteLine("Idioma deletado com sucesso!");
                LogService.Write("INFO", $"Idioma deletado: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao deletar idioma.");
                LogService.Write("ERROR", $"Erro ao deletar idioma: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}

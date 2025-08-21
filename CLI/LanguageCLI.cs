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
            var languages = languageService.GetAll();
            Console.WriteLine("\n=== Lista de Idiomas ===");
            foreach (var language in languages)
            {
                Console.WriteLine($"{language.ID} - {language.Name} - {language.LanguageID}");
            }
        }

        static void CreateLanguage(LanguageService languageService)
        {
            var ID = PromptHelper.PromptInt("ID: ");
            var Name = PromptHelper.PromptRequired("Name: ");
            var LanguageID = PromptHelper.PromptRequired("LanguageID: ");

            var newlanguage = new Language
            {
                ID = ID,
                Name = Name,
                LanguageID = Guid.NewGuid(),
            };

            languageService.Create(ID, Name, LanguageID);
            Console.WriteLine("Gênero Criado com Sucesso!");
        }


        static void GetByIdLanguage(LanguageService languageService)
        {
            var idInput = PromptHelper.PromptRequired("ID do Gênero");

            if (!int.TryParse(idInput, out int ID))
            {
                Console.WriteLine("ID Inválido! Certifique-se de digitar um número inteiro.");
                return;
            }

            var language = languageService.GetById(ID);

            if (language == null)
            {
                Console.WriteLine("Gênero não encontrado.");
                return;
            }

            Console.WriteLine("\n=== Gênero Encontrado ===");
            Console.WriteLine($"ID: {language.ID}");
            Console.WriteLine($"Nome: {language.Name}");
        }



        static void UpdateLanguage(LanguageService languageService)
        {
            Console.Write("Digite o ID do Gênero a atualizar: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            var existinglanguage = languageService.GetById(ID);
            if (existinglanguage == null)
            {
                Console.WriteLine("Gênero não encontrado.");
                return;
            }

            Console.Write("Novo Name_language: ");
            var name_language = PromptHelper.PromptRequired("Name_language");

            languageService.Update(ID, name_language);
            Console.WriteLine("Gênero atualizado com sucesso!");
        } //OBS: O usuário não deve ter acesso a edição do ID para não ter conflito de dados duplicados, por exemplo.


        static void DeleteLanguage(LanguageService languageService)
        {
            Console.Write("Digite o ID do Gênero a Deletar: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            languageService.Delete(ID);
            Console.WriteLine("Gênero Deletado com Sucesso!");
        }
    }
}

using Service;
using Domain;
using CLI.Helpers;

namespace CLI
{
    public static class GenreCLI
    {
        public static void Menu()
        {
            var genreService = new GenreService();

            while (true)
            {
                Console.WriteLine("\n===== Menu Gênero =====");
                Console.WriteLine("1 - Listar Gênero");
                Console.WriteLine("2 - Adicionar Gênero");
                Console.WriteLine("3 - Procurar pelo ID do Gênero");
                Console.WriteLine("4 - Atualizar Gênero do Livro");
                Console.WriteLine("5 - Deletar Gênero");
                Console.WriteLine("0 - Voltar ao Menu Principal");

                Console.Write("Escolha uma opção: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListGenre(genreService);
                        break;
                    case "2":
                        CreateGenre(genreService);
                        break;
                    case "3":
                        GetByIdGenre(genreService);
                        break;
                    case "4":
                        UpdateGenre(genreService);
                        break;
                    case "5":
                        DeleteGenre(genreService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        static void ListGenre(GenreService genreService)
        {
            try
            {
                var genres = genreService.GetAll();
                Console.WriteLine("\n=== Lista de Gêneros ===");
                foreach (var genre in genres)
                {
                    Console.WriteLine($"{genre.ID} - {genre.Name_genre}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar gêneros.");
                LogService.Write("ERROR", $"Erro ao listar gêneros: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }


        static void CreateGenre(GenreService genreService)
        {
            try
            {
                var name_genre = PromptHelper.PromptRequired("Nome do Gênero: ");

                genreService.Create(name_genre);

                Console.WriteLine("Gênero criado com sucesso!");
                LogService.Write("INFO", $"Gênero criado: {name_genre}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar gênero. Verifique os dados e tente novamente.");
                LogService.Write("ERROR", $"Erro ao criar gênero: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        static void GetByIdGenre(GenreService genreService)
        {
            try
            {
                var idInput = PromptHelper.PromptInt("ID do Gênero");

                var genre = genreService.GetById(idInput);
                if (genre == null)
                {
                    Console.WriteLine("Gênero não encontrado.");
                    return;
                }

                Console.WriteLine("\n=== Gênero Encontrado ===");
                Console.WriteLine($"ID: {genre.ID}");
                Console.WriteLine($"Nome: {genre.Name_genre}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar gênero.");
                LogService.Write("ERROR", $"Erro ao buscar gênero: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }




        static void UpdateGenre(GenreService genreService)
        {
            try
            {
                var idInput = PromptHelper.PromptInt("ID do Gênero a atualizar");

                var genre = genreService.GetById(idInput);
                if (genre == null)
                {
                    Console.WriteLine("Gênero não encontrado.");
                    return;
                }

                var newName = PromptHelper.PromptRequired($"Novo nome ({genre.Name_genre}): ");

                genreService.Update(idInput, newName);

                Console.WriteLine("Gênero atualizado com sucesso!");
                LogService.Write("INFO", $"Gênero atualizado: {idInput} - {newName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar gênero.");
                LogService.Write("ERROR", $"Erro ao atualizar gênero: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        } //OBS: O usuário não deve ter acesso a edição do ID para não ter conflito de dados duplicados, por exemplo.


        static void DeleteGenre(GenreService genreService)
        {
            try
            {
                var idInput = PromptHelper.PromptInt("ID do Gênero a deletar");

                genreService.Delete(idInput);

                Console.WriteLine("Gênero deletado com sucesso!");
                LogService.Write("INFO", $"Gênero deletado: {idInput}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao deletar gênero.");
                LogService.Write("ERROR", $"Erro ao deletar gênero: {ex.Message}");
                LogHelper.Error($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}

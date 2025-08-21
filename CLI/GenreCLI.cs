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

        static void ListGenre(GenreService GenreService)
        {
            var genres = GenreService.GetAll();
            Console.WriteLine("\n=== Lista de Gêneros ===");
            foreach (var genre in genres)
            {
                Console.WriteLine($"{genre.ID} - {genre.Name_genre}");
            }
        }

        static void CreateGenre(GenreService GenreService)
        {
            var ID = PromptHelper.PromptInt("ID: ");
            var Name_genre = PromptHelper.PromptRequired("Name_genre: ");

            var newgenre = new Genre
            {
                ID = ID,
                Name_genre = Name_genre,

            };

            GenreService.Create(ID, Name_genre);
            Console.WriteLine("Gênero Criado com Sucesso!");
        }


        static void GetByIdGenre(GenreService genreService)
        {
            var idInput = PromptHelper.PromptRequired("ID do Gênero");

            if (!int.TryParse(idInput, out int ID))
            {
                Console.WriteLine("ID Inválido! Certifique-se de digitar um número inteiro.");
                return;
            }

            var genre = genreService.GetById(ID);

            if (genre == null)
            {
                Console.WriteLine("Gênero não encontrado.");
                return;
            }

            Console.WriteLine("\n=== Gênero Encontrado ===");
            Console.WriteLine($"ID: {genre.ID}");
            Console.WriteLine($"Nome: {genre.Name_genre}");
        }



        static void UpdateGenre(GenreService genreService)
        {
            Console.Write("Digite o ID do Gênero a atualizar: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            var existingGenre = genreService.GetById(ID);
            if (existingGenre == null)
            {
                Console.WriteLine("Gênero não encontrado.");
                return;
            }

            Console.Write("Novo Name_genre: ");
            var name_genre = PromptHelper.PromptRequired("Name_genre");

            genreService.Update(ID, name_genre);
            Console.WriteLine("Gênero atualizado com sucesso!");
        } //OBS: O usuário não deve ter acesso a edição do ID para não ter conflito de dados duplicados, por exemplo.


        static void DeleteGenre(GenreService GenreService)
        {
            Console.Write("Digite o ID do Gênero a Deletar: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var ID))
            {
                Console.WriteLine("ID Inválido. Operação Cancelada.");
                return;
            }

            GenreService.Delete(ID);
            Console.WriteLine("Gênero Deletado com Sucesso!");
        }
    }
}

using Domain;

namespace CLIMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("------------Menu Biblioteca------------");
                Console.WriteLine("1 - Menu Cliente");
                Console.WriteLine("2 - Menu Livros");
                Console.WriteLine("3 - Menu Empréstimo");
                Console.WriteLine("4 - Menu Devoluções/Relatórios");
                Console.WriteLine("0 - Sair");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Client
                }
            }
        }
    }
}
using CLI;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n===== BIBLIOTECA =====");
            Console.WriteLine("1 - Clientes");
            Console.WriteLine("2 - Catálogo de Livros");
            Console.WriteLine("3 - Inventário");
            Console.WriteLine("4 - Empréstimos");
            Console.WriteLine("0 - Sair");

            Console.Write("Escolha uma opção: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ClientCLI.Menu();
                    break;
                case "2":
                    CatalogCLI.Menu();
                    break;
                case "3":
                    InventoryCLI.Menu();
                    break;
                case "4":
                    LoanCLI.Menu();
                    break;
                case "0":
                    Console.WriteLine("Saindo...");
                    return;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }
}

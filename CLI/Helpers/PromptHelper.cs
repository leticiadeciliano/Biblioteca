namespace CLI.Helpers
{
    public static class PromptHelper
    {
        public static string PromptRequired(string fieldName)
        {
            string? input;
            do
            {
                Console.Write($"{fieldName}: ");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine($"{fieldName} é obrigatório. Tente novamente.");

            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        public static int PromptInt(string fieldName)
        {
            int value;
            string? input;
            do
            {
                Console.Write($"{fieldName}: ");
                input = Console.ReadLine();
                if (!int.TryParse(input, out value) || value <= 0)
                    Console.WriteLine($"{fieldName} deve ser um número inteiro positivo. Tente novamente.");
            } while (!int.TryParse(input, out value) || value <= 0);
            return value;
        }

        public static Guid PromptGuid(string message)
        {
            Guid value;
            while (true)
            {
                Console.Write($"{message}: ");
                var input = Console.ReadLine();
                if (Guid.TryParse(input, out value))
                    return value;
                Console.WriteLine("Por favor, insira um GUID válido.");
            }
        }
    }
}

//classe criada unica e exclusivamente para garantir que nenhuma parte do CRUD no CLI irá permitir ser criada
//com espaço em branco ou sem a informação necessária.
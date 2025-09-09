using Service;

namespace CLI.Helpers
{
    public static class LogHelper
    {
        // Info → para mensagens normais
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[INFO] {message}");
            Console.ResetColor();

            LogService.Write(message, "INFO");
        }

        // Warn → para avisos
        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARN] {message}");
            Console.ResetColor();

            LogService.Write(message, "WARN");
        }

        // Error → para erros
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {message}");
            Console.ResetColor();

            LogService.Write(message, "ERROR");
        }
    }
}

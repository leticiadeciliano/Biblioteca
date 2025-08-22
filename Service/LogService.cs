using System;
using System.IO;

namespace Service
{
    public static class LogService
    {
        private static readonly string logFilePath = "logs.txt";

        public static void Write(string message, string v)
        {
            try
            {
                // Formata a mensagem com data e hora
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                
                // Adiciona a mensagem no arquivo de logs
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            catch
            {
                // Se der erro ao escrever o log, não fazemos nada para não travar o programa
            }
        }
    }
}

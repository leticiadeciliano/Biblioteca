using System.Text;

namespace CLI.Helpers
{
    public static class GuidHelper
    {
        // Lê qualquer coisa que o reader retornar (byte[] BLOB, string, int, etc.)
        public static Guid FromDb(object raw)
        {
            if (raw == null || raw is DBNull) return Guid.Empty;

            if (raw is byte[] bytes)
            {
                if (bytes.Length == 16) return new Guid(bytes);
                try
                {
                    var s = Encoding.UTF8.GetString(bytes);
                    if (Guid.TryParse(s, out var g)) return g;
                }
                catch { }
            }

            var str = raw.ToString();
            if (Guid.TryParse(str, out var parsed)) return parsed;
            return Guid.Empty;
        }

        // Retorna o objeto a ser usado como parâmetro no DB: byte[] (BLOB) ou string (TEXT)
        public static object ToDb(Guid guid, bool asBlob = false)
        {
            return asBlob ? guid.ToByteArray() : guid.ToString();
        }
    }
}

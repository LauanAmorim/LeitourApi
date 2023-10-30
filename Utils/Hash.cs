using System.Security.Cryptography;
using System.Text;



namespace LeitourApi.Utils
{
    public class Hash
    {

        public static string GerarHash(string texto)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(texto);
            byte[] hash = SHA256.HashData(bytes);
            StringBuilder result = new();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("x"));
            }
            return result.ToString();
        }
    }
}
using System.Security.Cryptography;
using System.Text;

namespace Kudobox.Helpers.Extensions
{
    public static class EncryptString
    {
        public static string Encrypt(this string data)
        {
            var algorithm = SHA512.Create();
            var encodedValue = Encoding.UTF8.GetBytes(data);
            var encryptedPassword = algorithm.ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var character in encryptedPassword)
                sb.Append(character.ToString("X2"));
            
            return sb.ToString();
        }
    }
}
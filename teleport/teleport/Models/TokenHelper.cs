using System.Security.Cryptography;

namespace teleport.Models
{
    public class TokenHelper
    {
        public static string Generate(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);

            var result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = chars[bytes[i] % chars.Length];

            return new string(result);
        }
    }
}

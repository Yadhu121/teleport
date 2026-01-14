using System.Security.Cryptography;

public static class TokenHelper
{
    public static string Generate(int length)
    {
        const String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);

        var result = new char[length];
        for (int i = 0; i < length; i++)
            result[i] = chars[bytes[i] % chars.Length];

        return new string(result);
    }
}
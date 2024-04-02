using CommunityToolkit.HighPerformance;

using System.Security.Cryptography;
using System.Text;

namespace Edelstein.Assets.Management;

public static class ManifestCryptor
{
    private const string AesKey = "akmzncej3dfheuds654sg9ad1f3fnfoi";
    private static readonly byte[] AesKeyBytes = Encoding.UTF8.GetBytes(AesKey);

    private const string AesIv = "lmxcye89bsdfb0a1";
    private static readonly byte[] AesIvBytes = Encoding.UTF8.GetBytes(AesIv);

    public static byte[] Decrypt(byte[] encryptedData)
    {
        using MemoryStream decryptedData = Decrypt(encryptedData.AsMemory().AsStream());
        return decryptedData.ToArray();
    }

    public static MemoryStream Decrypt(Stream encryptedData)
    {
        using Aes aes = Aes.Create();

        using ICryptoTransform decryptor = aes.CreateDecryptor(AesKeyBytes, AesIvBytes);

        using CryptoStream cryptoStream = new(encryptedData, decryptor, CryptoStreamMode.Read);
        MemoryStream decryptedDataStream = new();

        cryptoStream.CopyTo(decryptedDataStream);

        decryptedDataStream.Seek(0, SeekOrigin.Begin);

        return decryptedDataStream;
    }
}

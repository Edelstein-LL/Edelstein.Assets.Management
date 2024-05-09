using System.Security.Cryptography;
using System.Text;

namespace Edelstein.Assets.Management.Manifest;

public static class ManifestCryptor
{
    private const string AesKey = "akmzncej3dfheuds654sg9ad1f3fnfoi";
    private static readonly byte[] AesKeyBytes = Encoding.UTF8.GetBytes(AesKey);

    private const string AesIv = "lmxcye89bsdfb0a1";
    private static readonly byte[] AesIvBytes = Encoding.UTF8.GetBytes(AesIv);

    public static byte[] Decrypt(byte[] encryptedData)
    {
        using MemoryStream encryptedStream = new(encryptedData);
        using MemoryStream decryptedData = Decrypt(encryptedStream);
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

    public static byte[] Encrypt(byte[] data)
    {
        using MemoryStream dataStream = new(data);
        using MemoryStream encryptedData = Decrypt(dataStream);
        return encryptedData.ToArray();
    }

    public static MemoryStream Encrypt(Stream data)
    {
        using Aes aes = Aes.Create();

        using ICryptoTransform encryptor = aes.CreateEncryptor(AesKeyBytes, AesIvBytes);

        MemoryStream encryptedDataStream = new();
        using CryptoStream cryptoStream = new(encryptedDataStream, encryptor, CryptoStreamMode.Write, true);
        data.CopyTo(cryptoStream);
        cryptoStream.FlushFinalBlock();

        encryptedDataStream.Seek(0, SeekOrigin.Begin);

        return encryptedDataStream;
    }
}

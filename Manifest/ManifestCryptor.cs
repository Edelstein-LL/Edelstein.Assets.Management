using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Edelstein.Assets.Management.Manifest;

public static class ManifestCryptor
{
    private const string AesKey = "akmzncej3dfheuds654sg9ad1f3fnfoi";
    private static readonly byte[] AesKeyBytes = Encoding.UTF8.GetBytes(AesKey);

    private const string AesIv = "lmxcye89bsdfb0a1";
    private static readonly byte[] AesIvBytes = Encoding.UTF8.GetBytes(AesIv);

    public static async Task DecryptAsync(Stream encryptedStream, Stream outputStream)
    {
        using Aes aes = Aes.Create();

        using ICryptoTransform decryptor = aes.CreateDecryptor(AesKeyBytes, AesIvBytes);

        await using CryptoStream cryptoStream = new(encryptedStream, decryptor, CryptoStreamMode.Read, true);
        await using GZipStream gZipStream = new(cryptoStream, CompressionMode.Decompress, true);
        await gZipStream.CopyToAsync(outputStream);
    }

    public static async Task DecryptUncompressedAsync(Stream encryptedStream, Stream outputStream)
    {
        using Aes aes = Aes.Create();

        using ICryptoTransform decryptor = aes.CreateDecryptor(AesKeyBytes, AesIvBytes);

        await using CryptoStream cryptoStream = new(encryptedStream, decryptor, CryptoStreamMode.Read, true);
        await cryptoStream.CopyToAsync(outputStream);
    }

    public static async Task EncryptAsync(Stream dataStream, Stream outputStream)
    {
        using Aes aes = Aes.Create();

        using ICryptoTransform encryptor = aes.CreateEncryptor(AesKeyBytes, AesIvBytes);

        await using CryptoStream cryptoStream = new(outputStream, encryptor, CryptoStreamMode.Write, true);
        await using (GZipStream gZipStream = new(cryptoStream, CompressionMode.Compress, true))
        {
            await dataStream.CopyToAsync(gZipStream);
        }

        #if NET8_0_OR_GREATER
        await cryptoStream.FlushFinalBlockAsync();
        #else
        cryptoStream.FlushFinalBlock();
        #endif
    }
}

using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Edelstein.Assets.Management.Msts;

public class MstBundleCryptor
{
    public static void DecryptMstBundle(Stream dataStream, Stream outputStream)
    {
        using MemoryStream ms = new();
        dataStream.CopyTo(ms);

        byte[] buffer = ms.GetBuffer();
        ReadOnlySpan<byte> dataSpan = buffer.AsSpan(0, (int)ms.Length);

        ReadOnlySpan<byte> salt = dataSpan.Slice(16, 16);

        using Rfc2898DeriveBytes rfc2898DeriveBytes = new("3559b435f24b297a79c68b9709ef2125", salt.ToArray(), 1000, HashAlgorithmName.SHA1);
        byte[] key = rfc2898DeriveBytes.GetBytes(16);

        int ivLength = BinaryPrimitives.ReadInt32LittleEndian(dataSpan[32..36]);
        ReadOnlySpan<byte> iv = dataSpan.Slice(36, ivLength);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv.ToArray();

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream encryptedStream = new(buffer, 36 + ivLength, dataSpan.Length - 36 - ivLength);
        using CryptoStream cryptoStream = new(encryptedStream, decryptor, CryptoStreamMode.Read, true);
        using GZipStream gZipStream = new(cryptoStream, CompressionMode.Decompress, true);

        gZipStream.CopyTo(outputStream);
    }

    public static void EncryptMstBundle(Stream dataStream, Stream outputStream)
    {
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);

        using Rfc2898DeriveBytes rfc2898DeriveBytes = new("3559b435f24b297a79c68b9709ef2125", salt, 1000, HashAlgorithmName.SHA1);
        byte[] key = rfc2898DeriveBytes.GetBytes(16);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
        byte[] iv = aes.IV;

        byte[] ivLengthBinary = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(ivLengthBinary, iv.Length);

        Span<byte> beginningFiller = stackalloc byte[16];
        beginningFiller.Clear();

        outputStream.Write(beginningFiller);
        outputStream.Write(salt, 0, salt.Length);
        outputStream.Write(ivLengthBinary, 0, ivLengthBinary.Length);
        outputStream.Write(iv, 0, iv.Length);

        using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using CryptoStream cryptoStream = new(outputStream, encryptor, CryptoStreamMode.Write, true);

        using (GZipStream gZipStream = new(cryptoStream, CompressionMode.Compress, true))
        {
            dataStream.CopyTo(gZipStream);
        }

        cryptoStream.FlushFinalBlock();
    }
}

using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Edelstein.Assets.Management.Msts;

public class MstBundleCryptor
{
    public static MemoryStream DecryptMstBundle(Stream data)
    {
        using MemoryStream ms = new();
        data.CopyTo(ms);

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

        using MemoryStream encryptedDataStream = new(buffer, 36 + ivLength, dataSpan.Length - 36 - ivLength);
        using CryptoStream cryptoStream = new(encryptedDataStream, decryptor, CryptoStreamMode.Read);
        using GZipStream gZipStream = new(cryptoStream, CompressionMode.Decompress);
        MemoryStream resultMemoryStream = new();

        gZipStream.CopyTo(resultMemoryStream);

        resultMemoryStream.Position = 0;

        return resultMemoryStream;
    }

    public static MemoryStream EncryptMstBundle(Stream data)
    {
        using MemoryStream compressedStream = new();
        using (GZipStream gZipStream = new(compressedStream, CompressionMode.Compress, true))
        {
            data.CopyTo(gZipStream);
        }

        compressedStream.Position = 0;

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

        MemoryStream encryptedStream = new();
        encryptedStream.Write(beginningFiller);
        encryptedStream.Write(salt, 0, salt.Length);
        encryptedStream.Write(ivLengthBinary, 0, ivLengthBinary.Length);
        encryptedStream.Write(iv, 0, iv.Length);

        using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using CryptoStream cryptoStream = new(encryptedStream, encryptor, CryptoStreamMode.Write, true);

        compressedStream.CopyTo(cryptoStream);
        cryptoStream.FlushFinalBlock();

        encryptedStream.Position = 0;

        return encryptedStream;
    }
}

using CommunityToolkit.HighPerformance;

using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Edelstein.Assets.Management.Manifest;

#pragma warning disable SYSLIB0011
public static class ManifestSerializer
{
    public static T DeserializeCompressedBinary<T, TBinder>(byte[] serializedData, TBinder? serializationBinder = null)
        where TBinder : SerializationBinder, new() =>
        DeserializeCompressedBinary<T, TBinder>(serializedData.AsMemory().AsStream(), serializationBinder);

    public static T DeserializeCompressedBinary<T, TBinder>(Stream serializedData, TBinder? serializationBinder = null)
        where TBinder : SerializationBinder, new()
    {
        serializationBinder ??= new TBinder();

        using GZipStream gZipStream = new(serializedData, CompressionMode.Decompress);

        BinaryFormatter binaryFormatter = new() { Binder = serializationBinder };

        return (T)binaryFormatter.Deserialize(gZipStream);
    }

    public static MemoryStream SerializeAndEncrypt<T, TBinder>(T data, TBinder? serializationBinder = null)
        where TBinder : SerializationBinder, new()
    {
        serializationBinder ??= new TBinder();

        BinaryFormatter binaryFormatter = new() { Binder = serializationBinder };

        using MemoryStream compressedStream = new();
        using GZipStream gZipStream = new(compressedStream, CompressionMode.Compress, true);

        binaryFormatter.Serialize(gZipStream, data!);

        compressedStream.Position = 0;

        MemoryStream encryptedStream = ManifestCryptor.Encrypt(compressedStream);

        return encryptedStream;
    }
}

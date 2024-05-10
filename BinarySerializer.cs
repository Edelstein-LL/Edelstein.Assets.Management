using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Edelstein.Assets.Management;

public static class BinarySerializer
{
    public static T Deserialize<T>(Stream dataStream, BinaryFormatter binaryFormatter) =>
        (T)binaryFormatter.Deserialize(dataStream);

    public static T Deserialize<T, TBinder>(Stream dataStream)
        where TBinder : SerializationBinder, new()
    {
        BinaryFormatter binaryFormatter = new() { Binder = new TBinder() };

        return (T)binaryFormatter.Deserialize(dataStream);
    }

    public static void Serialize<T>(T data, Stream outputStream, BinaryFormatter binaryFormatter) =>
        binaryFormatter.Serialize(outputStream, data!);

    public static void Serialize<T, TBinder>(T data, Stream outputStream)
        where TBinder : SerializationBinder, new()
    {
        BinaryFormatter binaryFormatter = new() { Binder = new TBinder() };

        binaryFormatter.Serialize(outputStream, data!);
    }
}

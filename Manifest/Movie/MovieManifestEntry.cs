using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Movie;

[Serializable]
public class MovieManifestEntry : IManifestEntry, ISerializable
{
    public required string Identifier { get; set; }

    public required string Name { get; set; }

    public required string Hash { get; set; }

    public required string UsmHash { get; set; }

    public long UsmSize { get; set; }

    public required string UsmPrimaryPartHash { get; set; }

    public long UsmPrimaryPartSize { get; set; }

    public required string UsmSecondaryPartHash { get; set; }

    public long UsmSecondaryPartSize { get; set; }

    public bool EnableSplit { get; set; }

    public required string[] Labels { get; set; }

    public MovieManifestEntry() { }

    public MovieManifestEntry(SerializationInfo info, StreamingContext context)
    {
        Identifier = info.GetString("m_identifier")!;
        Name = info.GetString("m_name")!;
        Hash = info.GetString("m_hash")!;
        UsmHash = info.GetString("m_usmHash")!;
        UsmSize = info.GetInt64("m_usmSize");
        UsmPrimaryPartHash = info.GetString("m_usmPrimaryPartHash")!;
        UsmPrimaryPartSize = info.GetInt64("m_usmPrimaryPartSize");
        UsmSecondaryPartHash = info.GetString("m_usmSecondaryPartHash")!;
        UsmSecondaryPartSize = info.GetInt64("m_usmSecondaryPartSize");
        EnableSplit = info.GetBoolean("m_enableSplit");
        Labels = (string[])info.GetValue("m_labels", typeof(string[]))!;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("m_identifier", Identifier);
        info.AddValue("m_name", Name);
        info.AddValue("m_hash", Hash);
        info.AddValue("m_usmHash", UsmHash);
        info.AddValue("m_usmSize", UsmSize);
        info.AddValue("m_usmPrimaryPartHash", UsmPrimaryPartHash);
        info.AddValue("m_usmPrimaryPartSize", UsmPrimaryPartSize);
        info.AddValue("m_usmSecondaryPartHash", UsmSecondaryPartHash);
        info.AddValue("m_usmSecondaryPartSize", UsmSecondaryPartSize);
        info.AddValue("m_enableSplit", EnableSplit);
        info.AddValue("m_labels", Labels);
    }
}

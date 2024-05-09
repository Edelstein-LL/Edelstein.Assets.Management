using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Bundle;

[Serializable]
public class BundleManifestEntry : IManifestEntry, ISerializable
{
    public required string Identifier { get; set; }

    public required string Name { get; set; }

    public required string Hash { get; set; }

    public uint Crc { get; set; }

    public long Length { get; set; }

    public string[] Dependencies { get; set; } = [];

    public string[] Labels { get; set; } = [];

    public string[] Assets { get; set; } = [];

    public BundleManifestEntry() { }

    public BundleManifestEntry(SerializationInfo info, StreamingContext context)
    {
        Identifier = info.GetString("m_identifier")!;
        Name = info.GetString("m_name")!;
        Hash = info.GetString("m_hash")!;
        Crc = info.GetUInt32("m_crc");
        Length = info.GetInt64("m_length");
        Dependencies = (string[])info.GetValue("m_dependencies", typeof(string[]))!;
        Labels = (string[])info.GetValue("m_labels", typeof(string[]))!;
        Assets = (string[])info.GetValue("m_assets", typeof(string[]))!;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("m_identifier", Identifier);
        info.AddValue("m_name", Name);
        info.AddValue("m_hash", Hash);
        info.AddValue("m_crc", Crc);
        info.AddValue("m_length", Length);
        info.AddValue("m_dependencies", Dependencies);
        info.AddValue("m_labels", Labels);
        info.AddValue("m_assets", Assets);
    }
}

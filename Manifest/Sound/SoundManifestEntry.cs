using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Sound;

[Serializable]
public class SoundManifestEntry : IManifestEntry, ISerializable
{
    public required string Identifier { get; set; }

    public required string Name { get; set; }

    public required string Hash { get; set; }

    public required string AcbHash { get; set; }

    public long AcbSize { get; set; }

    public required string AcbPrimaryPartHash { get; set; }

    public long AcbPrimaryPartSize { get; set; }

    public required string AcbSecondaryPartHash { get; set; }

    public long AcbSecondaryPartSize { get; set; }

    public required string AwbHash { get; set; }

    public long AwbSize { get; set; }

    public required string AwbPrimaryPartHash { get; set; }

    public long AwbPrimaryPartSize { get; set; }

    public required string AwbSecondaryPartHash { get; set; }

    public long AwbSecondaryPartSize { get; set; }

    public required string[] Labels { get; set; }

    public bool EnableSplit { get; set; }

    public SoundManifestEntry() { }

    public SoundManifestEntry(SerializationInfo info, StreamingContext context)
    {
        Identifier = info.GetString("m_identifier")!;
        Name = info.GetString("m_name")!;
        Hash = info.GetString("m_hash")!;
        AcbHash = info.GetString("m_acbHash")!;
        AcbSize = info.GetInt64("m_acbSize");
        AcbPrimaryPartHash = info.GetString("m_acbPrimaryPartHash")!;
        AcbPrimaryPartSize = info.GetInt64("m_acbPrimaryPartSize");
        AcbSecondaryPartHash = info.GetString("m_acbSecondaryPartHash")!;
        AcbSecondaryPartSize = info.GetInt64("m_acbSecondaryPartSize");
        AwbHash = info.GetString("m_awbHash")!;
        AwbSize = info.GetInt64("m_awbSize");
        AwbPrimaryPartHash = info.GetString("m_awbPrimaryPartHash")!;
        AwbPrimaryPartSize = info.GetInt64("m_awbPrimaryPartSize");
        AwbSecondaryPartHash = info.GetString("m_awbSecondaryPartHash")!;
        AwbSecondaryPartSize = info.GetInt64("m_awbSecondaryPartSize");
        Labels = (string[])info.GetValue("m_labels", typeof(string[]))!;
        EnableSplit = info.GetBoolean("m_enableSplit");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("m_identifier", Identifier);
        info.AddValue("m_name", Name);
        info.AddValue("m_hash", Hash);
        info.AddValue("m_acbHash", AcbHash);
        info.AddValue("m_acbSize", AcbSize);
        info.AddValue("m_acbPrimaryPartHash", AcbPrimaryPartHash);
        info.AddValue("m_acbPrimaryPartSize", AcbPrimaryPartSize);
        info.AddValue("m_acbSecondaryPartHash", AcbSecondaryPartHash);
        info.AddValue("m_acbSecondaryPartSize", AcbSecondaryPartSize);
        info.AddValue("m_awbHash", AwbHash);
        info.AddValue("m_awbSize", AwbSize);
        info.AddValue("m_awbPrimaryPartHash", AwbPrimaryPartHash);
        info.AddValue("m_awbPrimaryPartSize", AwbPrimaryPartSize);
        info.AddValue("m_awbSecondaryPartHash", AwbSecondaryPartHash);
        info.AddValue("m_awbSecondaryPartSize", AwbSecondaryPartSize);
        info.AddValue("m_labels", Labels);
        info.AddValue("m_enableSplit", EnableSplit);
    }
}

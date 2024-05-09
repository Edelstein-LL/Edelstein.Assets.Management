using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Bundle;

[Serializable]
public class BundleManifest : ISerializable
{
    public required BundleManifestEntry[] Entries { get; set; }

    public BundleManifest() { }

    public BundleManifest(SerializationInfo info, StreamingContext context) =>
        Entries = (BundleManifestEntry[])info.GetValue("m_manifestCollection", typeof(BundleManifestEntry[]))!;

    public void GetObjectData(SerializationInfo info, StreamingContext context) =>
        info.AddValue("m_manifestCollection", Entries);
}

using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Sound;

[Serializable]
public class SoundManifest : ISerializable
{
    public required SoundManifestEntry[] Entries { get; set; }

    public SoundManifest() { }

    protected SoundManifest(SerializationInfo info, StreamingContext context) =>
        Entries = (SoundManifestEntry[])info.GetValue("m_manifestCollection", typeof(SoundManifestEntry[]))!;

    public void GetObjectData(SerializationInfo info, StreamingContext context) =>
        info.AddValue("m_manifestCollection", Entries);
}

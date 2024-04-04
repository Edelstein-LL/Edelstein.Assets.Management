using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Movie;

[Serializable]
public class MovieManifest : ISerializable
{
    public required MovieManifestEntry[] Entries { get; init; }

    public MovieManifest() { }

    public MovieManifest(SerializationInfo info, StreamingContext context) =>
        Entries = (MovieManifestEntry[])info.GetValue("m_manifestCollection", typeof(MovieManifestEntry[]))!;

    public void GetObjectData(SerializationInfo info, StreamingContext context) { }
}

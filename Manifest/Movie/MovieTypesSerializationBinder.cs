using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Movie;

public class MovieTypesSerializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName) =>
        typeName switch
        {
            "ShockBinaryMovieSingleManifest" => typeof(MovieManifest),
            "ShockBinaryMovieManifest" => typeof(MovieManifestEntry),
            _ => throw new NotImplementedException()
        };

    public override void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        assemblyName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        typeName = serializedType.Name switch
        {
            nameof(MovieManifest) => "ShockBinaryMovieSingleManifest",
            nameof(MovieManifestEntry) => "ShockBinaryMovieManifest",
            _ => throw new NotImplementedException()
        };
    }
}

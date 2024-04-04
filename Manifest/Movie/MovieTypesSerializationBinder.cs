using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Movie;

public class MovieTypesSerializationBinder : SerializationBinder
{
    public override Type? BindToType(string assemblyName, string typeName) =>
        typeName switch
        {
            "ShockBinaryMovieSingleManifest" => typeof(MovieManifest),
            "ShockBinaryMovieManifest" => typeof(MovieManifestEntry),
            _ => null
        };
}
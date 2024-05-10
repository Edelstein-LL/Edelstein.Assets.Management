using Edelstein.Assets.Management.Manifest.Bundle;
using Edelstein.Assets.Management.Manifest.Movie;
using Edelstein.Assets.Management.Manifest.Sound;

using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest;

public class ManifestSerializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName) =>
        typeName switch
        {
            "ShockBinaryBundleSingleManifest" => typeof(BundleManifest),
            "ShockBinaryBundleManifest" => typeof(BundleManifestEntry),
            "ShockBinaryMovieSingleManifest" => typeof(MovieManifest),
            "ShockBinaryMovieManifest" => typeof(MovieManifestEntry),
            "ShockBinarySoundSingleManifest" => typeof(SoundManifest),
            "ShockBinarySoundManifest" => typeof(SoundManifestEntry),
            _ => throw new NotImplementedException()
        };

    public override void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        if (serializedType.FullName!.StartsWith("System."))
        {
            assemblyName = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            typeName = serializedType.FullName;
            return;
        }

        assemblyName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        typeName = serializedType.Name switch
        {
            nameof(BundleManifest) => "ShockBinaryBundleSingleManifest",
            nameof(BundleManifestEntry) => "ShockBinaryBundleManifest",
            nameof(MovieManifest) => "ShockBinaryMovieSingleManifest",
            nameof(MovieManifestEntry) => "ShockBinaryMovieManifest",
            nameof(SoundManifest) => "ShockBinarySoundSingleManifest",
            nameof(SoundManifestEntry) => "ShockBinarySoundManifest",
            _ => throw new NotImplementedException()
        };
    }
}

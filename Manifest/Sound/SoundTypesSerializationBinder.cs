using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Sound;

public class SoundTypesSerializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName) =>
        typeName switch
        {
            "ShockBinarySoundSingleManifest" => typeof(SoundManifest),
            "ShockBinarySoundManifest" => typeof(SoundManifestEntry),
            _ => throw new NotImplementedException()
        };

    public override void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        assemblyName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        typeName = serializedType.Name switch
        {
            nameof(SoundManifest) => "ShockBinarySoundSingleManifest",
            nameof(SoundManifestEntry) => "ShockBinarySoundManifest",
            _ => throw new NotImplementedException()
        };
    }
}

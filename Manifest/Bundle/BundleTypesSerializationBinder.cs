using System.Runtime.Serialization;

namespace Edelstein.Assets.Management.Manifest.Bundle;

public class BundleTypesSerializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName) =>
        typeName switch
        {
            "ShockBinaryBundleSingleManifest" => typeof(BundleManifest),
            "ShockBinaryBundleManifest" => typeof(BundleManifestEntry),
            _ => throw new NotImplementedException()
        };

    public override void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        if (serializedType.FullName is "System.String")
        {
            assemblyName = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            typeName = "System.String";
            return;
        }

        assemblyName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        typeName = serializedType.Name switch
        {
            nameof(BundleManifest) => "ShockBinaryBundleSingleManifest",
            nameof(BundleManifestEntry) => "ShockBinaryBundleManifest",
            _ => throw new NotImplementedException()
        };
    }
}

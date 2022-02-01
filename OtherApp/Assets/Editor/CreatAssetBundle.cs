using System.IO;
using UnityEditor;
using UnityEngine;

public class CreatAssetBundle : MonoBehaviour
{
    private const string ASSET_BUNDLE_DIRECTORY = "Assets/AssetBundle";

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAssetBundle()
    {
        if (!Directory.Exists(ASSET_BUNDLE_DIRECTORY))
        {
            Directory.CreateDirectory(ASSET_BUNDLE_DIRECTORY);
        }

        BuildPipeline.BuildAssetBundles(ASSET_BUNDLE_DIRECTORY, BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}

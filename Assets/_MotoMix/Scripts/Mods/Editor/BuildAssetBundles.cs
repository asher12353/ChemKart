using UnityEditor;
using System.IO;

namespace ChemKart
{
    public class BuildAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            string bundleDirectory = "Assets/AssetBundles";
            if (!Directory.Exists(bundleDirectory))
            {
                Directory.CreateDirectory(bundleDirectory);
            }

            BuildPipeline.BuildAssetBundles(bundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}

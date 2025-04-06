using UnityEngine;
using UnityEditor;

public class EnableReadWriteForAllMeshes : EditorWindow
{
    [MenuItem("Tools/Enable Read/Write for All Meshes")]
    public static void EnableReadWrite()
    {
        string[] meshGUIDs = AssetDatabase.FindAssets("t:Model");
        foreach (string guid in meshGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
                Debug.Log($"Enabled Read/Write for: {path}");
            }
        }
        Debug.Log("Finished enabling Read/Write on all meshes.");
    }
}

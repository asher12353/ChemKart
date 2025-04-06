using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace ChemKart
{
    public class AssetBundleMenu : MonoBehaviour
    {
        public Transform buttonParent; // Assign a UI Panel to hold the buttons
        public Button buttonPrefab; // Assign a Button prefab in Unity Inspector

        private string bundleDirectory;

        // Variables to update
        public static string bundleName = "customprefab";
        public static string prefabName = "MyCustomPrefab";

        void Start()
        {
            bundleDirectory = Path.Combine(Application.streamingAssetsPath, "Tracks");
            LoadAssetBundles();
        }

        void LoadAssetBundles()
        {
            if (!Directory.Exists(bundleDirectory))
            {
                Debug.LogWarning("Tracks folder not found: " + bundleDirectory);
                return;
            }

            string[] files = Directory.GetFiles(bundleDirectory, "*", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                if (file.EndsWith(".meta") || file.EndsWith(".manifest")) continue; // Skip Unity meta files

                string assetBundleName = Path.GetFileNameWithoutExtension(file);

                // Create a button for each asset bundle
                Button newButton = Instantiate(buttonPrefab, buttonParent);
                newButton.GetComponentInChildren<TMP_Text>().text = assetBundleName;

                // Assign the click event to set bundleName and prefabName
                newButton.onClick.AddListener(() => SetAssetBundle(assetBundleName));
                newButton.onClick.AddListener(() => TrackLoader.LoadTrack());
            }
        }

        void SetAssetBundle(string assetBundleName)
        {
            bundleName = assetBundleName;
            prefabName = assetBundleName; // Assuming prefab name is the same
            Debug.Log($"Selected Asset Bundle: {bundleName}, Prefab: {prefabName}");
        }
    }

}

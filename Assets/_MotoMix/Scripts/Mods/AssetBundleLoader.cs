using UnityEngine;
using System.Collections;
using System.IO;

namespace ChemKart
{
    public class PrefabLoader : MonoBehaviour
    {
        [SerializeField]
        private WaypointGenerator m_WaypointGenerator;
        [SerializeField]
        private PickupGenerator m_PickupGenerator;
        [SerializeField]
        private PortalGenerator m_PortalGenerator;
        [SerializeField]
        private PlayerSpawner m_PlayerSpawner;
        [SerializeField]
        private GameObject m_SpawnPointPrefab;

        IEnumerator Start()
        {
            string bundlePath = Path.Combine(Application.streamingAssetsPath + "/Tracks", AssetBundleMenu.bundleName);
            
            // Load AssetBundle
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                yield break;
            }

            // Load the prefab
            GameObject prefab = bundle.LoadAsset<GameObject>(AssetBundleMenu.prefabName);
            if (prefab == null)
            {
                Debug.LogError($"Failed to load prefab '{AssetBundleMenu.prefabName}' from bundle!");
                bundle.Unload(false);
                yield break;
            }

            // Instantiate the prefab in the scene
            GameObject track = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            // find the transforms of the different components
            m_WaypointGenerator.tracks = track.transform.Find("Tracks").gameObject;
            m_PickupGenerator.pickupTransforms = track.transform.Find("Pickups").gameObject;
            m_PortalGenerator.portalTransforms = track.transform.Find("Portals").gameObject;
            track.transform.Find("RespawnPlane").gameObject.AddComponent<RespawnPlane>();

            SetLayerRecursively(track.transform.Find("RespawnPlane").gameObject, LayerMask.NameToLayer("Ignore Raycast"));
            
            // generate respective components
            m_WaypointGenerator.GenerateWaypoints();
            m_PickupGenerator.GeneratePickups();
            m_PortalGenerator.GeneratePortals();
            
            // deal with the spawn points, then spawn the players in
            GameObject spawnPoints = Instantiate(m_SpawnPointPrefab, Vector3.zero, Quaternion.identity);
            m_PlayerSpawner.spawnPoints = spawnPoints.transform;
            m_PlayerSpawner.spawnPoints.SetParent(track.transform);
            m_PlayerSpawner.SpawnPlayers();
            
            // Optionally, unload the bundle but keep the assets
            bundle.Unload(false);
        }

        void SetLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}

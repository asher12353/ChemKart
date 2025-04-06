using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChemKart
{
    public class TrackLoader : MonoBehaviour
    {
        public static void LoadTrack()
        {
            SceneManager.LoadScene("Main Track (modded)"); // this'll be dynamic later
        }
    }
}

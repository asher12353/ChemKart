using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChemKart
{
    public class TrackLoader : MonoBehaviour
    {
        public void LoadTrack()
        {
            SceneManager.LoadScene("Test Track Asher"); // this'll be dynamic later
        }
    }
}

using UnityEngine;

namespace ChemKart
{
    public class PlayerSelectStart : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerSelectMenu;
        [SerializeField]
        private GameObject trackSelectMenu;
        
        public void OnClick()
        {
            if(PlayerJoining.playersData.Count > 0)
            {
                playerSelectMenu.SetActive(false);
                trackSelectMenu.SetActive(true);
            }
        }
    }
}
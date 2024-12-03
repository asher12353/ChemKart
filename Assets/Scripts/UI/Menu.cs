using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace ChemKart
{
    public abstract class Menu : MonoBehaviour
    {
        [Header("Menu")]
        
        [SerializeField, Tooltip("Optional menu that appeared before this menu, will be re-opened when the back button is clicked.")]
        private Menu m_PreviousMenu;
        
        [SerializeField] private Button m_BackButton;
        
        [SerializeField,Tooltip("The button that will be selected first when opening the menu.")]
        private Button m_FirstSelectedButton;

        private void Awake()
        {
            if (!m_FirstSelectedButton)
            {
                Debug.LogWarning($"{name}: First selected button is not assigned.");
            }
        }
        
        private void OnEnable()
        {
            m_BackButton.OrNull()?.onClick.AddListener(OnBackButtonClicked);
        }
        
        private void Start()
        {
            SelectFirstButton();
        }
        
        private void OnDisable()
        {
            m_BackButton.OrNull()?.onClick.RemoveListener(OnBackButtonClicked);
        }
        
        public void ActivateMenu(Menu newMenu, bool deactivateCurrentMenu = true)
        {
            if (deactivateCurrentMenu)
            {
                DeactivateMenu();
            }

            if (newMenu)
            {
                newMenu.gameObject.SetActive(true);
                newMenu.SelectFirstButton();
            }
            else
            {
                Debug.LogWarning($"{name}: Attempted to activate a null menu.");
            }
        }
        
        public void DeactivateMenu()
        {
            gameObject.SetActive(false);
        }
        
        private void SelectFirstButton()
        {
            m_FirstSelectedButton.Select();
        }
        
        private void OnBackButtonClicked()
        {
            if (m_PreviousMenu)
            {
                ActivateMenu(m_PreviousMenu);
            }
            else
            {
                DeactivateMenu();
            }
        }
    }
}

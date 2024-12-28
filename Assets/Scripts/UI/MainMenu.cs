using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

namespace ChemKart
{
    public class MainMenu : Menu
    {
        [Header("Menus")]
        [SerializeField] private SettingsMenu m_SettingsMenu;
        
        [Header("Buttons")]
        [SerializeField] private Button m_SoloButton;
        [SerializeField] private Button m_MultiplayerButton;
        [SerializeField] private Button m_SettingsButton;
        [SerializeField] private Button m_QuitButton;

        [Header("Scene")]
        [SerializeField] private string m_MainGameSceneName; 

        private void Awake()
        {
            if (!m_SoloButton || !m_MultiplayerButton || !m_SettingsButton || !m_QuitButton)
            {
                Debug.LogError($"{name}: One or more Main Menu buttons are not assigned in the inspector.");
            }
        }
        
        private void OnEnable()
        {
            m_SoloButton.onClick.AddListener(OnSoloButtonClicked);
            m_MultiplayerButton.onClick.AddListener(OnMultiplayerButtonClicked);
            m_SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
            m_QuitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        
        private void OnDisable()
        {
            m_SoloButton.onClick.RemoveListener(OnSoloButtonClicked);
            m_MultiplayerButton.onClick.RemoveListener(OnMultiplayerButtonClicked);
            m_SettingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            m_QuitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }

        private void OnSoloButtonClicked()
        {
            Debug.Log("Solo Button Clicked");
            SceneManager.LoadScene(m_MainGameSceneName); 
        }
        
        private void OnMultiplayerButtonClicked()
        {
            Debug.Log("Multiplayer Button Clicked");
        }
        
        private void OnSettingsButtonClicked()
        {
            if (!m_SettingsMenu)
            {
                Debug.LogError($"{name}: Settings Menu is not assigned in the inspector.");
                return;
            }
            
            ActivateMenu(m_SettingsMenu);
        }
        
        private static void OnQuitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}

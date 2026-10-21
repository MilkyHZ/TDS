using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager I { get; private set; }

    [Header("UI Panels")]
    [SerializeField]
    private GameObject _titlePanel;
    [SerializeField]
    private GameObject _gameplayPanel;
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _settingsPanel;
    [SerializeField]
    private GameObject _winPanel;
    [SerializeField]
    private GameObject _gameOverPanel;

    [Header("Title Panel Buttons")]
    [SerializeField]
    private Button _btnPlay;

    [Header("Gameplay Panel Buttons")]
    [SerializeField]
    private Button _btnPause;

    [Header("Pause Panel Buttons")]
    [SerializeField]
    private Button _btnResume;
    [SerializeField]
    private Button _btnPauseRestart;
    [SerializeField] 
    private Button _btnSettings;

    [Header("Settings Panel Buttons")]
    [SerializeField] 
    private Button _btnBacktoPause;

    [Header("Win Panel Buttons")]
    [SerializeField] 
    private Button _btnWinRestart;

    [Header("Game Over Panel Buttons")]
    [SerializeField] 
    private Button _btnGameOverRestart;

    private void Awake()
    {
        I = this;

        if (_btnPlay != null) _btnPlay.onClick.AddListener(OnClickPlay);
        if (_btnPause != null) _btnPause.onClick.AddListener(OnClickPause);
        if (_btnResume != null) _btnResume.onClick.AddListener(OnClickResume);
        if (_btnPauseRestart != null) _btnPauseRestart.onClick.AddListener(OnClickRestart);
        if (_btnWinRestart != null) _btnWinRestart.onClick.AddListener(OnClickRestart);
        if (_btnGameOverRestart != null) _btnGameOverRestart.onClick.AddListener(OnClickRestart);

        if (_btnSettings != null) _btnSettings.onClick.AddListener(OnClickSettings);
        if (_btnBacktoPause != null) _btnBacktoPause.onClick.AddListener(OnClickBackToPause);

        ShowTitleScreen();
    }

    public void ShowTitleScreen()
    {
        HideAllPanels();
        if (_titlePanel != null) _titlePanel.SetActive(true);
    }

    public void ShowGameplayUI()
    {
        HideAllPanels();
        if (_gameplayPanel != null) _gameplayPanel.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        if (_pausePanel != null) _pausePanel.SetActive(true);
    }

    public void HidePauseMenu()
    {
        if (_pausePanel != null) _pausePanel.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        if (_settingsPanel != null) _settingsPanel.SetActive(true);
    }

    public void HideSettingsMenu()
    {
        if (_settingsPanel != null) _settingsPanel.SetActive(false);
    }

    public void ShowWinScreen()
    {
        HideAllPanels();
        if (_winPanel != null) _winPanel.SetActive(true);
    }

    public void ShowGameOverScreen()
    {
        HideAllPanels();
        if (_gameOverPanel != null) _gameOverPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        if (_titlePanel != null) _titlePanel.SetActive(false);
        if (_gameplayPanel != null) _gameplayPanel.SetActive(false);
        if (_pausePanel != null) _pausePanel.SetActive(false);
        if (_settingsPanel != null) _settingsPanel.SetActive(false);
        if (_winPanel != null) _winPanel.SetActive(false);
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
    }

    private void OnClickPlay()
    {
        if (GameControler.I != null)
        {
            GameControler.I.StartGame();
        }
        ShowGameplayUI(); 
    }


    private void OnClickPause()
    {
        ShowPauseMenu();
        if (GameControler.I != null)
        {
            GameControler.I.PauseGame();
        }
    }

    private void OnClickResume()
    {
        HidePauseMenu();
        if (GameControler.I != null)
        {
            GameControler.I.ResumeGame();
        }
    }

    private void OnClickSettings()
    {
        HidePauseMenu();
        ShowSettingsMenu();
    }

    private void OnClickBackToPause()
    {
        HideSettingsMenu();
        ShowPauseMenu();
    }

    private void OnClickRestart()
    {
        if (GameControler.I != null)
        {
            GameControler.I.RestartGameWithoutReload();
        }
        ShowGameplayUI();
    }
}
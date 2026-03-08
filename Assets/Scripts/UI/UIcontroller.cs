using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIcontroller : MonoBehaviour
{
    public static UIcontroller Instance;

    void Awake()
    {
        Instance = this;
    }

    [Header("Canvases")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _gameplayPanel;
    [SerializeField] private GameObject _miniGamePanel;

    [Header("Panels")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _gameOverPanel;

    [Header("Menu Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _closeInfoButton;

    [Header("Game Buttons")]
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _startNewGameButton;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _scoreTextTMP;
    [SerializeField] private TMP_Text _recordTextTMP;
    [SerializeField] private LocationScroller _locationScroller;
    [SerializeField] private Animator _playerAnimator;

    [Header("Sound")]
    [SerializeField] private Image _soundIcon;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;
    private bool isSoundEnabled;

    private bool isPaused = false;

    void Start()
    {
        _recordTextTMP.text = "Record: " + PlayerPrefs.GetInt("HighScore", 0).ToString();

        _playerAnimator.SetBool("idle", true);
        _playerAnimator.SetBool("run", false);

        SetAllPanelsInactive();

        if (_mainMenuPanel != null)
            _mainMenuPanel.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;

        InitializeButtons();
        SoundStart();
    }

    public void SetScore(int score)
    {
        if (_scoreTextTMP != null)
            _scoreTextTMP.text = score.ToString();
    }

    void InitializeButtons()
    {
        if (_startButton != null)
        {
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(StartGame);
        }

        if (_soundButton != null)
        {
            _soundButton.onClick.RemoveAllListeners();
            _soundButton.onClick.AddListener(SoundOnOff);
        }

        if (_infoButton != null)
        {
            _infoButton.onClick.RemoveAllListeners();
            _infoButton.onClick.AddListener(() => _infoPanel.SetActive(true));
        }

        if (_closeInfoButton != null)
        {
            _closeInfoButton.onClick.RemoveAllListeners();
            _closeInfoButton.onClick.AddListener(() => _infoPanel.SetActive(false));
        }

        if (_pauseButton != null)
        {
            _pauseButton.onClick.RemoveAllListeners();
            _pauseButton.onClick.AddListener(TogglePause);
        }

        if (_resumeButton != null)
        {
            _resumeButton.onClick.RemoveAllListeners();
            _resumeButton.onClick.AddListener(Resume);
        }

        if (_restartButton != null)
        {
            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(RestartGame);
        }
        if (_startNewGameButton != null)
        {
            _startNewGameButton.onClick.RemoveAllListeners();
            _startNewGameButton.onClick.AddListener(RestartGame);
        }
    }

    void StartGame()
    {
        SetAllPanelsInactive();

        if (_gameplayPanel != null)
            _gameplayPanel.SetActive(true);

        if (_miniGamePanel != null)
            _miniGamePanel.SetActive(false);

        _scoreTextTMP.text = "0";

        Time.timeScale = 1f;
        isPaused = false;

        if (_locationScroller != null)
            _locationScroller.StartScrolling();

        _playerAnimator.SetBool("idle", false);
        _playerAnimator.SetBool("run", true);
        AudioManager.Instance.PlayMusic("game");
    }

    void TogglePause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;

            if (_pausePanel != null)
                _pausePanel.SetActive(true);
        }
    }

    void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (_pausePanel != null)
            _pausePanel.SetActive(false);
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowMiniGame()
    {
        if (_miniGamePanel != null)
            _miniGamePanel.SetActive(true);
        // Time.timeScale = 0f;
    }

    public System.Collections.IEnumerator HideMiniGame()
    {
        if (_miniGamePanel != null)
            _miniGamePanel.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
    }

    public void ShowGameOver()
    {
        AudioManager.Instance.PlaySFX("gameOver");
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(true);
    }

    void SetAllPanelsInactive()
    {
        if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
        if (_gameplayPanel != null) _gameplayPanel.SetActive(false);
        if (_miniGamePanel != null) _miniGamePanel.SetActive(false);
        if (_infoPanel != null) _infoPanel.SetActive(false);
        if (_pausePanel != null) _pausePanel.SetActive(false);
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
    }

    void SoundOnOff()
    {
        isSoundEnabled = !isSoundEnabled;

        AudioListener.volume = isSoundEnabled ? 1f : 0f;

        PlayerPrefs.SetInt("Sound", isSoundEnabled ? 1 : 0);

        UpdateSoundIcon();
    }

    void SoundStart()
    {
        isSoundEnabled = PlayerPrefs.GetInt("Sound", 1) == 1;

        AudioListener.volume = isSoundEnabled ? 1f : 0f;

        UpdateSoundIcon();
    }

    void UpdateSoundIcon()
    {
        if (_soundIcon != null)
        {
            _soundIcon.sprite = isSoundEnabled ? _soundOnSprite : _soundOffSprite;
        }
    }
}
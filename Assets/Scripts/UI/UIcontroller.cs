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

    [Header("Panels")]
    [SerializeField] private GameObject _pausePanel;

    [Header("Menu Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _infoButton;

    [Header("Game Buttons")]
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _scoreTextTMP;
    [SerializeField] private TMP_Text _recordTextTMP;
    [SerializeField] private LocationScroller _locationScroller;
    [SerializeField] private Animator _playerAnimator;

    private bool isPaused = false;

    void Start()
    {
        _recordTextTMP.text = "Record:"+ PlayerPrefs.GetInt("HighScore", 0).ToString();
        _playerAnimator.SetBool("idle", true);
        _playerAnimator.SetBool("run", false);
        SetAllPanelsInactive();

        if (_mainMenuPanel != null)
            _mainMenuPanel.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;

        InitializeButtons();
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
            _soundButton.onClick.AddListener(() => Debug.Log("Sound button clicked!"));
        }

        if (_infoButton != null)
        {
            _infoButton.onClick.RemoveAllListeners();
            _infoButton.onClick.AddListener(() => Debug.Log("Info button clicked!"));
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
    }

    void StartGame()
    {
        SetAllPanelsInactive();

        if (_gameplayPanel != null)
            _gameplayPanel.SetActive(true);

        _scoreTextTMP.text = "0";
        Time.timeScale = 1f;
        isPaused = false;
        _locationScroller.StartScrolling();
        _playerAnimator.SetBool("idle", false);
        _playerAnimator.SetBool("run", true);
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

    void SetAllPanelsInactive()
    {
        if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
        if (_gameplayPanel != null) _gameplayPanel.SetActive(false);
        if (_pausePanel != null) _pausePanel.SetActive(false);
    }
}
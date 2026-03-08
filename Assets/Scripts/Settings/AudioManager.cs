using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    void Awake()
    {
        Instance = this;
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Audio Clips Music")]
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioClip _miniGameMusic;

    [Header("Audio Clips SFX")]
    [SerializeField] private AudioClip _jumpMusic;
    [SerializeField] private AudioClip _slideMusic;
    [SerializeField] private AudioClip _hitMusic;
    [SerializeField] private AudioClip _slipMusic;
    [SerializeField] private AudioClip _whistleMusic;
    [SerializeField] private AudioClip _gameOverMusic;

    private void Start()
    {
        PlayMusic("menu");
    }
    public void PlayMusic(string musicName)
    {
        AudioClip targetMusic = null;

        switch (musicName)
        {
            case "menu":
                targetMusic = _menuMusic;
                break;
            case "game":
                targetMusic = _gameMusic;
                break;
            case "miniGame":
                targetMusic = _miniGameMusic;
                break;
        }

        if (targetMusic != null && _musicSource.clip != targetMusic)
        {
            _musicSource.clip = targetMusic;
            _musicSource.Play();
        }
    }

    public void PlaySFX(string clipName)
    {
        switch (clipName)
        {
            case "jump":
                _sfxSource.PlayOneShot(_jumpMusic);
                break;
            case "slide":
                _sfxSource.PlayOneShot(_slideMusic);
                break;
            case "hit":
                _sfxSource.PlayOneShot(_hitMusic);
                break;
            case "slip":
                _sfxSource.PlayOneShot(_slipMusic);
                break;
            case "whistle":
                _sfxSource.PlayOneShot(_whistleMusic);
                break;
            case "gameOver":
                _sfxSource.PlayOneShot(_gameOverMusic);
                break;
        }
    }
}
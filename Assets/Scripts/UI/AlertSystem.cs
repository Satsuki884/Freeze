using UnityEngine;
using UnityEngine.UI;

public class AlertSystem : MonoBehaviour
{
    public static AlertSystem Instance;

    [Header("Alert")]
    [SerializeField] private float maxAlert = 5f;
    [SerializeField] private float currentAlert = 0;

    [SerializeField] private float decreaseSpeed = 0.25f;

    [Header("UI")]
    [SerializeField] private Slider alertSlider;

    bool miniGameStarted = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        DecreaseAlert();

        if (alertSlider != null)
            alertSlider.value = currentAlert;
    }

    void DecreaseAlert()
    {
        if (currentAlert > 0 && !miniGameStarted)
        {
            currentAlert -= decreaseSpeed * Time.deltaTime;
            currentAlert = Mathf.Clamp(currentAlert, 0, maxAlert);
        }
    }

    public void AddAlert(float value)
    {
        if (miniGameStarted) return;

        currentAlert += value;
        currentAlert = Mathf.Clamp(currentAlert, 0, maxAlert);

        if (currentAlert >= maxAlert)
        {
            StartMiniGame();
        }
    }

    void StartMiniGame()
    {
        miniGameStarted = true;

        StartCoroutine(MiniGameController.Instance.StartMiniGame());
    }

    public void ResetAlert()
    {
        currentAlert = 0;
        miniGameStarted = false;
    }
}
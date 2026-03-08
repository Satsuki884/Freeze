using System.Collections;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    public static MiniGameController Instance;

    void Awake()
    {
        Instance = this;
    }

    public SliderController slider;

    public IEnumerator StartMiniGame()
    {
        // зупиняємо рух світу
        LocationScroller.Instance.StopScrolling();
        PlayerController.Instance.SetRun(false);
        PlayerController.Instance.SetFreeze(true);
        RatController.Instance.SetRun(false);
        AudioManager.Instance.PlayMusic("miniGame");

        UIcontroller.Instance.ShowMiniGame();

        yield return new WaitForSecondsRealtime(0.2f);

        // ставимо гру на паузу
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(1f);

        slider.StartSlider();
    }

    public void Success()
    {
        StartCoroutine(UIcontroller.Instance.HideMiniGame());

        AlertSystem.Instance.ResetAlert();

        // запускаємо світ знову
        LocationScroller.Instance.StartScrolling();
        PlayerController.Instance.SetFreeze(false);
        PlayerController.Instance.SetRun(true);
        RatController.Instance.SetRun(true);
        AudioManager.Instance.PlayMusic("game");
    }

    public void Fail()
    {
        StartCoroutine(UIcontroller.Instance.HideMiniGame());

        LocationScroller.Instance.StopScrolling();

        Debug.Log("Game Over");

        UIcontroller.Instance.ShowGameOver();
    }
}
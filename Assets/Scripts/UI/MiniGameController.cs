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
    }

    public void Fail()
    {
        StartCoroutine(UIcontroller.Instance.HideMiniGame());

        LocationScroller.Instance.StopScrolling();

        Debug.Log("Game Over");

        UIcontroller.Instance.ShowGameOver();
    }
}
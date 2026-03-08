using UnityEngine;

public class SliderController : MonoBehaviour
{
    public RectTransform slider;
    public RectTransform sliderArea;
    public RectTransform greenZone;

    public float speed = 800f;

    bool active = false;
    int direction = 1;

    float minX;
    float maxX;

    // void Start()
    // {
    //     minX = -sliderArea.rect.width / 2;
    //     maxX = sliderArea.rect.width / 2;
    // }

    void Update()
    {
        if (!active) return;

        MoveSlider();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

    public void StartSlider()
    {
        float width = sliderArea.rect.width;

        minX = -width / 2;
        maxX = width / 2;

        RandomizeGreenZone();

        slider.anchoredPosition = new Vector2(minX, slider.anchoredPosition.y);

        direction = 1;
        active = true;
    }

    void MoveSlider()
    {
        slider.anchoredPosition += Vector2.right * speed * direction * Time.unscaledDeltaTime;

        if (slider.anchoredPosition.x > maxX)
        {
            direction = -1;
        }

        if (slider.anchoredPosition.x < minX)
        {
            direction = 1;
        }
    }

    void RandomizeGreenZone()
    {
        float width = sliderArea.rect.width;

        float randomX = Random.Range(-width / 2 + 100, width / 2 - 100);

        greenZone.anchoredPosition = new Vector2(randomX, greenZone.anchoredPosition.y);
    }

    void CheckHit()
    {
        float pos = slider.anchoredPosition.x;

        float min = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float max = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        if (pos > min && pos < max)
        {
            MiniGameController.Instance.Success();
        }
        else
        {
            MiniGameController.Instance.Fail();
        }

        active = false;
    }
}
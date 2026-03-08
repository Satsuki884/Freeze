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

    public void StartSlider()
    {
        float areaWidth = sliderArea.rect.width;
        float sliderWidth = slider.rect.width;

        // враховуємо ширину повзунка
        minX = -areaWidth / 2 + sliderWidth / 2;
        maxX = areaWidth / 2 - sliderWidth / 2;

        slider.anchoredPosition = new Vector2(minX, slider.anchoredPosition.y);

        RandomizeGreenZone();

        direction = 1;
        active = true;
    }

    void Update()
    {
        if (!active) return;

        MoveSlider();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

    void MoveSlider()
    {
        float newX = slider.anchoredPosition.x + speed * direction * Time.unscaledDeltaTime;

        // жорстке обмеження
        newX = Mathf.Clamp(newX, minX, maxX);

        slider.anchoredPosition = new Vector2(newX, slider.anchoredPosition.y);

        // зміна напрямку
        if (newX >= maxX)
            direction = -1;

        if (newX <= minX)
            direction = 1;
    }

    void RandomizeGreenZone()
    {
        float areaWidth = sliderArea.rect.width;
        float zoneWidth = greenZone.rect.width;

        float minZone = -areaWidth / 2 + zoneWidth / 2;
        float maxZone = areaWidth / 2 - zoneWidth / 2;

        float randomX = Random.Range(minZone, maxZone);

        greenZone.anchoredPosition = new Vector2(randomX, greenZone.anchoredPosition.y);
    }

    void CheckHit()
    {
        float sliderX = slider.anchoredPosition.x;

        float zoneMin = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float zoneMax = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        active = false;

        if (sliderX >= zoneMin && sliderX <= zoneMax)
        {
            MiniGameController.Instance.Success();
        }
        else
        {
            MiniGameController.Instance.Fail();
        }
    }
}
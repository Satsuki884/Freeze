using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AlertAnimation : MonoBehaviour
{
    public Image image;

    public Sprite frame1;
    public Sprite frame2;

    public float frameTime = 0.15f;

    public IEnumerator PlayAnimation()
    {
        image.sprite = frame1;
        yield return new WaitForSecondsRealtime(frameTime);

        image.sprite = frame2;
    }
}
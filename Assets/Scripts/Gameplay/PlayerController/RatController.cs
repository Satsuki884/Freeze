using System.Collections;
using UnityEngine;

public class RatController : MonoBehaviour
{
    public static RatController Instance;

    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private float speed = 5f;
    [SerializeField] private float targetX = 15f;
    [SerializeField] private Animator animator;

    public void SetRun(bool value)
    {
        animator.SetBool("run", value);
    }

    public IEnumerator StartIntroRun()
    {
        if (animator != null)
            animator.SetBool("run", true);

        while (transform.position.x < targetX)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            yield return null;
        }

        Vector3 pos = transform.position;
        pos.x = targetX;
        transform.position = pos;

    }
}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.6f;

    [Header("Slowdown")]
    [SerializeField] private float slowdownMultiplier = 0.5f;
    [SerializeField] private float slowdownTime = 1.5f;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerVisual;

    private Vector3 startPosition;

    private bool isJumping;
    private bool isSliding;
    private bool isStunned;

    void Start()
    {
        startPosition = playerVisual.localPosition;
    }

    void Update()
    {
        if (isStunned) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartJump();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartSlide();
        }
    }

    void StartJump()
    {
        if (isJumping || isSliding) return;

        StartCoroutine(JumpRoutine());
    }

    System.Collections.IEnumerator JumpRoutine()
    {
        isJumping = true;

        animator.SetBool("jump", true);

        float timer = 0;

        while (timer < jumpDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / jumpDuration;

            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;

            playerVisual.localPosition = startPosition + Vector3.up * height;

            yield return null;
        }

        playerVisual.localPosition = startPosition;

        animator.SetBool("jump", false);

        isJumping = false;
    }

    void StartSlide()
    {
        if (isSliding || isJumping) return;

        StartCoroutine(SlideRoutine());
    }

    System.Collections.IEnumerator SlideRoutine()
    {
        isSliding = true;

        animator.SetBool("slide", true);

        yield return new WaitForSeconds(0.8f);

        animator.SetBool("slide", false);

        isSliding = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();

        if (obstacle == null) return;

        if (obstacle.type == ObstacleType.jump)
        {
            if (!isJumping)
            {
                HitHighObstacle();
            }
        }

        if (obstacle.type == ObstacleType.slide)
        {
            if (!isSliding)
            {
                HitLowObstacle();
            }
        }
    }

    void HitHighObstacle()
    {
        StartCoroutine(StunRoutine("hit"));
    }

    void HitLowObstacle()
    {
        StartCoroutine(StunRoutine("slip"));
    }

    System.Collections.IEnumerator StunRoutine(string boolName)
    {
        isStunned = true;

        animator.SetBool(boolName, true);

        float originalTime = Time.timeScale;

        Time.timeScale = slowdownMultiplier;

        yield return new WaitForSecondsRealtime(slowdownTime);

        Time.timeScale = originalTime;

        animator.SetBool(boolName, false);

        isStunned = false;
    }
}
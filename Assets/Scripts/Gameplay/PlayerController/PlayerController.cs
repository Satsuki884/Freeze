using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    void Awake()
    {
        Instance = this;
    }

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.6f;

    [Header("Slowdown")]
    [SerializeField] private float slowdownMultiplier = 0.5f;
    [SerializeField] private float slowdownTime = 1.5f;

    [Header("SlowUp")]
    [SerializeField] private float slowupMultiplier = 2f;
    [SerializeField] private float slowupTime = 1.5f;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerVisual;

    [SerializeField] private float reactionWindow = 0.25f;
    private float lastJumpPressTime = -10f;
    private float lastSlidePressTime = -10f;

    private Vector3 startPosition;
    public void SetRun(bool value)
    {
        animator.SetBool("run", value);
    }

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
            lastJumpPressTime = Time.time;
            StartJump();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            lastSlidePressTime = Time.time;
            StartSlide();
        }
    }

    void StartJump()
    {
        if (isJumping || isSliding) return;

        AudioManager.Instance.PlaySFX("jump");

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

        AudioManager.Instance.PlaySFX("slide");

        StartCoroutine(SlideRoutine());
    }

    System.Collections.IEnumerator SlideRoutine()
    {
        isSliding = true;

        animator.SetBool("slide", true);

        float originalTime = Time.timeScale;

        Time.timeScale = slowupMultiplier;

        yield return null;//new WaitForSecondsRealtime(slowupTime);

        Time.timeScale = originalTime;

        //yield return null;//new WaitForSeconds(0.8f);

        animator.SetBool("slide", false);

        isSliding = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle == null) return;

        if (obstacle.triggered) return;
        obstacle.triggered = true;

        if (obstacle.type == ObstacleType.jump)
        {
            bool reactedInTime = Time.time - lastJumpPressTime <= reactionWindow;

            if (!reactedInTime)
            {
                HitHighObstacle();
            }
        }

        if (obstacle.type == ObstacleType.slide)
        {
            bool reactedInTime = Time.time - lastSlidePressTime <= reactionWindow;

            if (!reactedInTime)
            {
                HitLowObstacle();
            }
        }
    }

    void HitHighObstacle()
    {
        AudioManager.Instance.PlaySFX("hit");
        StartCoroutine(StunRoutine("hit"));
        AlertSystem.Instance.AddAlert(1f);
    }

    void HitLowObstacle()
    {
        AudioManager.Instance.PlaySFX("slip");
        StartCoroutine(StunRoutine("slip"));
        AlertSystem.Instance.AddAlert(1f);
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
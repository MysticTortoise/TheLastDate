using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
public class Customer : MonoBehaviour
{
    [Header("Grid")]
    public int currentGrid;

    [Header("Requested Food")]
    public int requestedFoodID; // 0..2

    [Header("Bubble")]
    public GameObject bubble; // assign in inspector

    [Header("Thinking Text (TMP)")]
    public TMP_Text thinkingText; // assign the TMPPro object here
    public float dotsInterval = 0.35f;

    [Header("Patience / Countdown")]
    public float thinkDuration = 5f;          // how long they do . .. ...
    public float countdownStepSeconds = 1f;   // 1 second per number

    [Header("Spawn Animation")]
    public float spawnAnimDuration = 0.35f;
    public float spawnMoveDownAmount = 0.25f;

    [Header("Leave Animation")]
    public float leaveAnimDuration = 0.4f;
    public float leaveMoveUpAmount = 0.4f;

    private SpriteRenderer sr;
    private bool isLeaving = false;
    private bool isServed = false;

    // manager reference so customer can trigger "timeout = disliked"
    private InteractionManager manager;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Called by spawner right after Instantiate so this customer can notify the manager on timeout.
    /// </summary>
    public void Init(InteractionManager managerRef)
    {
        manager = managerRef;
    }

    private void Start()
    {
        SetGridFromPosition();
        StartCoroutine(SpawnAnim());
        StartCoroutine(PatienceRoutine());
    }

    void SetGridFromPosition()
    {
        float x = transform.position.x;

        float d0 = Mathf.Abs(x - 0f);
        float d1 = Mathf.Abs(x - 1.5f);
        float d2 = Mathf.Abs(x - 3f);

        if (d0 <= d1 && d0 <= d2) currentGrid = 0;
        else if (d1 <= d0 && d1 <= d2) currentGrid = 1;
        else currentGrid = 2;
    }

    System.Collections.IEnumerator SpawnAnim()
    {
        Vector3 endPos = transform.position;
        Vector3 startPos = endPos + new Vector3(0f, spawnMoveDownAmount, 0f);
        transform.position = startPos;

        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        float t = 0f;
        while (t < spawnAnimDuration)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / spawnAnimDuration);

            transform.position = Vector3.Lerp(startPos, endPos, u);

            Color cc = sr.color;
            cc.a = Mathf.Lerp(0f, 1f, u);
            sr.color = cc;

            yield return null;
        }

        transform.position = endPos;

        Color final = sr.color;
        final.a = 1f;
        sr.color = final;
    }

    // -------- NEW: Patience / timeout behavior --------

    System.Collections.IEnumerator PatienceRoutine()
    {
        // Wait until spawn anim has likely finished (optional small buffer)
        yield return new WaitForSeconds(spawnAnimDuration);

        // Phase 1: ". .. ..." looping for thinkDuration
        float elapsed = 0f;
        int dotState = 0; // 0=".", 1="..", 2="..."

        while (elapsed < thinkDuration && !isServed && !isLeaving)
        {
            if (thinkingText != null)
            {
                thinkingText.text = dotState == 0 ? "." : (dotState == 1 ? ".." : "...");
            }

            dotState = (dotState + 1) % 3;

            float step = dotsInterval;
            elapsed += step;
            yield return new WaitForSeconds(step);
        }

        if (isServed || isLeaving) yield break;

        // Phase 2: countdown 3,2,1
        for (int n = 3; n >= 1; n--)
        {
            if (isServed || isLeaving) yield break;

            if (thinkingText != null)
                thinkingText.text = n.ToString();

            yield return new WaitForSeconds(countdownStepSeconds);
        }

        if (isServed || isLeaving) yield break;

        // Timeout: treat as "didn't like food"
        if (manager != null)
        {
            manager.OnCustomerTimedOut(this); // manager applies -3 charm + sad popup
        }

        LeaveBecauseTimeout();
    }

    void LeaveBecauseTimeout()
    {
        // Hide bubble + play leave anim
        ServeAndLeave();
    }

    // ---------- Called when served by player ----------
    public void MarkServed()
    {
        isServed = true;
    }

    public void ServeAndLeave()
    {
        if (isLeaving) return;
        isLeaving = true;

        if (bubble != null)
            bubble.SetActive(false);

        if (thinkingText != null)
            thinkingText.gameObject.SetActive(false);

        StartCoroutine(LeaveAnim());
    }

    System.Collections.IEnumerator LeaveAnim()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0f, leaveMoveUpAmount, 0f);

        float t = 0f;

        while (t < leaveAnimDuration)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / leaveAnimDuration);

            transform.position = Vector3.Lerp(startPos, endPos, u);

            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, u);
            sr.color = c;

            yield return null;
        }

        Destroy(gameObject);
    }
}

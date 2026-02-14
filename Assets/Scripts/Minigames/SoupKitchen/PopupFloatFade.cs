using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PopupFloatFade : MonoBehaviour
{
    [Header("Motion")]
    public Vector3 moveOffset = new Vector3(0f, 0.6f, 0f);
    public float duration = 0.6f;

    [Header("Fade")]
    [Range(0f, 1f)] public float fadeInPortion = 0.2f; // first 20% fade in, rest fade out

    private SpriteRenderer sr;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        endPos = startPos + moveOffset;

        // start invisible
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
    }

    private void Update()
    {
        float t = Mathf.Clamp01(Time.deltaTime / Mathf.Max(0.0001f, duration));
        // We'll accumulate time manually:
    }

    private float elapsed = 0f;

    private void LateUpdate()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / Mathf.Max(0.0001f, duration));

        // move
        transform.position = Vector3.Lerp(startPos, endPos, t);

        // fade in then out
        float a;
        if (t <= fadeInPortion)
        {
            float fi = t / Mathf.Max(0.0001f, fadeInPortion);
            a = Mathf.Lerp(0f, 1f, fi);
        }
        else
        {
            float fo = (t - fadeInPortion) / Mathf.Max(0.0001f, (1f - fadeInPortion));
            a = Mathf.Lerp(1f, 0f, fo);
        }

        Color c = sr.color;
        c.a = a;
        sr.color = c;

        if (t >= 1f)
            Destroy(gameObject);
    }
}

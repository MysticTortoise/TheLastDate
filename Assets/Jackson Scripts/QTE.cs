using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class QTE : MonoBehaviour
{
    [Header("References")]
    public Transform ring;   // assign child ring here

    [Header("Timing")]
    public float duration = 1f;      // time to fully close
    public float hitWindow = 0.1f;   // seconds around close moment

    [Header("Scale")]
    public float startScale = 3f;
    public float endScale = 1f;

    private float startTime;
    private bool resolved;

    void OnEnable()
    {
        startTime = Time.time;
        resolved = false;

        if (ring != null)
            ring.localScale = Vector3.one * startScale;
    }

    void Update()
    {
        if (resolved) return;

        float elapsed = Time.time - startTime;
        float t = Mathf.Clamp01(elapsed / duration);

        // Shrink ring
        if (ring != null)
        {
            float scale = Mathf.Lerp(startScale, endScale, t);
            ring.localScale = Vector3.one * scale;
        }

       // Click detection (New Input System)
if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
{
    Vector2 mouseScreen = Mouse.current.position.ReadValue();
    Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

    Collider2D col = GetComponent<Collider2D>();
    if (col.OverlapPoint(mouseWorld))
    {
        float timeToClose = duration - elapsed;
        bool inWindow = Mathf.Abs(timeToClose) <= hitWindow;

        if (inWindow)
        {
            resolved = true;
            Debug.Log("QTE HIT");
            Destroy(gameObject);
        }
        // else: too early/late -> do nothing
    }
}


        // Cleanup if finished (no punishment)
        if (elapsed > duration + hitWindow)
        {
            Destroy(gameObject);
        }
    }
}

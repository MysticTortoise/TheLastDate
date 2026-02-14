using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class QTE : MonoBehaviour
{
    [Header("References")]
    public Transform ring; // assign in inspector

    [Header("Timing / Motion")]
    public float duration = 1f;

    [Header("Scale")]
    public float startScale = 3f;
    public float endScale = 1f;

    [Header("Hit Window (SCALE-BASED)")]
    [Tooltip("How close the ring scale must be to endScale to allow a hit. Example: 0.08 means +/-0.08 scale units.")]
    public float hitScaleTolerance = 0.08f;

    [Header("Gym Slider")]
    public string sliderTag = "gymSlide";
    public float hitIncrease = 0.10f;
    public float missDecrease = 0.07f;

    [Header("State (Read Only)")]
    public bool canHit = false;

    private float _startTime;
    private bool _resolved;
    private bool _hit;
    private Slider _gymSlider;
    private Collider2D _col;

    void Awake()
    {
        _col = GetComponent<Collider2D>();

        GameObject sliderGO = GameObject.FindGameObjectWithTag(sliderTag);
        if (sliderGO != null) _gymSlider = sliderGO.GetComponent<Slider>();

        if (_gymSlider == null)
            Debug.LogWarning($"QTE: No Slider found with tag '{sliderTag}' (or it has no Slider component).");
    }

    void OnEnable()
    {
        _startTime = Time.time;
        _resolved = false;
        _hit = false;
        canHit = false;

        if (ring != null)
            ring.localScale = Vector3.one * startScale;
    }

    void Update()
    {
        if (_resolved) return;
        if (ring == null) return;

        float elapsed = Time.time - _startTime;
        float t = Mathf.Clamp01(elapsed / duration);

        // Shrink ring
        float currentScale = Mathf.Lerp(startScale, endScale, t);
        ring.localScale = Vector3.one * currentScale;

        // Determine if ring is within the "hit zone" (scale window around endScale)
        float scaleDiff = Mathf.Abs(currentScale - endScale);

        // Once true, it stays true (your request)
        if (!canHit && scaleDiff <= hitScaleTolerance)
            canHit = true;

        // Click handling
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (ClickedOnQTE())
            {
                if (!canHit)
                {
                    // Clicked too early: treat as miss (penalize + destroy)
                    ResolveMiss();
                    return;
                }
                else
                {
                    // Clicked during/after hit window opened: treat as hit
                    ResolveHit();
                    return;
                }
            }
        }

        // Auto-miss once we've passed the hit zone without hitting:
        // When the ring has finished closing AND we didn't hit, disappear and apply missDecrease.
        // (This ensures it doesn't hang around if canHit turned true but user didn't click.)
        if (elapsed >= duration && !_hit)
        {
            ResolveMiss();
            return;
        }
    }

    private bool ClickedOnQTE()
    {
        if (Camera.main == null) return false;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        return _col != null && _col.OverlapPoint(mouseWorld);
    }

    private void ResolveHit()
    {
        _hit = true;
        _resolved = true;
        AdjustGym(+hitIncrease);
        Destroy(gameObject);
    }

    private void ResolveMiss()
    {
        _resolved = true;
        // You said: if it doesn't (hit) and destroys then decrease
        AdjustGym(-missDecrease);
        Destroy(gameObject);
    }

    private void AdjustGym(float delta)
    {
        if (_gymSlider == null) return;

        float newValue = Mathf.Clamp(_gymSlider.value + delta, _gymSlider.minValue, _gymSlider.maxValue);
        _gymSlider.value = newValue;
    }
}

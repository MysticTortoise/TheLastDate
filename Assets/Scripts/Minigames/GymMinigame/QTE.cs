using System.Collections.Generic;
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

    [Header("Popup Text")]
    public GameObject popupPrefab; // prefab with WorldTextPopup (TMP)
    public Vector3 popupOffset = new Vector3(0f, 1.0f, 0f);
    public List<string> popupMessages = new List<string>()
    {
        "Nice!",
        "Good rep!",
        "Clean!",
        "Let’s go!",
        "Solid!"
    };
    public bool showPopupOnMiss = true; // if false, only on hit

    [Header("State (Read Only)")]
    public bool canHit = false;

    private float _startTime;
    private bool _resolved;
    private bool _hit;
    private Slider _gymSlider;
    private Collider2D _col;

    // NEW: StrengthManager reference
    private StrengthManager _strengthManager;

    void Awake()
    {
        _col = GetComponent<Collider2D>();

        GameObject sliderGO = GameObject.FindGameObjectWithTag(sliderTag);
        if (sliderGO != null) _gymSlider = sliderGO.GetComponent<Slider>();

        if (_gymSlider == null)
            Debug.LogWarning($"QTE: No Slider found with tag '{sliderTag}' (or it has no Slider component).");

        _strengthManager = FindAnyObjectByType<StrengthManager>();
        if (_strengthManager == null)
            Debug.LogWarning("QTE: No StrengthManager found in the scene (looks won't decrease on miss).");
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

        // Once true, it stays true
        if (!canHit && scaleDiff <= hitScaleTolerance)
            canHit = true;

        // Click handling (New Input System)
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

        // Auto-miss when the ring has finished closing and we didn't hit
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
        SpawnPopup();

        Destroy(gameObject);
    }

    private void ResolveMiss()
    {
        _resolved = true;

        // Check if slider is empty BEFORE changing it
        bool sliderWasEmpty = false;

        if (_gymSlider != null)
            sliderWasEmpty = Mathf.Approximately(_gymSlider.value, _gymSlider.minValue);

        // Apply normal gym decrease
        AdjustGym(-missDecrease);

        // ONLY lose looks if slider was already empty
        if (sliderWasEmpty && _strengthManager != null)
        {
            _strengthManager.ModifyLooks(-1, showPopup: true);
        }

        if (showPopupOnMiss)
            SpawnPopup();

        Destroy(gameObject);
    }

    private void SpawnPopup()
    {
        if (popupPrefab == null) return;
        if (popupMessages == null || popupMessages.Count == 0) return;

        string msg = popupMessages[Random.Range(0, popupMessages.Count)];

        GameObject go = Instantiate(popupPrefab, transform.position + popupOffset, Quaternion.identity);

        // Your popup prefab should have WorldTextPopup (or similar) on it
        var popup = go.GetComponent<WorldTextPopup>();
        if (popup != null)
            popup.SetText(msg);
    }

    private void AdjustGym(float delta)
    {
        if (_gymSlider == null) return;

        float newValue = Mathf.Clamp(_gymSlider.value + delta, _gymSlider.minValue, _gymSlider.maxValue);
        _gymSlider.value = newValue;
    }
}
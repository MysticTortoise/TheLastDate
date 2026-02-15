using UnityEngine;
using UnityEngine.UI;

public class StrengthManager : MonoBehaviour
{
    [Header("Gym Slider")]
    public string sliderTag = "gymSlide";
    public Slider gymSlider;

    [Header("Looks")]
    public int maxLooks = 5;

    [Header("Looks Indicators")]
    public GameObject looksIndicator1;
    public GameObject looksIndicator2;
    public GameObject looksIndicator3;
    public GameObject looksIndicator4;
    public GameObject looksIndicator5;

    [Header("Looks Popup")]
    public GameObject looksTextPopupPrefab;     // same prefab system you used for empathy
    public Transform looksPopupSpawnPoint;      // optional spawn point
    public Vector3 looksPopupOffset = new Vector3(0f, 1.6f, 0f); // fallback if no spawn point

    [Header("Win State")]
    public GameObject win;
    public bool HasWon { get; private set; }

void Awake()
{
    // Find slider
    if (gymSlider == null)
    {
        var go = GameObject.FindGameObjectWithTag(sliderTag);
        if (go != null) gymSlider = go.GetComponent<Slider>();
    }

    if (gymSlider == null)
        Debug.LogWarning($"StrengthManager: No Slider found with tag '{sliderTag}' (or missing Slider component).");
}


    void Start()
    {
        HasWon = false;

        if (win != null)
            win.SetActive(false);

        RefreshIndicators();
    }

    void Update()
    {
        if (HasWon) return;
        if (gymSlider == null) return;
        if (PlayerGlobalHandler.GlobalHandler == null || PlayerGlobalHandler.GlobalHandler.stats == null) return;

        // Use threshold to avoid float weirdness (esp if slider is 0..1)
        if (gymSlider.value >= gymSlider.maxValue - 0.0001f)
        {
            gymSlider.value = gymSlider.minValue;

            int before = PlayerGlobalHandler.GlobalHandler.stats.looks;

            // Increment GLOBAL looks
            PlayerGlobalHandler.GlobalHandler.stats.looks = Mathf.Min(PlayerGlobalHandler.GlobalHandler.stats.looks + 1, maxLooks);

            int after = PlayerGlobalHandler.GlobalHandler.stats.looks;

            // Popup only if it actually increased (not clamped at max)
            if (after > before)
                SpawnLooksPopup(+1);

            RefreshIndicators();

            if (PlayerGlobalHandler.GlobalHandler.stats.looks >= maxLooks)
                TriggerWin();
        }
    }

    void SpawnLooksPopup(int delta)
    {
        if (looksTextPopupPrefab == null) return;

        Vector3 pos = (looksPopupSpawnPoint != null)
            ? looksPopupSpawnPoint.position
            : transform.position + looksPopupOffset;

        pos.z = 0f; // good default for 2D

        GameObject go = Instantiate(looksTextPopupPrefab, pos, Quaternion.identity);

        TextPopupSetTMP setter = go.GetComponent<TextPopupSetTMP>();
        if (setter != null)
        {
            string sign = delta > 0 ? "+" : "";
            setter.SetText($"{sign}{delta} Looks");
        }
    }

    void TriggerWin()
    {
        HasWon = true;

        if (win != null)
            win.SetActive(true);

        Debug.Log("WIN CONDITION REACHED (looks >= 5)");
    }

    void RefreshIndicators()
    {
        if (PlayerGlobalHandler.GlobalHandler == null || PlayerGlobalHandler.GlobalHandler.stats == null) return;

        int looks = PlayerGlobalHandler.GlobalHandler.stats.looks;

        // Turn all OFF first
        if (looksIndicator1 != null) looksIndicator1.SetActive(false);
        if (looksIndicator2 != null) looksIndicator2.SetActive(false);
        if (looksIndicator3 != null) looksIndicator3.SetActive(false);
        if (looksIndicator4 != null) looksIndicator4.SetActive(false);
        if (looksIndicator5 != null) looksIndicator5.SetActive(false);

        // Turn ON based on looks
        if (looks >= 1 && looksIndicator1 != null) looksIndicator1.SetActive(true);
        if (looks >= 2 && looksIndicator2 != null) looksIndicator2.SetActive(true);
        if (looks >= 3 && looksIndicator3 != null) looksIndicator3.SetActive(true);
        if (looks >= 4 && looksIndicator4 != null) looksIndicator4.SetActive(true);
        if (looks >= 5 && looksIndicator5 != null) looksIndicator5.SetActive(true);
    }

    public int strength
    {
        get
        {
            if (PlayerGlobalHandler.GlobalHandler == null || PlayerGlobalHandler.GlobalHandler.stats == null)
                return 0;

            return PlayerGlobalHandler.GlobalHandler.stats.looks;
        }
        set
        {
            if (PlayerGlobalHandler.GlobalHandler == null || PlayerGlobalHandler.GlobalHandler.stats == null)
                return;

            PlayerGlobalHandler.GlobalHandler.stats.looks = Mathf.Clamp(value, 0, maxLooks);
        }
    }
}

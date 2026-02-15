using UnityEngine;
using UnityEngine.UI;

public class StrengthManager : MonoBehaviour
{
    [Header("Gym Slider")]
    public string sliderTag = "gymSlide";
    public Slider gymSlider;

    [Header("Global Stats")]
    public PlayerGlobalHandler playerGlobal;

    [Header("Looks")]
    public int maxLooks = 5;

    [Header("Looks Indicators")]
    public GameObject looksIndicator1;
    public GameObject looksIndicator2;
    public GameObject looksIndicator3;
    public GameObject looksIndicator4;
    public GameObject looksIndicator5;

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
            Debug.LogWarning($"LooksManager: No Slider found with tag '{sliderTag}' (or missing Slider component).");

        // Find global handler
        if (playerGlobal == null)
            playerGlobal = PlayerGlobalHandler.GlobalHandler != null
                ? PlayerGlobalHandler.GlobalHandler
                : FindObjectOfType<PlayerGlobalHandler>();

        if (playerGlobal == null)
            Debug.LogWarning("LooksManager: No PlayerGlobalHandler found in scene.");

        // Ensure stats exists (since you can't change other scripts)
        if (playerGlobal != null && playerGlobal.stats == null)
            playerGlobal.stats = new StatBlock();
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
        if (playerGlobal == null || playerGlobal.stats == null) return;

        // Use threshold to avoid float weirdness (esp if slider is 0..1)
        if (gymSlider.value >= gymSlider.maxValue - 0.0001f)
        {
            gymSlider.value = gymSlider.minValue;

            // Increment GLOBAL looks
            playerGlobal.stats.looks = Mathf.Min(playerGlobal.stats.looks + 1, maxLooks);

            RefreshIndicators();

            if (playerGlobal.stats.looks >= maxLooks)
                TriggerWin();
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
        if (playerGlobal == null || playerGlobal.stats == null) return;

        int looks = playerGlobal.stats.looks;

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
            if (playerGlobal == null || playerGlobal.stats == null)
                return 0;

            return playerGlobal.stats.looks;
        }
        set
        {
            if (playerGlobal == null || playerGlobal.stats == null)
                return;

            playerGlobal.stats.looks = Mathf.Clamp(value, 0, maxLooks);
        }
    }
}

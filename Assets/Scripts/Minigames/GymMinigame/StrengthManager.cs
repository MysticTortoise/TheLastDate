using UnityEngine;
using UnityEngine.UI;

public class StrengthManager : MonoBehaviour
{
    [Header("Gym Slider")]
    public string sliderTag = "gymSlide";
    public Slider gymSlider;

    [Header("Strength")]
    public int strength = 0;
    public int maxStrength = 5;

    [Header("Strength Indicators")]
    public GameObject strengthIndicator1;
    public GameObject strengthIndicator2;
    public GameObject strengthIndicator3;
    public GameObject strengthIndicator4;
    public GameObject strengthIndicator5;

    [Header("Win State")]
    public GameObject win;
    public bool HasWon { get; private set; }

    void Awake()
    {
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

        // Use threshold to avoid float weirdness (esp if slider is 0..1)
        if (gymSlider.value >= gymSlider.maxValue - 0.0001f)
        {
            gymSlider.value = gymSlider.minValue;

            strength = Mathf.Min(strength + 1, maxStrength);

            RefreshIndicators();

            if (strength >= maxStrength)
                TriggerWin();
        }
    }

    void TriggerWin()
    {
        HasWon = true;

        if (win != null)
            win.SetActive(true);

        Debug.Log("WIN CONDITION REACHED (strength >= 5)");
    }

    void RefreshIndicators()
    {
        // Turn all OFF first (important)
        if (strengthIndicator1 != null) strengthIndicator1.SetActive(false);
        if (strengthIndicator2 != null) strengthIndicator2.SetActive(false);
        if (strengthIndicator3 != null) strengthIndicator3.SetActive(false);
        if (strengthIndicator4 != null) strengthIndicator4.SetActive(false);
        if (strengthIndicator5 != null) strengthIndicator5.SetActive(false);

        // Turn ON based on strength
        if (strength >= 1 && strengthIndicator1 != null) strengthIndicator1.SetActive(true);
        if (strength >= 2 && strengthIndicator2 != null) strengthIndicator2.SetActive(true);
        if (strength >= 3 && strengthIndicator3 != null) strengthIndicator3.SetActive(true);
        if (strength >= 4 && strengthIndicator4 != null) strengthIndicator4.SetActive(true);
        if (strength >= 5 && strengthIndicator5 != null) strengthIndicator5.SetActive(true);
    }
}

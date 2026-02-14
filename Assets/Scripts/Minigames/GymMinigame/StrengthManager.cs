using UnityEngine;
using UnityEngine.UI;

public class StrengthManager : MonoBehaviour
{
    [Header("Gym Slider")]
    public string sliderTag = "gymSlide";
    public Slider gymSlider;

    [Header("Strength")]
    public int strength = 0;

    [Header("Strength Indicators")]
    public GameObject strengthIndicator1;
    public GameObject strengthIndicator2;
    public GameObject strengthIndicator3;
    public GameObject strengthIndicator4;
    public GameObject strengthIndicator5;

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
        RefreshIndicators();
    }

    void Update()
    {
        if (gymSlider == null) return;

        // If slider reaches max, level up strength and reset slider
        if (gymSlider.value >= gymSlider.maxValue)
        {
            gymSlider.value = gymSlider.minValue;
            strength++;

            RefreshIndicators();
        }
    }

    void RefreshIndicators()
    {
        if (strength > 0){
            strengthIndicator1.SetActive(true);
        }

        if (strength > 1){
            strengthIndicator2.SetActive(true);
        }

         if (strength > 2){
            strengthIndicator3.SetActive(true);
        }

        if (strength > 3){
            strengthIndicator4.SetActive(true);
        }

        if (strength > 4){
            strengthIndicator5.SetActive(true);
        }
    }
}

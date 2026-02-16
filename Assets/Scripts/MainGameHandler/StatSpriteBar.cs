using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIImageBar : MonoBehaviour
{
    public enum StatType { Empathy, Smarts, Rizz, Looks }

    [Header("Which stat should this bar display?")]
    public StatType statToDisplay = StatType.Empathy;

    [Header("UI Images (index 0 = lowest, last = highest)")]
    public List<Image> images = new List<Image>();

    [Header("Colors")]
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.white;

    [Header("Stat Range")]
    public int minStat = -5;
    public int maxStat = 5;

    [Header("Debug")]
    public bool logIfMissing = false;

    private int _lastIndex = int.MinValue;

    private void Awake()
    {
        images.RemoveAll(i => i == null);

        EnsureStatsExist();
        ForceRefresh();
    }

    private void OnEnable()
    {
        EnsureStatsExist();
        ForceRefresh();
    }

    private void Update()
    {
        EnsureStatsExist();
        if (images.Count == 0) return;

        int statValue = GetSelectedStatValue();

        // Map min..max -> 0..(max-min) then clamp to image count
        int desiredIndex = statValue - minStat;
        desiredIndex = Mathf.Clamp(desiredIndex, 0, images.Count - 1);

        if (desiredIndex == _lastIndex) return;
        _lastIndex = desiredIndex;

        for (int i = 0; i < images.Count; i++)
            images[i].color = (i == desiredIndex) ? activeColor : inactiveColor;
    }

    private void EnsureStatsExist()
    {
        // If your GlobalHandler is created elsewhere (like a singleton), we can only guard here.
        if (PlayerGlobalHandler.GlobalHandler == null)
        {
            if (logIfMissing)
                Debug.LogWarning($"{name}: PlayerGlobalHandler.GlobalHandler was null (can't create it here).");
            return;
        }

        if (PlayerGlobalHandler.GlobalHandler.stats == null)
        {
            if (logIfMissing)
                Debug.LogWarning($"{name}: GlobalHandler.stats was null. Creating new StatBlock().");

            PlayerGlobalHandler.GlobalHandler.stats = new StatBlock();
        }
    }

    private void ForceRefresh()
    {
        _lastIndex = int.MinValue; // forces recolor next Update
    }

    private int GetSelectedStatValue()
    {
        // If GlobalHandler is still null, avoid null ref + pick a safe default
        if (PlayerGlobalHandler.GlobalHandler == null || PlayerGlobalHandler.GlobalHandler.stats == null)
            return 0;

        switch (statToDisplay)
        {
            case StatType.Empathy: return PlayerGlobalHandler.GlobalHandler.stats.empathy;
            case StatType.Smarts:  return PlayerGlobalHandler.GlobalHandler.stats.smarts;
            case StatType.Rizz:    return PlayerGlobalHandler.GlobalHandler.stats.rizz;   // <- new path
            case StatType.Looks:   return PlayerGlobalHandler.GlobalHandler.stats.looks;
            default:               return 0;
        }
    }
}

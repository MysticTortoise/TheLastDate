using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIImageBar : MonoBehaviour
{ 
    public enum StatType { Empathy, Smarts, Rizz, Looks }

    [Header("References")]
    public PlayerGlobalHandler playerGlobal;

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
    if (playerGlobal == null)
        playerGlobal = PlayerGlobalHandler.GlobalHandler;

    if (playerGlobal == null)
        playerGlobal = FindAnyObjectByType<PlayerGlobalHandler>();

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
        if (playerGlobal == null)
        {
            if (logIfMissing) Debug.LogWarning($"{name}: No PlayerGlobalHandler found.");
            return;
        }

        EnsureStatsExist();

        if (images.Count == 0) return;

        int statValue = GetSelectedStatValue();

        // Map -5..5 -> 0..10 (then clamp to your image count)
        int desiredIndex = statValue - minStat;
        desiredIndex = Mathf.Clamp(desiredIndex, 0, images.Count - 1);

        if (desiredIndex == _lastIndex) return;
        _lastIndex = desiredIndex;

        for (int i = 0; i < images.Count; i++)
            images[i].color = (i == desiredIndex) ? activeColor : inactiveColor;
    }

    private void EnsureStatsExist()
    {
        if (playerGlobal.stats == null)
        {
            if (logIfMissing) Debug.LogWarning($"{name}: playerGlobal.stats was null. Creating new StatBlock().");
            playerGlobal.stats = new StatBlock();
        }
    }

    private void ForceRefresh()
    {
        _lastIndex = int.MinValue; // forces recolor next Update
    }

    private int GetSelectedStatValue()
    {
        switch (statToDisplay)
        {
            case StatType.Empathy: return playerGlobal.stats.empathy;
            case StatType.Smarts:  return playerGlobal.stats.smarts;
            case StatType.Rizz:    return playerGlobal.stats.rizz;
            case StatType.Looks:   return playerGlobal.stats.looks;
            default:               return 0;
        }
    }
}

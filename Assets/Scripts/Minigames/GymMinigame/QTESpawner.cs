using System.Collections.Generic;
using UnityEngine;

public class QTESpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject qtePrefab; // must be your "QTE" prefab

    [Header("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawn Rate")]
    public float baseInterval = 1.25f;         // seconds at strength 0
    public float intervalReductionPerStrength = 0.08f; // faster each strength
    public float minInterval = 0.35f;

    [Header("References")]
    public StrengthManager strengthManager;

    private float _nextSpawnTime;

    void Start()
    {
        _nextSpawnTime = Time.time + GetCurrentInterval();
    }

    void Update()
    {
        if (qtePrefab == null || spawnPoints.Count == 0) return;

        if (Time.time >= _nextSpawnTime)
        {
            SpawnOne();
            _nextSpawnTime = Time.time + GetCurrentInterval();
        }
    }

    float GetCurrentInterval()
    {
        int strength = (strengthManager != null) ? strengthManager.strength : 0;

        float interval = baseInterval - (strength * intervalReductionPerStrength);
        return Mathf.Max(minInterval, interval);
    }

    void SpawnOne()
    {
        int idx = Random.Range(0, spawnPoints.Count);
        Transform sp = spawnPoints[idx];
        if (sp == null) return;

        Instantiate(qtePrefab, sp.position, sp.rotation);
    }
}

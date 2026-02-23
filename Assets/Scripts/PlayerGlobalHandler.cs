
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGlobalHandler : MonoBehaviour
{
    public StatBlock stats = new StatBlock
    {
        empathy = 0,
        charm = 0,
        looks = 0,
        money = 0,
        rizz = 0,
         smarts = 0,
    };
    private TimerHandler timerHandler;
    [NonSerialized] public List<ItemDefinition> heldItems = new();

    private bool hasStarted;

    private void OnEnable()
    {
        if (GlobalHandler != null)
        {
            Destroy(this);
            return;
        }
        GlobalHandler = this;
        timerHandler = GetComponentInChildren<TimerHandler>(true);
        DontDestroyOnLoad(gameObject);
        transform.SetParent(null);
        gameObject.AddComponent<MusicHandler>();
    }

    public void LetTheGamesBegin()
    {
        if (hasStarted)
            return;
        
        hasStarted = true;
        transform.Find("TimerUI").gameObject.SetActive(true);
    }
    
    public static void LoadIntoMainGame(GameObject scene)
    {
        MainPlayerHandler.ToLoadScene = scene;
        SceneManager.LoadScene("MainGameScene");
    }

    public void AddStats(StatBlock statsAdded)
    {
        stats.money += statsAdded.money;
        stats.empathy = Mathf.Clamp(stats.empathy + statsAdded.empathy, -5, 5);
        stats.smarts = Mathf.Clamp(stats.smarts + statsAdded.smarts, -5, 5);
        stats.rizz = Mathf.Clamp(stats.rizz + statsAdded.rizz, -5, 5);
        stats.looks = Mathf.Clamp(stats.looks + statsAdded.looks, -5, 5);
        stats.charm += statsAdded.charm;
    }

    public static PlayerGlobalHandler GlobalHandler;
}


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGlobalHandler : MonoBehaviour
{
    public StatBlock stats = new StatBlock
    {
        empathy = -5,
        charm = 0,
        looks = -5,
        money = -5,
        rizz = -5,
         smarts = -5,
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
        stats.empathy += statsAdded.empathy;
        stats.smarts += statsAdded.smarts;
        stats.rizz += statsAdded.rizz;
        stats.looks += statsAdded.looks;
        stats.charm += statsAdded.charm;
    }

    public static PlayerGlobalHandler GlobalHandler;
}

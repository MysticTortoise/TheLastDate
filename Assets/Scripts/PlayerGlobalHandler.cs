
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGlobalHandler : MonoBehaviour
{
    public StatBlock stats = new();
    private TimerHandler timerHandler;

    private bool hasStarted;


    private void OnEnable()
    {
        if (GlobalHandler != null)
        {
            Destroy(gameObject);
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
    }

    public static PlayerGlobalHandler GlobalHandler;
}

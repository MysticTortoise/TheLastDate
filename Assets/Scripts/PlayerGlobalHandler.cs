
using System;
using UnityEngine;

public class PlayerGlobalHandler : MonoBehaviour
{
    public StatBlock stats;
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

    public static PlayerGlobalHandler GlobalHandler;
}

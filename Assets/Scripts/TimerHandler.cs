
using System;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    private static readonly int RedFlashAnim = Animator.StringToHash("RedFlash");
    private const int StartingTimeMinutes = 10;
    
    public float timeLeft { private set; get; } = StartingTimeMinutes * 60;
    private int lastMinute = StartingTimeMinutes;

    private TextMeshProUGUI textUI;
    private Animator animator;

    private void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        
        TimeSpan span = TimeSpan.FromSeconds(timeLeft);
        string timeString = string.Format("{0:00}:{1:00}.{2:00}", span.Minutes, span.Seconds, span.Milliseconds / 10);
        textUI.text = timeString;

        if (span.Minutes < lastMinute)
        {
            lastMinute = span.Minutes;
            animator.SetTrigger(RedFlashAnim);
        }
    }
}

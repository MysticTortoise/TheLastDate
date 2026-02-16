
using System;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    private void Start()
    {
        var timer = FindAnyObjectByType<TimerHandler>();
        if (timer != null)
        {
            Destroy(timer.gameObject);
        }
    }
}

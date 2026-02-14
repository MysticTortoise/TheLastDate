
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentButton : MonoBehaviour
{
    public Collider2D hoverCollider { private set; get; }

    public static readonly HashSet<EnvironmentButton> buttonList = new();
    
    private bool IsHovered()
    {
        return hoverCollider.OverlapPoint(MainPlayerHandler.PlayerHandler.ProjectMouseToWorld());
    }

    private void Start()
    {
        hoverCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        buttonList.Add(this);
    }

    private void OnDisable()
    {
        buttonList.Remove(this);
    }

    public virtual void Click()
    {
        Debug.Log("CLICK");
    }
}

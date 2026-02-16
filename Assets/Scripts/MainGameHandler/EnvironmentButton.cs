
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentButton : MonoBehaviour
{
    public Collider2D hoverCollider { private set; get; }

    [SerializeField] private bool UseHoverSprite;
    private SpriteRenderer spriteRenderer;

    public static readonly HashSet<EnvironmentButton> buttonList = new();
    
    protected bool IsHovered()
    {
        if (!MainPlayerHandler.PlayerHandler.CanInteract())
            return false;
        
        return hoverCollider.OverlapPoint(MainPlayerHandler.PlayerHandler.ProjectMouseToWorld());
    }

    protected void Start()
    {
        if (UseHoverSprite)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        hoverCollider = GetComponent<Collider2D>();
    }

    protected void Update()
    {
        if(UseHoverSprite)
            spriteRenderer.enabled = IsHovered();
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

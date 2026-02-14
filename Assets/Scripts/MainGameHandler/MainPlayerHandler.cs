using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatBlock
{
    public float money = 0;
    public int empathy = 0;
    public int smarts = 0;
    public int rizz = 0;
    public int looks = 0;
}

public class MainPlayerHandler : MonoBehaviour
{
    private Camera playerViewCamera;

    public static MainPlayerHandler PlayerHandler;
    
    public StatBlock playerStats { private set; get; }

    public Vector2 ProjectMouseToWorld()
    {
        return playerViewCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private void Interact()
    {
        Vector2 mousePos = ProjectMouseToWorld();
        foreach (EnvironmentButton button in EnvironmentButton.buttonList
                     .Where(button => button.hoverCollider.OverlapPoint(mousePos)))
        {
            button.Click();
        }
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
            Interact();
    }
    
    void Start()
    {
        playerViewCamera = GetComponent<Camera>();
        PlayerHandler = this;
    }
}

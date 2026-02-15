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
    private static readonly int FadeOutTransitionAnim = Animator.StringToHash("FadeOutTransition");

    public StatBlock playerStats { private set; get; } = new StatBlock
    {
        money = 200,
    };

    private GameObject currentScene;
    private GameObject nextScene;
    private Animator animator;

    public static GameObject ToLoadScene;

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
            return;
        }
    }

    public bool CanInteract()
    {
        if (DialogueManager.dialogueManager.blockInput)
            return false;

        if (nextScene != null)
            return false;
        
        return true;
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.started && CanInteract())
            Interact();
    }

    public void DoSceneTransition(GameObject newScene)
    {
        nextScene = newScene;
        animator.SetTrigger(FadeOutTransitionAnim);
    }

    public void LoadNewScene()
    {
        if(currentScene)
            Destroy(currentScene);
        currentScene = Instantiate(nextScene);
        nextScene = null;
    }
    
    void Start()
    {
        playerViewCamera = GetComponent<Camera>();
        PlayerHandler = this;
        animator = GetComponent<Animator>();

        if (ToLoadScene != null)
        {
            nextScene = ToLoadScene;
            LoadNewScene();
        }
    }
}

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainPlayerHandler : MonoBehaviour
{
    private RectTransform playerViewRect;
    private Camera playerViewCamera;

    public static MainPlayerHandler PlayerHandler;

    public Vector2 ProjectMouseToWorld()
    {
        //Debug.Log(Mouse.current.position.ReadValue());
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            playerViewRect,
            Mouse.current.position.ReadValue(),
            null,
            out Vector2 localMousePosition
        );
        var corners = new Vector3[4];
        playerViewRect.GetWorldCorners(corners);
        float width = (corners[2].x - corners[0].x);
        float height = (corners[2].y - corners[0].y);


        Vector2 cursorPosition = playerViewCamera.transform.position;
        cursorPosition.y += (localMousePosition.y / height) * playerViewCamera.orthographicSize * 2;
        cursorPosition.x += (localMousePosition.x / width) * playerViewCamera.orthographicSize * 2 *
                            playerViewCamera.aspect;

        return cursorPosition;
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
        playerViewRect = GameTag.GetFirstObjectWith("PlayerCameraImage").GetComponent<RectTransform>();
        playerViewCamera = GetComponent<Camera>();
        PlayerHandler = this;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrid : MonoBehaviour
{
    public int currentGrid;

    [Header("Limits")]
    public int minGrid = 0;
    public int maxGrid = 5;

    [Header("Movement")]
    public float gridSpacing = 2f;

    [Header("Input")]
    public InputActionReference moveAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
        moveAction.action.performed += Move;
    }

    private void OnDisable()
    {
        moveAction.action.performed -= Move;
        moveAction.action.Disable();
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();

        if (input.x > 0.5f)
            TryMove(1);
        else if (input.x < -0.5f)
            TryMove(-1);
    }

    void TryMove(int dir)
    {
        int newGrid = Mathf.Clamp(currentGrid + dir, minGrid, maxGrid);

        if (newGrid == currentGrid) return;

        currentGrid = newGrid;

        // snap position
        transform.position = new Vector3(currentGrid * gridSpacing, transform.position.y, 0);
    }
}

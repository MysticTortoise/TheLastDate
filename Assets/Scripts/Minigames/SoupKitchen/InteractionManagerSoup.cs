using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManagerSoup : MonoBehaviour
{
    public PlayerGrid player;
    public Transform holdPoint;
    public InputActionReference interactAction;

    private Food heldFood;

    private void OnEnable()
    {
        interactAction.action.Enable();
        interactAction.action.performed += Interact;
    }

    private void OnDisable()
    {
        interactAction.action.performed -= Interact;
        interactAction.action.Disable();
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        Shelf shelf = FindShelfAtGrid(player.currentGrid);

        if (shelf == null) return;

        if (heldFood == null)
        {
            // Try pick up
            Food food = FindFoodAtGrid(player.currentGrid);

            if (food != null && !food.isBeingPickedUp)
            {
                heldFood = food;
                heldFood.isBeingPickedUp = true;
                heldFood.transform.SetParent(holdPoint);
                heldFood.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            // Try place
            Food existingFood = FindFoodAtGrid(player.currentGrid);

            if (existingFood == null || existingFood == heldFood)
            {
                heldFood.isBeingPickedUp = false;
                heldFood.currentGrid = player.currentGrid;
                heldFood.transform.SetParent(null);
                heldFood.transform.position = shelf.transform.position;

                heldFood = null;
            }
        }
    }

    Shelf FindShelfAtGrid(int grid)
    {
        GameObject[] shelves = GameObject.FindGameObjectsWithTag("shelf");

        foreach (GameObject obj in shelves)
        {
            Shelf s = obj.GetComponent<Shelf>();
            if (s != null && s.currentGrid == grid)
                return s;
        }

        return null;
    }

    Food FindFoodAtGrid(int grid)
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");

        foreach (GameObject obj in foods)
        {
            Food f = obj.GetComponent<Food>();

            if (f != null && f.currentGrid == grid && !f.isBeingPickedUp)
                return f;
        }

        return null;
    }

    private void Update()
    {
        // Make held food follow grid while moving
        if (heldFood != null && heldFood.isBeingPickedUp)
        {
            heldFood.currentGrid = player.currentGrid;
        }
    }
}

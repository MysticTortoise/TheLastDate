using UnityEngine;

public class ShelfSlot : MonoBehaviour
{
    public Transform holdPoint;

    private GameObject currentFood;

    public bool HasFood()
    {
        if (currentFood != null) return true;

        if (holdPoint == null) return false;

        // Only count FOOD objects as occupying the shelf
        for (int i = 0; i < holdPoint.childCount; i++)
        {
            if (holdPoint.GetChild(i).CompareTag("food"))
                return true;
        }

        return false;
    }

    public void SetCurrentFood(GameObject food)
    {
        currentFood = food;
    }
}

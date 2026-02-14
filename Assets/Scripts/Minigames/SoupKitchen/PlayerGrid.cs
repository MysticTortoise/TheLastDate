using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    public int currentGrid;

    public int minGrid = 0;
    public int maxGrid = 5;

    public float gridSpacing = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            TryMove(-1);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            TryMove(1);
    }

    void TryMove(int dir)
    {
        int newGrid = Mathf.Clamp(currentGrid + dir, minGrid, maxGrid);
        if (newGrid == currentGrid) return;

        currentGrid = newGrid;
        transform.position = new Vector3(currentGrid * gridSpacing, transform.position.y, transform.position.z);
    }
}

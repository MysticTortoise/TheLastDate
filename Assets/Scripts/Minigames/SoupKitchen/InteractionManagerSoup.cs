using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    [Header("References")]
    public PlayerGrid player;
    public Transform holdPoint;

    [Header("Charm UI")]
    public Slider charmSlider;
    public int charmMax = 10;

    [Header("Empathy")]
    [SerializeField] private int empathy = 0; // starts at 0
    public int Empathy => empathy;
    public int empathyMin = -5;
    public int empathyMax = 5;

    [Header("Popups")]
    public GameObject happyPopupPrefab;
    public GameObject sadPopupPrefab;
    public Transform popupSpawnPoint;
    public Vector3 popupOffsetFromPlayer = new Vector3(0f, 1.2f, 0f);

    [Header("Trash")]
    public int trashGrid = 6; // pressing space while holding food here destroys it

    private int charm = 0;

    private GameObject heldInstance;
    private Food heldFood;

    void Start()
    {
        SetCharm(0);
        empathy = Mathf.Clamp(empathy, empathyMin, empathyMax);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            HandleSpace();

        // keep held food grid in sync
        if (heldFood != null)
            heldFood.currentGrid = player.currentGrid;
    }

    void HandleSpace()
    {
        int grid = player.currentGrid;

        // --- TRASH RULE ---
        // If holding food and on trash grid, destroy it instantly.
        if (heldInstance != null && grid == trashGrid)
        {
            Destroy(heldInstance);
            heldInstance = null;
            heldFood = null;
            return;
        }

        // NOT holding -> spawn from basket
        if (heldInstance == null)
        {
            Basket basket = FindBasketAtGrid(grid);
            if (basket == null || basket.foodPrefab == null) return;

            SpawnInHand(basket.foodPrefab, grid);
            return;
        }

        // HOLDING -> only serve if there is a customer at this grid AND a shelf exists AND shelf is empty
        Customer customer = FindCustomerAtGrid(grid);
        if (customer == null) return; // key change: cannot place unless customer exists

        Shelf shelf = FindShelfAtGrid(grid);
        if (shelf == null) return;

        // shelf must be empty (no food sitting there)
        if (FindFoodAtGrid(grid) != null) return;

        // Place + Serve (food is consumed either way)
        ServeCustomerAtShelf(customer, shelf, grid);
    }

    void SpawnInHand(GameObject prefab, int grid)
    {
        heldInstance = Instantiate(prefab, holdPoint.position, Quaternion.identity);
        heldInstance.transform.SetParent(holdPoint, true);
        heldInstance.transform.localPosition = Vector3.zero;

        heldFood = heldInstance.GetComponent<Food>();
        if (heldFood != null) heldFood.currentGrid = grid;
    }

    void ServeCustomerAtShelf(Customer customer, Shelf shelf, int grid)
    {
        // snap food to shelf visually (optional, but looks correct)
        heldInstance.transform.SetParent(null, true);
        heldInstance.transform.position = shelf.transform.position;

        if (heldFood != null)
            heldFood.currentGrid = grid;

        // score + popup
        bool liked = (heldFood != null && heldFood.foodID == customer.requestedFoodID);

        AddCharm(liked ? +1 : -3);
        SpawnPopup(liked);

        // remove customer and consume food
        Destroy(customer.gameObject);
        Destroy(heldInstance);

        heldInstance = null;
        heldFood = null;
    }

    void SpawnPopup(bool happy)
    {
        GameObject prefab = happy ? happyPopupPrefab : sadPopupPrefab;
        if (prefab == null) return;

        Vector3 pos = (popupSpawnPoint != null)
            ? popupSpawnPoint.position
            : player.transform.position + popupOffsetFromPlayer;

        Instantiate(prefab, pos, Quaternion.identity);
    }

    // -------- Charm + Empathy rules --------

    void SetCharm(int value)
    {
        charm = Mathf.Clamp(value, 0, charmMax);
        if (charmSlider != null)
        {
            charmSlider.maxValue = charmMax;
            charmSlider.value = charm;
        }
    }

    public void AddCharm(int delta)
    {
        int newCharm = charm + delta;
        if (newCharm < 0) newCharm = 0;

        // charm max -> empathy +1 (clamped), charm resets to 0
        if (newCharm >= charmMax)
        {
            empathy = Mathf.Clamp(empathy + 1, empathyMin, empathyMax);
            SetCharm(0);
            return;
        }

        SetCharm(newCharm);
    }

    // -------- Find helpers --------

    Basket FindBasketAtGrid(int grid)
    {
        GameObject[] baskets = GameObject.FindGameObjectsWithTag("basket");
        foreach (GameObject obj in baskets)
        {
            Basket b = obj.GetComponent<Basket>();
            if (b != null && b.currentGrid == grid) return b;
        }
        return null;
    }

    Shelf FindShelfAtGrid(int grid)
    {
        GameObject[] shelves = GameObject.FindGameObjectsWithTag("shelf");
        foreach (GameObject obj in shelves)
        {
            Shelf s = obj.GetComponent<Shelf>();
            if (s != null && s.currentGrid == grid) return s;
        }
        return null;
    }

    Food FindFoodAtGrid(int grid)
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
        foreach (GameObject obj in foods)
        {
            Food f = obj.GetComponent<Food>();
            if (f != null && f.currentGrid == grid) return f;
        }
        return null;
    }

    Customer FindCustomerAtGrid(int grid)
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("customer");
        foreach (GameObject obj in customers)
        {
            Customer c = obj.GetComponent<Customer>();
            if (c != null && c.currentGrid == grid) return c;
        }
        return null;
    }
}

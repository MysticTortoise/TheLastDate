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

        if (heldFood != null)
            heldFood.currentGrid = player.currentGrid;
    }

    void HandleSpace()
    {
        int grid = player.currentGrid;

        // Not holding -> spawn from basket
        if (heldInstance == null)
        {
            Basket basket = FindBasketAtGrid(grid);
            if (basket == null || basket.foodPrefab == null) return;

            SpawnInHand(basket.foodPrefab, grid);
            return;
        }

        // Holding -> place on shelf if empty
        Shelf shelf = FindShelfAtGrid(grid);
        if (shelf == null) return;

        if (FindFoodAtGrid(grid) != null) return; // shelf occupied

        PlaceOnShelfAndMaybeServe(shelf, grid);
    }

    void SpawnInHand(GameObject prefab, int grid)
    {
        heldInstance = Instantiate(prefab, holdPoint.position, Quaternion.identity);
        heldInstance.transform.SetParent(holdPoint, true);
        heldInstance.transform.localPosition = Vector3.zero;

        heldFood = heldInstance.GetComponent<Food>();
        if (heldFood != null) heldFood.currentGrid = grid;
    }

    void PlaceOnShelfAndMaybeServe(Shelf shelf, int grid)
    {
        heldInstance.transform.SetParent(null, true);
        heldInstance.transform.position = shelf.transform.position;

        if (heldFood != null)
            heldFood.currentGrid = grid;

        Customer customer = FindCustomerAtGrid(grid);

        if (customer != null && heldFood != null)
        {
            bool liked = (heldFood.foodID == customer.requestedFoodID);

            AddCharm(liked ? +1 : -3);
            SpawnPopup(liked);

            Destroy(customer.gameObject);
            Destroy(heldInstance);

            heldInstance = null;
            heldFood = null;
            return;
        }

        // No customer: food stays on shelf
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

        // clamp low end
        if (newCharm < 0) newCharm = 0;

        // if we hit or exceed max, we "level up" empathy and reset charm
        if (newCharm >= charmMax)
        {
            empathy = Mathf.Clamp(empathy + 1, empathyMin, empathyMax);
            SetCharm(0); // resets when maxed
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

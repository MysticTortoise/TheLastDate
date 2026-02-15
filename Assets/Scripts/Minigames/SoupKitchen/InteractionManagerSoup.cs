using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    [Header("References")]
    public PlayerGrid player;
    public Transform holdPoint;

    [Header("Global Stats")]
    public PlayerGlobalHandler playerGlobal;

    [Header("Charm UI")]
    public Slider charmSlider;
    public int charmMax = 10;

    [Header("Empathy (Global)")]
    public int empathyMin = -5;
    public int empathyMax = 5;

    // IMPORTANT: keep Empathy working for other scripts
    public int Empathy => empathy;

    // IMPORTANT: keep "empathy" accessible even if other scripts reference it
    public int empathy
    {
        get
        {
            if (playerGlobal == null || playerGlobal.stats == null) return 0;
            return playerGlobal.stats.empathy;
        }
        private set
        {
            if (playerGlobal == null || playerGlobal.stats == null) return;
            playerGlobal.stats.empathy = Mathf.Clamp(value, empathyMin, empathyMax);
        }
    }

    [Header("Empathy Text Popup")]
    public GameObject empathyTextPopupPrefab;   // prefab that contains a TMP text + a script to set it
    public Transform empathyTextSpawnPoint;     // optional (leave null to spawn above player)
    public Vector3 empathyTextOffset = new Vector3(0f, 1.6f, 0f);

    [Header("Popups (Happy/Sad)")]
    public GameObject happyPopupPrefab;
    public GameObject sadPopupPrefab;
    public Transform popupSpawnPoint;
    public Vector3 popupOffsetFromPlayer = new Vector3(0f, 1.2f, 0f);

    [Header("Indicator")]
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;

    [Header("Trash")]
    public int trashGrid = 6;

    private int charm = 0;

    private GameObject heldInstance;
    private Food heldFood;

void Awake()
{
    if (playerGlobal == null)
        playerGlobal = PlayerGlobalHandler.GlobalHandler;

    if (playerGlobal == null)
        playerGlobal = FindAnyObjectByType<PlayerGlobalHandler>(); // ✅ new API (fast)

    // Ensure stats exists (since you can’t edit other scripts)
    if (playerGlobal != null && playerGlobal.stats == null)
        playerGlobal.stats = new StatBlock();
}


    void Start()
    {
        SetCharm(2);
        empathy = Mathf.Clamp(empathy, empathyMin, empathyMax);

        RefreshEmpathyIndicators();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            HandleSpace();

        // If holding food, keep its grid synced
        if (heldFood != null)
            heldFood.currentGrid = player.currentGrid;
    }

    void HandleSpace()
    {
        int grid = player.currentGrid;

        // TRASH: destroy held food instantly
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

        // HOLDING -> only serve if customer exists, shelf exists, shelf empty
        Customer customer = FindCustomerAtGrid(grid);
        if (customer == null) return;

        Shelf shelf = FindShelfAtGrid(grid);
        if (shelf == null) return;

        if (FindFoodAtGrid(grid) != null) return;

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
        // Snap food to shelf for visuals
        heldInstance.transform.SetParent(null, true);
        heldInstance.transform.position = shelf.transform.position;

        if (heldFood != null)
            heldFood.currentGrid = grid;

        bool liked = (heldFood != null && heldFood.foodID == customer.requestedFoodID);

        if (liked)
        {
            AddCharm(+1);
            SpawnHappySadPopup(true);
        }
        else
        {
            HandleNegativeOutcome(); // includes -3 charm, and empathy -1 if charm was empty
            SpawnHappySadPopup(false);
        }

        customer.MarkServed();
        customer.ServeAndLeave();

        Destroy(heldInstance);
        heldInstance = null;
        heldFood = null;
    }

    // Called by Customer when countdown ends
    public void OnCustomerTimedOut(Customer customer)
    {
        HandleNegativeOutcome();
        SpawnHappySadPopup(false);
    }

    // ---------- Charm + Empathy rules ----------

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

        // If we hit max: empathy +1, charm resets to 0
        if (newCharm >= charmMax)
        {
            ChangeEmpathy(+1);  // IMPORTANT: also spawns +1 text popup
            SetCharm(0);
            return;
        }

        SetCharm(newCharm);
    }

    // If charm is empty (0) and a negative happens, empathy -1
    void HandleNegativeOutcome()
    {
        if (charm <= 0)
            ChangeEmpathy(-1);

        AddCharm(-3); // charm clamps at 0
    }

    void ChangeEmpathy(int delta)
    {
        int before = empathy;
        empathy = empathy + delta; // uses property setter (clamps)

        int actualDelta = empathy - before;
        if (actualDelta == 0) return; // clamped, no visible change

        SpawnEmpathyText(actualDelta);
        RefreshEmpathyIndicators();
    }

    void SpawnEmpathyText(int delta)
    {
        if (empathyTextPopupPrefab == null) return;

        Vector3 pos =
            (empathyTextSpawnPoint != null)
                ? empathyTextSpawnPoint.position
                : player.transform.position + empathyTextOffset;

        GameObject go = Instantiate(empathyTextPopupPrefab, pos, Quaternion.identity);

        // EXPECTS: your prefab has this script attached.
        // If your script name differs, change this type to your script class name.
        TextPopupSetTMP setter = go.GetComponent<TextPopupSetTMP>();
        if (setter != null)
        {
            string sign = delta > 0 ? "+" : "";
            setter.SetText($"{sign}{delta} Empathy");
        }
    }

    void RefreshEmpathyIndicators()
    {
        if (One != null) One.SetActive(false);
        if (Two != null) Two.SetActive(false);
        if (Three != null) Three.SetActive(false);
        if (Four != null) Four.SetActive(false);
        if (Five != null) Five.SetActive(false);

        int e = empathy;
        if (e >= 1 && One != null) One.SetActive(true);
        if (e >= 2 && Two != null) Two.SetActive(true);
        if (e >= 3 && Three != null) Three.SetActive(true);
        if (e >= 4 && Four != null) Four.SetActive(true);
        if (e >= 5 && Five != null) Five.SetActive(true);
    }

    void SpawnHappySadPopup(bool happy)
    {
        GameObject prefab = happy ? happyPopupPrefab : sadPopupPrefab;
        if (prefab == null) return;

        Vector3 pos = (popupSpawnPoint != null)
            ? popupSpawnPoint.position
            : player.transform.position + popupOffsetFromPlayer;

        Instantiate(prefab, pos, Quaternion.identity);
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

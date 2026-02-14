using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Prefabs (3 varieties)")]
    public GameObject[] customerPrefabs; // size 3 recommended

    [Header("Spawn Timing")]
    public float spawnCheckInterval = 1.0f;   // how often we check if we should spawn
    public float spawnChancePerCheck = 0.6f;  // 0..1 chance to spawn when under cap

    [Header("Spawn Positions")]
    public float ySpawn = 0f;
    public float zSpawn = 0f;

    // fixed grid X positions
    private readonly float[] gridXs = new float[] { 0f, 1.5f, 3f };

    [Header("Reference")]
    public InteractionManager manager; // to read empathy

    private float timer;

    private void Update()
    {
        if (manager == null) return;
        if (customerPrefabs == null || customerPrefabs.Length == 0) return;

        timer += Time.deltaTime;
        if (timer < spawnCheckInterval) return;
        timer = 0f;

        int maxCustomers = GetMaxCustomers(manager.Empathy);
        int currentCustomers = CountCustomers();

        if (currentCustomers >= maxCustomers) return;

        // roll chance
        if (Random.value > spawnChancePerCheck) return;

        TrySpawnOne();
    }

    int GetMaxCustomers(int empathy)
    {
        if (empathy < 0) return 1;
        if (empathy <= 2) return 2;
        return 3;
    }

    void TrySpawnOne()
    {
        // pick a grid that is empty
        int[] grids = new int[] { 0, 1, 2 };
        Shuffle(grids);

        for (int i = 0; i < grids.Length; i++)
        {
            int grid = grids[i];
            if (IsGridOccupiedByCustomer(grid)) continue;

            // spawn random variety
            GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];

            Vector3 pos = new Vector3(gridXs[grid], ySpawn, zSpawn);
            Instantiate(prefab, pos, Quaternion.identity);

            return;
        }

        // all grids occupied, do nothing
    }

    int CountCustomers()
    {
        return GameObject.FindGameObjectsWithTag("customer").Length;
    }

    bool IsGridOccupiedByCustomer(int grid)
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("customer");
        foreach (GameObject obj in customers)
        {
            Customer c = obj.GetComponent<Customer>();
            if (c != null && c.currentGrid == grid) return true;
        }
        return false;
    }

    void Shuffle(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            int j = Random.Range(i, arr.Length);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
    }
}

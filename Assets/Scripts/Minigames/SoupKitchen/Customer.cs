using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Customer : MonoBehaviour
{
    [Header("Grid")]
    public int currentGrid;

    [Header("Requested Food")]
    public int requestedFoodID; // 0..2

    [Header("Spawn Animation")]
    public float spawnAnimDuration = 0.35f;
    public float spawnMoveDownAmount = 0.25f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetGridFromPosition();
        StartCoroutine(SpawnAnim());
    }

    void SetGridFromPosition()
    {
        // Snap-to mapping based on x positions you described
        float x = transform.position.x;

        // nearest of {0, 1.5, 3}
        float d0 = Mathf.Abs(x - 0f);
        float d1 = Mathf.Abs(x - 1.5f);
        float d2 = Mathf.Abs(x - 3f);

        if (d0 <= d1 && d0 <= d2) currentGrid = 0;
        else if (d1 <= d0 && d1 <= d2) currentGrid = 1;
        else currentGrid = 2;
    }

    System.Collections.IEnumerator SpawnAnim()
    {
        // Start slightly ABOVE then move DOWN into place (your request)
        Vector3 endPos = transform.position;
        Vector3 startPos = endPos + new Vector3(0f, spawnMoveDownAmount, 0f);
        transform.position = startPos;

        // start transparent
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        float t = 0f;
        while (t < spawnAnimDuration)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / Mathf.Max(0.0001f, spawnAnimDuration));

            // move down into position
            transform.position = Vector3.Lerp(startPos, endPos, u);

            // fade in
            Color cc = sr.color;
            cc.a = Mathf.Lerp(0f, 1f, u);
            sr.color = cc;

            yield return null;
        }

        transform.position = endPos;
        Color final = sr.color;
        final.a = 1f;
        sr.color = final;
    }
}

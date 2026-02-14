using UnityEngine;
using UnityEngine.InputSystem;

public class SoupPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public Transform playerHoldPoint;

    private Rigidbody2D rb;

    private Transform shelfHoldPointInRange;   // where to place food (on the shelf)
    private GameObject foodInRange;            // food we can pick up
    private GameObject heldFood;               // food currently held (null if none)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // movement (A/D or arrows)
        float x = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x = 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                Interact();
        }

        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
    }

    private void Interact()
    {
        // PICK UP
        if (heldFood == null && foodInRange != null)
        {
            heldFood = foodInRange;

            heldFood.transform.SetParent(playerHoldPoint);
            heldFood.transform.localPosition = Vector3.zero;
            heldFood.transform.localRotation = Quaternion.identity;

            // prevent it from constantly re-triggering while held
            var col = heldFood.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            return;
        }

        // PLACE
        if (heldFood != null && shelfHoldPointInRange != null)
        {
            // if shelf already has a FOOD child, don't place
            for (int i = 0; i < shelfHoldPointInRange.childCount; i++)
            {
                if (shelfHoldPointInRange.GetChild(i).CompareTag("food"))
                    return;
            }

            heldFood.transform.SetParent(shelfHoldPointInRange);
            heldFood.transform.localPosition = Vector3.zero;
            heldFood.transform.localRotation = Quaternion.identity;

            var col = heldFood.GetComponent<Collider2D>();
            if (col != null) col.enabled = true;

            heldFood = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // FOOD in range
        if (other.CompareTag("food"))
        {
            // only allow pickup if we're not already holding
            if (heldFood == null)
                foodInRange = other.gameObject;
        }

        // SHELF in range
        if (other.CompareTag("shelf"))  // change to "self" if that's your tag
        {
            // find ShelfHoldPoint (child transform)
            shelfHoldPointInRange = other.transform.Find("ShelfHoldPoint");
            if (shelfHoldPointInRange == null)
            {
                // fallback: if the shelf hold point is elsewhere, just use the shelf transform itself
                shelfHoldPointInRange = other.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("food") && foodInRange == other.gameObject)
            foodInRange = null;

        if (other.CompareTag("shelf")) // change to "self" if that's your tag
            shelfHoldPointInRange = null;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Slots (size 9)")]
    [Tooltip("Assign your 9 UI Image components here (slot 0 = first item, etc.)")]
    public List<Image> slots = new List<Image>(9);

    [Header("Behavior")]
    [Tooltip("If true, leftover slots are hidden when there are fewer than 9 items.")]
    public bool hideUnusedSlots = true;

    [Tooltip("If false, refresh every frame. If true, refresh only when something changes.")]
    public bool refreshOnlyWhenChanged = true;

    // Change detection
    private int _lastCount = -1;
    private readonly int[] _lastSpriteIds = new int[9];

    private void Awake()
    {
        // Initialize last sprite ids
        for (int i = 0; i < _lastSpriteIds.Length; i++)
            _lastSpriteIds[i] = int.MinValue;
    }

    private void OnEnable()
    {
        RefreshUI(force: true);
    }

    private void Update()
    {
        if (!refreshOnlyWhenChanged)
        {
            RefreshUI(force: true);
            return;
        }

        RefreshUI(force: false);
    }

    public void RefreshUI(bool force = true)
    {
        // Ensure we have a global handler
        var gh = PlayerGlobalHandler.GlobalHandler;
        if (gh == null)
        {
            // Optional fallback: try find one (safe if your GlobalHandler persists)
            gh = FindObjectOfType<PlayerGlobalHandler>();
            if (gh != null) PlayerGlobalHandler.GlobalHandler = gh;
        }

        if (gh == null)
            return;

        List<ItemDefinition> items = gh.heldItems;

        int count = items != null ? items.Count : 0;

        // If count changed, we should refresh
        bool changed = force || (count != _lastCount);

        // Also detect sprite changes (in case list contents changed but count didn't)
        int maxSlots = Mathf.Min(slots.Count, 9);
        int checkCount = Mathf.Min(count, maxSlots);

        for (int i = 0; i < checkCount; i++)
        {
            var sprite = items[i] != null ? items[i].Image : null;
            int id = sprite != null ? sprite.GetInstanceID() : 0;

            if (id != _lastSpriteIds[i])
            {
                changed = true;
                break;
            }
        }

        if (!changed)
            return;

        _lastCount = count;

        // Apply slot sprites
        for (int i = 0; i < maxSlots; i++)
        {
            var img = slots[i];
            if (img == null) continue;

            if (items != null && i < items.Count && items[i] != null && items[i].Image != null)
            {
                img.sprite = items[i].Image;
                img.enabled = true;

                // track sprite id
                _lastSpriteIds[i] = img.sprite.GetInstanceID();
            }
            else
            {
                img.sprite = null;
                _lastSpriteIds[i] = 0;

                if (hideUnusedSlots)
                    img.enabled = false;
                else
                    img.enabled = true; // shows empty Image (sprite null) if you prefer that look
            }
        }

        // If you accidentally assigned more than 9 in the inspector, we still support it,
        // but only track/update the first 9 by design.
        for (int i = maxSlots; i < slots.Count; i++)
        {
            if (slots[i] == null) continue;
            if (hideUnusedSlots) slots[i].enabled = false;
            else slots[i].sprite = null;
        }
    }
}

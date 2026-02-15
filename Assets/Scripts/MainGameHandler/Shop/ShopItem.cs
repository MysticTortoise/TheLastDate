
using System;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private Collider2D hitbox;

    public ItemDefinition ItemDef;

    private void Start()
    {
        hitbox = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector2 mousePos = MainPlayerHandler.PlayerHandler.ProjectMouseToWorld();
        if (hitbox.OverlapPoint(mousePos))
        {
            DialogueManager.dialogueManager.SetLiveMessage(ItemDef.GetShopText(), "Annoyed Teen");
        }
    }
}

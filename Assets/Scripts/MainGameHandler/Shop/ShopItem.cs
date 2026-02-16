
using System;
using UnityEngine;

public class ShopItem : EnvironmentButton
{
    public ItemDefinition ItemDef;

    private const string ShopkeepName = "Annoyed Teen";
    private bool wasHovering;

    protected new void Start()
    {
        if (PlayerGlobalHandler.GlobalHandler.heldItems.Contains(ItemDef))
        {
            Destroy(gameObject);
            return;
        }
        
        base.Start();
        GetComponent<SpriteRenderer>().sprite = ItemDef.Image;
    }

    private new void Update()
    {
        base.Update();
        bool currHovered = IsHovered();
        if (currHovered && !wasHovering)
        {
            DialogueManager.dialogueManager.SetLiveMessage(ItemDef.GetShopText(), ShopkeepName);
        }
        wasHovering = currHovered;
    }

    
    public override void Click()
    {
        if (PlayerGlobalHandler.GlobalHandler.stats.money >= ItemDef.Cost)
        {
            DialogueManager.dialogueManager.SetLiveMessage("Thanks for purchasing. I guess.", ShopkeepName);
            PlayerGlobalHandler.GlobalHandler.AddStats(new StatBlock
            {
                money = -ItemDef.Cost
            });
            PlayerGlobalHandler.GlobalHandler.heldItems.Add(ItemDef);
            Destroy(gameObject);
        }
        else
        {
            DialogueManager.dialogueManager.SetLiveMessage("Broke ass.", ShopkeepName);
        }
    }
}

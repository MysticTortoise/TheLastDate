
using UnityEngine;

[CreateAssetMenu(fileName = "GiveItem", menuName = "Dialogue/Actions/Give Item")]
public class DIalogueActionGiveItem : DialogueActionBase
{
    public ItemDefinition Item;

    public override void Trigger()
    {
        PlayerGlobalHandler.GlobalHandler.heldItems.Add(Item);
    }
}

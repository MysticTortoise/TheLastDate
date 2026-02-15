
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory Item")]
public class ItemDefinition : ScriptableObject
{
    public string Name;
    public string Description;
    public string ShopDescription;
    public float Cost;
    public Sprite Image;


    public string GetShopText()
    {
        return $"{Name} - {ShopDescription} {Cost:C2}";
    }
}

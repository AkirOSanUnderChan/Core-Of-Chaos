using UnityEngine;


[CreateAssetMenu(fileName = "New item object", menuName = "My/Inventory System/Items")]

public class Item : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public string itemType;
    public Color itemTypeColor;
    public string description;
    public bool stackable;
    public int maxStack;
    public bool canUse;
}

public sealed class ItemData
{
    public string UniqueName;
    public int StackCount;
}
using UnityEngine;


[CreateAssetMenu(fileName = "New item object", menuName = "My/Inventory System/Items")]

public class Item : ScriptableObject
{
    public string itemName;
    public string itemType;
    public Color itemTypeColor;
    public string description;
    public Sprite itemImage;
    public bool stackable;
    public int maxStack;
}

public sealed class ItemData
{
    public string UniqueName;
    public int StackCount;
}
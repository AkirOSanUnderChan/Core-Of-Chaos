using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    private bool isHovered;
    public int currentStack;

    public delegate void MouseEnterAction();
    public event MouseEnterAction onMouseEnterAction;

    public delegate void MouseExitAction();
    public event MouseExitAction onMouseExitAction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.instance.AddItem(item, currentStack);
            Destroy(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        isHovered = true;
        if (onMouseEnterAction != null)
        {
            onMouseEnterAction();
        }
    }

    private void OnMouseExit()
    {
        isHovered = false;
        if (onMouseExitAction != null)
        {
            onMouseExitAction();
        }
    }

    public bool IsHovered()
    {
        return isHovered;
    }
}

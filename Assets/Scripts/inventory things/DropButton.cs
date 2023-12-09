using UnityEngine;

public class DropButton : MonoBehaviour
{
    private int selectedItemIndex = -1;

    public void SetSelectedItemIndex(int index)
    {
        selectedItemIndex = index;
    }

    public void OnDropButtonClick()
    {
        if (selectedItemIndex >= 0 && selectedItemIndex < InventoryManager.instance.inventoryItems.Count)
        {
            InventoryManager.instance.RemoveItem(selectedItemIndex);
        }
    }

    public void OnUseButtonClick()
    {
        int myIndex = InventoryManager.instance._selectedCellIndex;
        if (myIndex >= 0 && myIndex < InventoryManager.instance.inventoryItems.Count)
        {
            InventoryManager.instance.UseItem(myIndex);
        }
    }

    public void OnEquipButtonClick()
    {
        int myIndex = InventoryManager.instance._selectedCellIndex;
        if (myIndex >= 0 && myIndex < InventoryManager.instance.inventoryItems.Count)
        {
            InventoryManager.instance.EquipWeaponItem(myIndex);
        }
    }
}
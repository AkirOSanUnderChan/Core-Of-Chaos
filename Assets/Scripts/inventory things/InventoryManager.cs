using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
using System.Xml.Serialization;

public class InventoryManager : MonoBehaviour
{
    public Action OnItemSelected;
    public Action OnItemDeselected;


    public GameObject descriptionPanel;
    public Image itemDescriptionImage;
    public TextMeshProUGUI itemDescriptionName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemDescriptionType;
    public bool toolTipPanelOn;

    [SerializeField]
    private PlayerCOntroller playerController;
    public PlayerWeaponChanger weaponChanger;
    public static InventoryManager instance;


    public List<ItemData> inventoryItems = new List<ItemData>();
    public Item weapon1Slot;
    public List<Item> allItems = new List<Item>();


    public GameObject equipButton;
    public GameObject unequipButton;

    [SerializeField]
    private Transform itemsParent;
    public ItemCell itemCellPrefab;
    public Button dropButton; // Public змінна для посилання на кнопку "Викинути предмет"
    private List<ItemCell> _itemCells = new();

    public int _selectedCellIndex = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        playerController = GetComponentInChildren<PlayerCOntroller>();
        weaponChanger = PlayerControllerSingleton.Instance.playerWeaponChanger;
    }


    public void Update()
    {
        if (toolTipPanelOn)
        {
            descriptionPanel.SetActive(true);
        }
        else
        {
            descriptionPanel.SetActive(false);
        }
    }




    public void AddItem(Item item, int StackToAdd)
    {
        if (item.stackable)
        {
            foreach (var existingItem in inventoryItems)
            {
                if (existingItem.UniqueName == item.itemName && existingItem.StackCount < item.maxStack)
                {
                    int remainingStack = item.maxStack - existingItem.StackCount;
                    int amountToAdd = Mathf.Min(StackToAdd, remainingStack);

                    existingItem.StackCount += amountToAdd;
                    StackToAdd -= amountToAdd;
                }
            }
        }

        if (StackToAdd > 0)
        {
            inventoryItems.Add(new ItemData { UniqueName=item.itemName, StackCount= StackToAdd }); ///шо то таке
        }

        UpdateInventoryUI();
    }


    public bool DeleteTheSpecifiedItem(Item item, int amountToRemove)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].UniqueName == item.itemName)
            {
                if (inventoryItems[i].StackCount >= amountToRemove)
                {
                    inventoryItems[i].StackCount -= amountToRemove;
                    Debug.Log("дійшло =-=-=-=-=-=-");
                    return true;
                    
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }


    public void UseItem(int myIndex)
    {
        var item = allItems.Find(itemSO => itemSO.itemName == inventoryItems[myIndex].UniqueName);
        if (item.canUse)
        {
            inventoryItems[myIndex].StackCount--;
            playerController.usingItem(item);
            if (inventoryItems[myIndex].StackCount <= 0)
            {
                RemoveItem(myIndex);
            }

            UpdateInventoryUI();
        }
        
    }

    public void EquipWeaponItem(int weaponIndex)
    {
        var item = allItems.Find(itemSO => itemSO.itemName == inventoryItems[weaponIndex].UniqueName);

        if (item is WeaponItem weaponItem)
        {

            if (weapon1Slot == null)
            {
                weapon1Slot = item;
                RemoveItem(weaponIndex);
                UpdateInventoryUI();

            }
            else
            {
                AddItem(weapon1Slot, 1);
                weapon1Slot = item;
                RemoveItem(weaponIndex);
                UpdateInventoryUI();

            }




        }
    }
    public void UnequipWeaponItem()
    {
        AddItem(weapon1Slot, 1);
        weapon1Slot = null;
        weaponChanger.currentWeaponItem = null;
        weaponChanger.ChangeWeapon();
        UpdateInventoryUI();
    }




    public void RemoveItem(int index)
    {
        if (index >= 0 && index < inventoryItems.Count)
        {
            inventoryItems.RemoveAt(index);
            UpdateInventoryUI();
            toolTipPanelOn = false;
        }
    }

    public void UpdateInventoryUI()
    {

        
        // Очищуємо попередні клітинки айтемів на канвасі
        itemsParent.DestroyChildrens();

        // Створюємо нові клітинки айтемів для всіх предметів у списку inventoryItems
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            var item = allItems.Find(itemSO => itemSO.itemName == inventoryItems[i].UniqueName);
            // можливо краще використовувати айді предмету як числова зміна.
            // потрібно зробити так, щоб уникнути методу Find, або перебору кожного елементу при пошуку.
            //Dictionary - ознайомитись.

            //itemDescriptionName.SetText(item.itemName);
            //itemDescription.SetText(item.description);
            //itemDescriptionImage.sprite = item.itemImage;


            if (item != null)
            {
                if (inventoryItems[i].StackCount > 0)
                {
                    CreateItemCell(item, i);
                }
                else
                {
                    RemoveItem(i);
                }
                
            }
            else
            {
                Debug.Log("Крч, сталась та фігня,про яку говорив Кукумбер. Типу якась там обнова, імена і т.д. розберешся, удачі в майбутньому.");


            }

        }

        
    }

    private void CreateItemCell(Item item, int index)
    {
        var newItemCell = Instantiate(itemCellPrefab, itemsParent);
        var count = inventoryItems[index].StackCount;
        newItemCell.UpdateVisual(item, index, count);
        _itemCells.Add(newItemCell);
        newItemCell.OnClicked += ItemCellOnClicked;
    }

    private void ItemCellOnClicked(ItemCell cell, int index)
    {
        
        //RemoveItem(index);
        _selectedCellIndex = index;
        OnItemSelected?.Invoke();
        toolTipPanelOn = true;

        var item = allItems.Find(itemSO => itemSO.itemName == inventoryItems[index].UniqueName);
        
        if (item is WeaponItem weaponItem)
        {
            itemDescription.SetText(
                "Atack speed: " + weaponItem.weaponSpeed +"\n" + 
                "Damage: " + weaponItem.weaponDamage + "\n" + "\n" +
                item.description
                );

            itemDescriptionName.SetText(item.itemName);
            itemDescriptionType.SetText(item.itemType);
            itemDescriptionType.color = item.itemTypeColor;
            itemDescriptionImage.sprite = item.itemImage;
        }
        else
        {
            itemDescriptionName.SetText(item.itemName);
            itemDescription.SetText(item.description);
            itemDescriptionType.SetText(item.itemType);
            itemDescriptionType.color = item.itemTypeColor;
            itemDescriptionImage.sprite = item.itemImage;
        }

    }

    public void RemoveSelecteditem()
    {
        if (_selectedCellIndex != -1)
        {
            RemoveItem(_selectedCellIndex);
            _selectedCellIndex = -1;
            OnItemDeselected?.Invoke();
            toolTipPanelOn = false;
        }
    }
}

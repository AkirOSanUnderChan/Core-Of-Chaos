using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public Action OnItemSelected;
    public Action OnItemDeselected;


    public GameObject descriptionPanel;
    public Image itemDescriptionImage;
    public TextMeshProUGUI itemDescriptionName;
    public TextMeshProUGUI itemDescription;

    public bool toolTipPanelOn;



    public static InventoryManager instance;

    public List<ItemData> inventoryItems = new List<ItemData>();

    public List<Item> allItems = new List<Item>();

    [SerializeField]
    private Transform itemsParent;

    public ItemCell itemCellPrefab;
    public Button dropButton; // Public ����� ��� ��������� �� ������ "�������� �������"

    private List<ItemCell> _itemCells = new();

    private int _selectedCellIndex = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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

    public void AddItem(Item item, int currentStack)
    {
        if (item.stackable)
        {
            foreach (var existingItem in inventoryItems)
            {
                if (existingItem.UniqueName == item.itemName && existingItem.StackCount < item.maxStack)
                {
                    int remainingStack = item.maxStack - existingItem.StackCount;
                    int amountToAdd = Mathf.Min(currentStack, remainingStack);

                    existingItem.StackCount += amountToAdd;
                    currentStack -= amountToAdd;
                }
            }
        }

        if (currentStack > 0)
        {
            inventoryItems.Add(new ItemData { UniqueName=item.itemName, StackCount=currentStack }); ///�� �� ����
        }

        UpdateInventoryUI();
    }



    public void RemoveItem(int index)
    {
        if (index >= 0 && index < inventoryItems.Count)
        {
            inventoryItems.RemoveAt(index);
            UpdateInventoryUI();
        }
    }

    private void UpdateInventoryUI()
    {

        
        // ������� �������� ������� ������ �� ������
        itemsParent.DestroyChildrens();

        // ��������� ��� ������� ������ ��� ��� �������� � ������ inventoryItems
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            var item = allItems.Find(itemSO => itemSO.itemName == inventoryItems[i].UniqueName);
            // ������� ����� ��������������� ��� �������� �� ������� ����.
            // ������� ������� ���, ��� �������� ������ Find, ��� �������� ������� �������� ��� ������.
            //Dictionary - ������������.

            //itemDescriptionName.SetText(item.itemName);
            //itemDescription.SetText(item.description);
            //itemDescriptionImage.sprite = item.itemImage;


            if (item != null)
            {

                CreateItemCell(item, i);
            }
            else
            {
                Debug.Log("���, ������� �� �����,��� ��� ������� ��������. ���� ����� ��� ������, ����� � �.�. ����������, ����� � �����������.");


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
        itemDescriptionName.SetText(item.itemName);
        itemDescription.SetText(item.description);
        itemDescriptionImage.sprite = item.itemImage;

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

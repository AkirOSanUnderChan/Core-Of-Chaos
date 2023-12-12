using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UI.Image;
using Text = TMPro.TextMeshProUGUI;

public class TradeCell : MonoBehaviour
{
    public TradeWindow tradeWindow;


    public Item currentTradeItem;

    public Image itemImage;
    public Text itemName;
    public Text itemPriceText;
    public Button buy1XButton;
    public Button checkItemDescription;
    public Item valuteOfItem;
    public int itemPrice;

    public int CellIndex;


    void Start()
    {
        tradeWindow = GetComponentInParent<TradeWindow>();

        UpdateTradeVisual();


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateTradeVisual()
    {
        if (currentTradeItem != null)
        {
            itemImage.sprite = currentTradeItem.itemImage;
            itemName.SetText(currentTradeItem.itemName);
            itemPriceText.SetText("Price: " + itemPrice + " gold");

        }
    }
    public void ItemSelected()
    {
        tradeWindow.selectedItem = currentTradeItem;
        tradeWindow.uptadeTradeItemDescription();
    }
    public void BuyButtonClicked()
    {
        Debug.Log("Клік на кнопку купити");
        bool sucsesefull =  InventoryManager.instance.DeleteTheSpecifiedItem(valuteOfItem, itemPrice);

        if (sucsesefull)
        {
            InventoryManager.instance.AddItem(currentTradeItem,1);
        }
    }
    public void SellButtonClicked()
    {
        Debug.Log("Клік на кнопку купити");
        bool sucsesefull = InventoryManager.instance.DeleteTheSpecifiedItem(currentTradeItem, 1);

        if (sucsesefull)
        {
            InventoryManager.instance.AddItem(valuteOfItem, itemPrice);
        }
    }
}

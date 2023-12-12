using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Text = TMPro.TextMeshProUGUI;

public class TradeWindow : MonoBehaviour
{

    public Item selectedItem;

    public GameObject tradeDescrPanelObj;

    public Image itemDescrIcon;
    public Text itemDescrName;
    public Text itemDescrType;
    public Text itemDescrText;


    void Start()
    {
        
    }

    void Update()
    {
        if (selectedItem != null)
        {
            tradeDescrPanelObj.SetActive(true);
        }
        else
        {
            tradeDescrPanelObj.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            tradeDescrPanelObj.SetActive(false);
            PlayerControllerSingleton.Instance.playerMenuManager.playerInWindow = false;
        }


    }

    public void uptadeTradeItemDescription()
    {
        if (selectedItem != null)
        {
            itemDescrIcon.sprite = selectedItem.itemImage;
            itemDescrName.SetText(selectedItem.itemName);
            itemDescrType.SetText(selectedItem.itemType);
            itemDescrType.color = selectedItem.itemTypeColor;
            itemDescrText.SetText(selectedItem.description);
        }
    }
}

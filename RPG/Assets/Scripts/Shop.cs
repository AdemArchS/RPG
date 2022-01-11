using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    public Text goldText;

    public string[] itemsForSale;

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    public Item selectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy)
        // {
        //     OpenShop();
        // }
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();

        GameManager.instance.shopActive = true;

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);

        GameManager.instance.shopActive = false;
    }

    public void OpenBuyMenu()
    {
        //call first object just to show it
        buyItemButtons[0].Press();
        //This sets selected to first item you are able to buy in shop when you first enter the buy menu(aka don't press the buy button)
        selectedItem = GameManager.instance.GetItemDetails(itemsForSale[0]);

        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for(int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;

            if(itemsForSale[i] != "")
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";
            }
            else
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

    public void OpenSellMenu()
    {
        sellItemButtons[0].Press();
        
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        ShowSellItems();
    }

    private void ShowSellItems()
    {
        GameManager.instance.SortItems();
        for(int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

    //Shows the item's name, description, and value, that you want to buy.
    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
 
        if (selectedItem != null)
        {
            buyItemName.text = selectedItem.itemName;
            buyItemDescription.text = selectedItem.description;
            buyItemValue.text = "Value: " + selectedItem.value + "g";
        }
        else
        {
            buyItemName.text = "";
            buyItemDescription.text = "";
            buyItemValue.text = "";
        }
    }

    //Shows the item's name, description, and value, that you want to sell.
    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;
 
        if (selectedItem != null)
        {
            sellItemName.text = selectedItem.itemName;
            sellItemDescription.text = selectedItem.description;
            sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * 0.5f).ToString() + "g";
        }
        else
        {
            sellItemName.text = "";
            sellItemDescription.text = "";
            sellItemValue.text = "";
        }
    }

    //Buys selected item, removing gold and adding to inventory
    public void BuyItem()
    {
        if(selectedItem != null)
        {
            if(GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;
                GameManager.instance.AddItem(selectedItem.itemName);
            }

            goldText.text = GameManager.instance.currentGold.ToString() + "g";
        }
    }

    //Sells selected item, adding gold and removing from inventory
    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * 0.5f);
            GameManager.instance.RemoveItem(selectedItem.itemName);
            //If there is no more after selling set selected item to nothing.
            if(GameManager.instance.NumberOfItem(selectedItem.itemName) == 0)
            {
                selectedItem = null;
                SelectSellItem(selectedItem);
            }

            goldText.text = GameManager.instance.currentGold.ToString() + "g";

            ShowSellItems();
        }
    }
}

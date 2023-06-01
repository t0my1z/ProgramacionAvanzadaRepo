using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tienda : MonoBehaviour
{
    #region "Singleton"
    public static Tienda instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }
    #endregion

    List<ItemBase> ShopList;
    public Inventory inventoryRef;
    [SerializeField] TextMeshProUGUI currentCharacterMoney;

    public ItemSelectableObject selectedItem;

    [Header("Canvas refs")]
    [SerializeField] GameObject buyPanel;
    [SerializeField] GameObject sellPanel;
    [SerializeField] GameObject equipButton;
    [SerializeField] TextMeshProUGUI panelButtonText;
    [SerializeField] TextMeshProUGUI actionButtonText;
    public RectTransform selector;
    private bool isBuying = true;

    private void Start()
    {
        UpdateCharacterMoney();
       
    }
    public void action()
    {
        if (selectedItem == null)
        { 
            return;
        }
        if (isBuying) 
        {
            if (selectedItem.item.Buy(inventoryRef))
            {
                selector.gameObject.SetActive(false);
                Destroy(selectedItem.gameObject);
                UpdateCharacterMoney();
            }
        }
        else 
        {
            if (selectedItem.equipItem != null)
            {
                inventoryRef.SellEquipmentItem(selectedItem.equipItem);
            }
           
            selectedItem.item.Sell(inventoryRef);
            selector.gameObject.SetActive(false);
            Destroy(selectedItem.gameObject);
            UpdateCharacterMoney();
        }
    }

    public void switchPanels()
    {
        if (isBuying)
        {
            buyPanel.SetActive(false);
            panelButtonText.text = "Tienda";
            sellPanel.SetActive(true);
            actionButtonText.text = "Vender";
            inventoryRef.DisplayAllItems();
            equipButton.SetActive(true);
          
        }
        else
        {
            buyPanel.SetActive(true);
            equipButton.SetActive(false);
            panelButtonText.text = "Inventario";
            sellPanel.SetActive(false);
            actionButtonText.text = "Comprar";
        }
        //cambiamos el bool entre cada uso del boton
        isBuying = !isBuying;
        //desactivamos el selector
        selector.gameObject.SetActive(false);
    }

    void UpdateCharacterMoney()
    {
        currentCharacterMoney.text = inventoryRef.currentGold.ToString();
    }
    
}

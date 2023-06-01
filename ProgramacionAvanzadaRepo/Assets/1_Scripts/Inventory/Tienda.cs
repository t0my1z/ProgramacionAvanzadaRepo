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
    [SerializeField] GameObject iconButtonPrefab;
    [SerializeField] Transform parentForIconPrefabs;
    [Header("Canvas refs")]
    [SerializeField] GameObject buyPanel;
    [SerializeField] GameObject sellPanel;
    [SerializeField] GameObject equipButton;
    [SerializeField] TextMeshProUGUI panelButtonText;
    [SerializeField] TextMeshProUGUI actionButtonText;
    public RectTransform selector;
    public bool isBuying = true;

    private void Start()
    {
        UpdateCharacterMoney();
        SetShop();
    }
    public void Action() 
    {
        if (selectedItem == null) return;

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
            EquipmentItem equipItem = (EquipmentItem)selectedItem.item;
            if (equipItem != null)
            {
                inventoryRef.SellEquipmentItem(equipItem);
            }

            selectedItem.item.Sell(inventoryRef);
            selector.gameObject.SetActive(false);
            Destroy(selectedItem.gameObject);
            UpdateCharacterMoney();
            
        }
    }

    public void SwitchPanels()
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

    void SetShop()
    {
        ItemBase[] itemsInResources = Resources.LoadAll<ItemBase>("TestItems");

        for (int i = 0; i < itemsInResources.Length; i++)
        {
            GameObject inst = Instantiate(iconButtonPrefab, parentForIconPrefabs);
            ItemSelectableObject itemSelect = inst.GetComponent<ItemSelectableObject>();
            itemSelect.item = itemsInResources[i];
            itemSelect.SetItem();
        }
    }
    
    public void SetEquipButton(bool state)
    {
        equipButton.SetActive(state);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

[CreateAssetMenu(fileName = "newItemShop", menuName = "ProgramacionAvanzadaRepo/ItemShop", order = 1)]
public class ItemBase : ScriptableObject, Interfaces.ISellable, Interfaces.IBuyable
{
    public string Name;
    public Sprite icon;


    public int capacitySlot;
    public int weight;

    [SerializeField] int price;
    [SerializeField] int sellingPrice;

    public bool Buy(Inventory inventoryRef)
    {
        if (inventoryRef.CanBuyItem(price) && inventoryRef.AddItemToInventory(this))
        {
            inventoryRef.currentGold -= price;
            Debug.Log("Has comprado el objeto " + Name);
            return true;
        }

        return false;
    }

    public int GetPrice()
    {
        return price;
    }

    public int GetSellingPrice()
    {
        return sellingPrice;
    }

    public void Sell(Inventory inventoryRef)
    {
        inventoryRef.DropItemFromInventory(this);
        inventoryRef.currentGold += sellingPrice;
        Debug.Log("Has vendido el objeto " + Name);
    }
}

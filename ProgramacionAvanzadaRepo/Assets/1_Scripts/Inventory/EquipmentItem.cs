using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEquipmentItem", menuName = "ProgramacionAvanzadaRepo/Equipment", order = 2)]
public class EquipmentItem : ItemBase
{
    public Inventory.BodyPart bodyPart;

    public int defence;
    public int attack;

    public override bool Buy(Inventory inventoryRef)
    {
        if (inventoryRef.CanBuyItem(price) && inventoryRef.AddItemToInventory(this))
        {
            inventoryRef.currentGold -= price;
            Debug.Log("Has comprado el objeto equipable " + Name);
            return true;
        }

        return false;
    }

    public override void Sell(Inventory inventoryRef)
    {
        inventoryRef.DropItemFromInventory(this);
        inventoryRef.currentGold += sellingPrice;
        Debug.Log("Has vendido el objeto equipable " + Name);
    }
}

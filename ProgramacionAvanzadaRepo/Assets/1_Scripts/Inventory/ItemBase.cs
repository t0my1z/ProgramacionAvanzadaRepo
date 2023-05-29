using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class ItemBase : Interfaces.ISellable, Interfaces.IBuyable
{
    public string name;
    public Sprite icon;


    public int capacitySlot;
    public int weight;

    public int price;

    public int GetPrice()
    {
        throw new System.NotImplementedException();
    }

    public int GetSellingPrice()
    {
        throw new System.NotImplementedException();
    }
}

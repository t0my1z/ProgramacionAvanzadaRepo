using System.Collections.Generic;
using UnityEngine;

public class CharacterT
{
    public int defence;
    public int attack;
}
public class Inventory : MonoBehaviour
{
    CharacterT character;
    List<ItemBase> InventoryList;

    int maxCapacity;
    int currentCapacity;
    int currentWeight;
    int maxWeight;

    // GOLD
    int _currentGold;
    public int currentGold
    {
        get { return _currentGold;   }
        set { _currentGold = value;  }
    }
  
    public enum BodyPart
    {
        head,
        chest,
        arms,
        legs,
        weapon
    }

    /// Inventory
    public void AddItemToInventory(ItemBase newItem)
    {
        if (newItem.weight + currentWeight > maxWeight)
        {
            return;
        }

        if (newItem.capacitySlot + currentCapacity > maxCapacity)
        {
            return;
        }

        InventoryList.Add(newItem);
    }

    public void DropItemFromInventory(ItemBase dropItem)
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i] == dropItem)
            {
                InventoryList.RemoveAt(i);
                break;
            }
        }

        currentCapacity -= dropItem.capacitySlot;
        currentWeight -= dropItem.weight;
    }


    /// Player Equipment
    Dictionary<BodyPart, EquipmentItem> PlayerEquipment;

    void InitializeEquipment()
    {
        Dictionary<BodyPart, EquipmentItem> PlayerEquipment = new Dictionary<BodyPart, EquipmentItem>
        {
            {BodyPart.head, null },
            {BodyPart.chest, null},
            {BodyPart.arms, null },
            {BodyPart.legs, null },
            {BodyPart.weapon, null}
        };
    }

    void EquipNewItem(EquipmentItem newEquipment)
    {
        if (PlayerEquipment.ContainsValue(newEquipment))
        {
            Debug.Log("Esta pieza ya está equipada");
            return;
        }

        if (PlayerEquipment[newEquipment.bodyPart] == null)
        {
            AddStats(newEquipment);
            Debug.Log("Has añadido la pieza del " + newEquipment.bodyPart + "por " + newEquipment.name);
        }
        if (PlayerEquipment.ContainsKey(newEquipment.bodyPart))
        {
            PlayerEquipment[newEquipment.bodyPart] = newEquipment;
            Debug.Log("Has sustituido la pieza de " + newEquipment.bodyPart + "por " + newEquipment.name);
            UpdateStats();
        }
    }

    void UnEquipItem(BodyPart bodyPart)
    {
        if (PlayerEquipment.ContainsKey(bodyPart))
        {
            PlayerEquipment.Remove(bodyPart);
            DeleteStats(bodyPart);
        }
    }

    void UpdateStats()
    {
        Debug.Log("Equipment del jugado:");
        foreach (KeyValuePair<BodyPart, EquipmentItem> item in PlayerEquipment)
        {
            RefreshItemStats(item.Value);
            Debug.Log(item.Key + ": " + item.Value.name);
        }
    }

    void RefreshItemStats(EquipmentItem newItem)
    {
        DeleteStats(newItem.bodyPart);
        AddStats(newItem);
    }

    void DeleteStats(BodyPart bodyPart)
    {
        if (PlayerEquipment[bodyPart] == null)
        {
            return;
        }

        character.defence -= PlayerEquipment[bodyPart].defence;
        character.attack -= PlayerEquipment[bodyPart].attack;
    }
    void AddStats(EquipmentItem newItem)
    {
        character.defence += newItem.defence;
        character.attack += newItem.attack;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
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

    [SerializeReference]CharacterBase character;
    List<ItemBase> InventoryList = new List<ItemBase>();

    int maxCapacity = 100;
    int currentCapacity;

    int maxWeight = 100;
    int currentWeight;

  
    // GOLD
    [SerializeField] int _currentGold;
    public int currentGold
    {
        get { return _currentGold;   }
        set { 
            _currentGold = Mathf.Clamp(value, 0, 9999);
            }
    }

    public enum BodyPart
    {
        head,
        chest,
        arms,
        legs,
        weapon
    }

    private void Start()
    {
        RefreshStatsUI();
       
    }
    /// Inventory
    public bool CanBuyItem(int itemCost)
    {
        if (currentGold -itemCost< 0)
        {
            Debug.Log("No tienes suficiente dinero");
            return false;
        }
            return true;
    }
    public bool AddItemToInventory(ItemBase newItem)
    {
        if (newItem.weight + currentWeight > maxWeight)
        {
            Debug.Log("Tienes demasiado peso encima, no puede coger el objeto " + newItem.Name);
            return false;
        }

        if (newItem.capacitySlot + currentCapacity > maxCapacity)
        {
            Debug.Log("No te quedas huecos libres en el inventario, no puede coger el objeto " + newItem.Name);
            return false;
        }

        InventoryList.Add(newItem);
        Debug.Log("Has metido al inventario el objeto " + newItem.Name);
        return true;
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
    Dictionary<BodyPart, EquipmentItem> PlayerEquipment = new Dictionary<BodyPart, EquipmentItem>
        {
            {BodyPart.head, null },
            {BodyPart.chest, null},
            {BodyPart.arms, null },
            {BodyPart.legs, null },
            {BodyPart.weapon, null}
        };

    /// <summary>
    /// Comprueba si el objeto que vendemos está equipado y elimina las stats 
    /// </summary>
    public void SellEquipmentItem(EquipmentItem itemToSell)
    {
        foreach (KeyValuePair<BodyPart, EquipmentItem> item in PlayerEquipment)
        {
            if (item.Value == itemToSell)
            {
                DeleteStats(itemToSell.bodyPart);
                RefreshStatsUI();
                ResetImage(itemToSell.bodyPart);
                Debug.Log("Has vendido un objeto que tienes equipado");
                continue;
            }
        }
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
            PlayerEquipment[newEquipment.bodyPart] = newEquipment;
            SetImage(newEquipment.bodyPart, newEquipment);
            AddStats(newEquipment);
            Debug.Log("Has añadido la pieza del " + newEquipment.bodyPart + "por " + newEquipment.Name);
        }
        else
        {
            DeleteStats(newEquipment.bodyPart);
            PlayerEquipment[newEquipment.bodyPart] = newEquipment;
            Debug.Log("Has sustituido la pieza de " + newEquipment.bodyPart + "por " + newEquipment.Name);
            SetImage(newEquipment.bodyPart, newEquipment);
            AddStats(newEquipment);
           
        }

        RefreshStatsUI();
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

    [Header("Inventory UI")]
    public ItemSelectableObject selectedItem;

    public Transform parentContent;
    public GameObject selectObject;
    public Image[] slotsEquipment = new Image[5];

    public List<GameObject> slotsItemsUI;

    public TextMeshProUGUI attackTxt;
    public TextMeshProUGUI defenceTxt;

    void SetImage(BodyPart bodyPart, EquipmentItem equipmentItem)
    {
        switch (bodyPart)
        {
            case BodyPart.head:
                slotsEquipment[0].sprite = equipmentItem.icon;
                break;
            case BodyPart.chest:
                slotsEquipment[1].sprite = equipmentItem.icon;
                break;
            case BodyPart.arms:
                slotsEquipment[2].sprite = equipmentItem.icon;
                break;
            case BodyPart.legs:
                slotsEquipment[3].sprite = equipmentItem.icon;
                break;
            case BodyPart.weapon:
                slotsEquipment[4].sprite = equipmentItem.icon;
                break;
            default:
                break;
        }
    }

    void ResetImage(BodyPart bodyPart)
    {
        slotsEquipment[((int)bodyPart)].sprite = null;
    }
    public void EquipAction()
    {
        if (selectedItem == null) return;

        EquipmentItem itemToEquip = (EquipmentItem)selectedItem.item;

        EquipNewItem(itemToEquip);
    }
    void RefreshStatsUI()
    {
        attackTxt.text = character.attack.ToString();
        defenceTxt.text = character.defence.ToString();
    }
    public void DisplayAllItems()
    {
        for (int i = 0; i < slotsItemsUI.Count; i++)
        {
            GameObject objectToDestroy = slotsItemsUI[i];
            Destroy(objectToDestroy);
        }

        slotsItemsUI.Clear();

        for (int i = 0; i < InventoryList.Count; i++)
        {
            GameObject newItem = Instantiate(selectObject, parentContent);
            newItem.GetComponent<ItemSelectableObject>().item = InventoryList[i];
            newItem.GetComponent<ItemSelectableObject>().SetItem();
            slotsItemsUI.Add(newItem);
        }
    }

    public List<ConsumableItem> GetConsumableItemsFromInventory()
    {
        List<ConsumableItem> consumList = new List<ConsumableItem>();
        consumList = InventoryList.OfType<ConsumableItem>().ToList();


        return consumList;
    }
}

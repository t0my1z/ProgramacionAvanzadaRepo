using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Para poder seleccionar los items en la UI
/// </summary>
public class ItemSelectableObject : MonoBehaviour
{
    public ItemBase item;
    public EquipmentItem equipItem;

    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI costeTexto;


    public virtual void SelectItem()
    {
        Inventory.instance.selectedItem = this;
        Tienda.instance.selectedItem = this;
        Tienda.instance.selector.gameObject.SetActive(true);
        Tienda.instance.selector.position = this.GetComponent<RectTransform>().position;


        if (item is ConsumableItem && !Tienda.instance.isBuying) Tienda.instance.SetEquipButton(false);
        else if(!Tienda.instance.isBuying) Tienda.instance.SetEquipButton(true);
    }

    public void SetItem()
    {
        image.sprite = item.icon;
        costeTexto.text = item.GetPrice().ToString();
    }

}

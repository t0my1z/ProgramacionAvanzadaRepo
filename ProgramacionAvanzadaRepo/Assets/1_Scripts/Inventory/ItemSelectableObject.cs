using System.Collections;
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

    
    private void Start()
    {
        equipItem = Resources.Load<EquipmentItem>("TestItems/" + item.name);

        if (item != null)
        {
            image.sprite = item.icon;
        }
      
        if (costeTexto != null)
        {
            costeTexto.text = item.GetPrice().ToString();
        }
     
    }

    public virtual void SelectItem()
    {
        Inventory.instance.selectedItem = this;
        Tienda.instance.selectedItem = this;
        Tienda.instance.selector.gameObject.SetActive(true);
        Tienda.instance.selector.position = this.GetComponent<RectTransform>().position;
    }

}

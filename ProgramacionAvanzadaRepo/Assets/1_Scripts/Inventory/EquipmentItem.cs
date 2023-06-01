using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEquipmentItem", menuName = "ProgramacionAvanzadaRepo/Equipment", order = 2)]
public class EquipmentItem : ItemBase
{
    public Inventory.BodyPart bodyPart;

    public int defence;
    public int attack;
}

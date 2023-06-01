using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newConsumableItem", menuName = "ProgramacionAvanzadaRepo/Consumable", order = 2)] 
public class ConsumableItem : ItemBase, Interfaces.IInteractable
{
    public CharacterBase _char;
    public ConsumableType typeOfConsumable;

    public int damageIncrement;
    public float critMultiplier;
    public int defenseIncrement;

    public enum ConsumableType
    {
        Damage, CriticalMultiplier, Defense
    }


    public void Interact()
    {
        UseItem(_char);
    }

    protected virtual void UseItem(CharacterBase charToAffect)
    {
        if(typeOfConsumable == ConsumableType.Damage)
        {
            _char.attack += damageIncrement;
        }
        else if(typeOfConsumable == ConsumableType.CriticalMultiplier)
        {
            _char._criticalProbability *= critMultiplier;
        }
        else if (typeOfConsumable == ConsumableType.Defense)
        {
            _char._protection += defenseIncrement;
        }
    }
}

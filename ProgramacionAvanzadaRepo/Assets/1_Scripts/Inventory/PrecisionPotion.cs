using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionPotion : ConsumableItem
{
    public float critMultiplier = 1.2f; 

    public PrecisionPotion(string n, CharacterBase character)
    {
        Name = n;
        _char = character;
    }

    protected override void UseItem(CharacterBase charToAffect)
    {
        charToAffect._criticalProbability *= critMultiplier;
    }
}

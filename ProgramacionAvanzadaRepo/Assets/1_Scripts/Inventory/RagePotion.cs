using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagePotion : ConsumableItem
{
    public int damageMultiplier = 2;

    public RagePotion(string n , CharacterBase character)
    {
        Name = n;
        _char = character;
    }
    protected override void UseItem(CharacterBase charToAffect)
    {
        charToAffect.attack *= damageMultiplier;
    }
}

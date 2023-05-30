using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : ItemBase, Interfaces.IInteractable
{
    public CharacterBase _char;

    public void Interact()
    {
        UseItem(_char);
    }

    protected virtual void UseItem(CharacterBase charToAffect)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Interfaces 
{
    public interface IBuyable
    {
        public bool Buy(Inventory inventoryRef);
        public int GetPrice();
    }

    public interface ISellable
    {
        public void Sell(Inventory inventoryRef);
        public int GetSellingPrice();  
    }

    public interface IDamageable
    {
        public void TakeDamage(int amount);
        public void Die();
    }

    public interface IInteractable
    {
        public void Interact();
        bool canInteractAgain { get { return canInteractAgain; } set { canInteractAgain = value; } }
    }

}

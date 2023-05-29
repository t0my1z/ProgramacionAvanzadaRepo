using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Usable : MonoBehaviour, Interfaces.IInteractable
{
    [SerializeField] private UnityEvent InteractEvent;

    public void Interact()
    {
        InteractEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.GetComponent<Player>() != null)
        //{
        //    //Show Interact UI
            
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        //Look for player input
        //Fire interact event
        //Interact();
    }

    private void OnTriggerExit(Collider other)
    {
        //Hide interact UI if its not already hidden
    }
}

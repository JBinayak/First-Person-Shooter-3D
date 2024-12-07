using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class interactable : MonoBehaviour
{
    public string promptMessage;
    public bool useEvents;

    public virtual string OnLook()
    {
        return promptMessage;
    }

    public void baseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionScript>().onInteract.Invoke();
        }
        Interact();
    }

    protected virtual void Interact()
    {

    }
}

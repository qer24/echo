using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool CanBeInteracted = true;
    [SerializeField] private UnityEvent OnInteract;

    public virtual void Interact(PlayerInteractor playerInteractor)
    {
        if (!CanBeInteracted) return;

        OnInteract?.Invoke();
    }

    public virtual void OnStartHover()
    {
        //Debug.Log("Hovering over " + name);
    }

    public virtual void OnEndHover()
    {
        //Debug.Log("No longer hovering over " + name);
    }

    public void SetInteractable(bool value)
    {
        CanBeInteracted = value;
    }
}

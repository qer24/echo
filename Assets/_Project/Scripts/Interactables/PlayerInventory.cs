using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private readonly List<Interactable> _inventory = new();

    public void AddToInventory(Interactable interactable)
    {
        _inventory.Add(interactable);
    }

    public void RemoveFromInventory(Interactable interactable)
    {
        _inventory.Remove(interactable);
    }

    public bool HasInInventory(Interactable interactable)
    {
        return _inventory.Contains(interactable);
    }
}

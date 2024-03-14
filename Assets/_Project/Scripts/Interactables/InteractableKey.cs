using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class InteractableKey : Interactable
{
    [SerializeField] private EventReference KeyPickupSound;

    public override void Interact(PlayerInteractor playerInteractor)
    {
        base.Interact(playerInteractor);

        KeyPickupSound.Play();

        playerInteractor.GetComponent<PlayerInventory>().AddToInventory(this);

        gameObject.SetActive(false);
    }
}

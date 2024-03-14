using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class InteractableDoor : Interactable
{
    [SerializeField] private Transform[] DoorSegments;
    [SerializeField] private Vector3[] DoorSegmentsTargetRotations;
    [SerializeField] private AutoTween DoorOpenTween;

    [Space]
    [SerializeField] private InteractableKey KeyToOpen;
    [SerializeField] private string FailToInteractMessage;

    [Space]
    [SerializeField] private EventReference DoorOpenSound;
    [SerializeField] private EventReference DoorClosedSound;

    private void Awake()
    {
        DoorOpenTween.AutoCancel = false;
    }

    public override void Interact(PlayerInteractor playerInteractor)
    {
        if (!CanBeInteracted) return;
        if (KeyToOpen == null || !playerInteractor.GetComponent<PlayerInventory>().HasInInventory(KeyToOpen))
        {
            DoorClosedSound.Play(transform.position);
            playerInteractor.FailToInteract(FailToInteractMessage);
            return;
        }

        base.Interact(playerInteractor);

        DoorOpenSound.Play(transform.position);

        for (int i = 0; i < DoorSegments.Length; i++)
        {
            var segment = DoorSegments[i];
            var targetRotation = DoorSegmentsTargetRotations[i];

            DoorOpenTween.Play(segment.LeanRotateLocal(targetRotation.x, targetRotation.y, targetRotation.z, DoorOpenTween.Time));
        }

        CanBeInteracted = false;
    }

    public void SetKey(InteractableKey key)
    {
        KeyToOpen = key;
    }
}

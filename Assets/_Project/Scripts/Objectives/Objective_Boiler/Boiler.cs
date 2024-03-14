using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Boiler : Interactable
{
    [SerializeField] private BoilerRepairMinigame BoilerRepair;
    [SerializeField] private Objective RepairObjectiveFirst, RepairObjectiveSecond;
    [SerializeField] private float Cooldown = 2f;

    [Space]
    [SerializeField] private string FailToInteractMessage;
    [SerializeField] private EventReference NoWrenchSound;

    private static readonly int FRESNEL_SPEED = Shader.PropertyToID("_FresnelSpeed");

    private InteractableKey _wrench;
    private PlayerInteractor _playerInteractor;

    public void SetWrench(InteractableKey wrench)
    {
        _wrench = wrench;
    }

    public override void Interact(PlayerInteractor playerInteractor)
    {
        if (!CanBeInteracted) return;

        base.Interact(playerInteractor);

        if (_wrench == null || !playerInteractor.GetComponent<PlayerInventory>().HasInInventory(_wrench))
        {
            NoWrenchSound.Play(transform.position);
            playerInteractor.FailToInteract(FailToInteractMessage);
            return;
        }

        _playerInteractor = playerInteractor;

        CanBeInteracted = false;
        BoilerRepair.StartMinigame();

        BoilerRepair.OnRepair += OnRepair;

        playerInteractor.TryEndHover();
    }

    private void OnRepair(bool repaired)
    {
        BoilerRepair.OnRepair -= OnRepair;

        if (!repaired)
        {
            WaitUtils.Wait(Cooldown, true, () =>
            {
                CanBeInteracted = true;
                _playerInteractor.TryEndHover();
            });
        }
        else
        {
            if (RepairObjectiveFirst.IsAdded && !RepairObjectiveFirst.IsCompleted) RepairObjectiveFirst.CompleteObjective();
            else if (RepairObjectiveSecond.IsAdded) RepairObjectiveSecond.CompleteObjective();

            GetComponent<Renderer>().material.SetFloat(FRESNEL_SPEED, 0);
        }
    }
}

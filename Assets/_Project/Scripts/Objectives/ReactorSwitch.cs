using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorSwitch : Interactable
{
    [SerializeField] private Objective FinalObjective;
    [SerializeField] private string FailToInteractMessage;

    [Space]
    [SerializeField] private Transform LeverHandle;
    [SerializeField] private float LeverRotation = -45f;
    [SerializeField] private AutoTween LeverTween;

    [Space]
    [SerializeField] private GameObject EndNoise;
    [SerializeField] private AutoTween EndNoiseTween;

    [Space]
    [SerializeField] private float TimeToLoadScene = 1f;

    private static readonly int ALPHA = Shader.PropertyToID("_Alpha");

    public override void Interact(PlayerInteractor playerInteractor)
    {
        if (!CanBeInteracted) return;
        if (!FinalObjective.IsAdded)
        {
            playerInteractor.FailToInteract(FailToInteractMessage);
            return;
        }

        base.Interact(playerInteractor);

        CanBeInteracted = false;

        FinalObjective.CompleteObjective();

        LeverTween.Play(LeverHandle.LeanRotateLocal(LeverRotation, 0f, 0f, LeverTween.Time));

        EndNoise.gameObject.SetActive(true);
        var endNoiseMaterial = EndNoise.GetComponent<MeshRenderer>().material;
        EndNoiseTween.Play(LeanTween.value(0, 1, EndNoiseTween.Time)).setOnUpdate(value =>
        {
            endNoiseMaterial.SetFloat(ALPHA, value);
        });

        WaitUtils.Wait(TimeToLoadScene, true, () =>
        {
            SceneTransition.Instance.LoadScene(0);
        });
    }
}

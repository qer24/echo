using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairSwapper : MonoBehaviour
{
    [SerializeField] private PlayerInteractor PlayerInteractor;

    [Space]
    [SerializeField] private Image NormalCrosshair;
    [SerializeField] private Image InteractCrosshair;
    [SerializeField] private Image FailedInteractCrosshair;
    [SerializeField] private TextMeshProUGUI FailedInteractText;
    [SerializeField] private float FailedInteractDuration = 1f;

    private bool _isInteractable;
    private bool _isShowingFailedInteraction;

    private void Start()
    {
        PlayerInteractor.OnInteractableChanged += OnInteractableChanged;
        PlayerInteractor.OnFailToInteract += OnFailToInteract;
    }

    private void OnInteractableChanged(Interactable interactable)
    {
        _isInteractable = interactable != null && interactable.CanBeInteracted;

        if (!_isShowingFailedInteraction) ChangeToInteractCursor(_isInteractable);
    }

    private void OnFailToInteract(string response)
    {
        StopAllCoroutines();
        StartCoroutine(FailedToInteractCoroutine(response));
    }

    private IEnumerator FailedToInteractCoroutine(string response)
    {
        NormalCrosshair.enabled = false;
        InteractCrosshair.gameObject.SetActive(false);
        FailedInteractCrosshair.gameObject.SetActive(true);
        FailedInteractText.gameObject.SetActive(true);
        FailedInteractText.text = response;

        _isShowingFailedInteraction = true;

        yield return new WaitForSeconds(FailedInteractDuration);

        FailedInteractCrosshair.gameObject.SetActive(false);
        FailedInteractText.gameObject.SetActive(false);
        ChangeToInteractCursor(_isInteractable);

        _isShowingFailedInteraction = false;
    }

    private void ChangeToInteractCursor(bool isInteractable)
    {
        NormalCrosshair.enabled = !isInteractable;
        InteractCrosshair.gameObject.SetActive(isInteractable);
    }
}

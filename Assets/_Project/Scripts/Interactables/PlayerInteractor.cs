using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float InteractDistance = 2f;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private LayerMask InteractableLayerMask;

    private Interactable _currentInteractable;

    public event Action<Interactable> OnInteractableChanged;
    public event Action<string> OnFailToInteract;
    public event Action OnInteractInput;

    private void LateUpdate()
    {
        if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out var hit, InteractDistance, InteractableLayerMask, QueryTriggerInteraction.Collide))
        {
            var interactable = hit.collider.GetComponent<Interactable>();
            if (interactable == null) interactable = hit.collider.GetComponentInParent<Interactable>();

            if (interactable != null)
            {
                if (_currentInteractable == interactable) return;

                TryEndHover();
                _currentInteractable = interactable;
                _currentInteractable.OnStartHover();
                OnInteractableChanged?.Invoke(_currentInteractable);
            }
            else TryEndHover();
        }
        else TryEndHover();
    }

    public void TryEndHover()
    {
        if (_currentInteractable == null) return;

        _currentInteractable.OnEndHover();
        _currentInteractable = null;
        OnInteractableChanged?.Invoke(null);
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            OnInteractInput?.Invoke();

            if (_currentInteractable == null) return;

            _currentInteractable.Interact(this);
            TryEndHover();
        }
    }

    public void FailToInteract(string response)
    {
        OnFailToInteract?.Invoke(response);
    }

    public void ClearInteractable()
    {
        TryEndHover();
    }

    // private void OnGUI()
    // {
    //     if (_currentInteractable != null)
    //     {
    //         GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Press E to interact with " + _currentInteractable.name);
    //     }
    // }
}

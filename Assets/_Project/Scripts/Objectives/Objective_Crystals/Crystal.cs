using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Interactable
{
    [SerializeField] private Renderer CrystalRenderer;
    [SerializeField, ColorUsage(true, true)] private Color InactiveColor1, InactiveColor2, ActiveColor;
    [SerializeField] private float InactiveColorSinSpeed = 1;
    [SerializeField] private float ChangeColorHalfTime = 0.1f;

    [Space]
    [SerializeField] private float SineFloatSpeed = 1;
    [SerializeField] private float SineFloatHeight;

    [Space]
    [SerializeField] private float CrystalFixShake = 0.5f;
    [SerializeField] private Objective[] CrystalFixObjectives;

    private float _startY;
    private Color _currentColor;
    private bool _isActive;
    private static readonly int DISTORT_DISTANCE = Shader.PropertyToID("_DistortDistance");

    private void Start()
    {
        _currentColor = InactiveColor1;
        _startY = transform.position.y;
    }

    public override void Interact(PlayerInteractor playerInteractor)
    {
        if (!CanBeInteracted) return;
        base.Interact(playerInteractor);

        CanBeInteracted = false;
        _isActive = !_isActive;
        CrystalRenderer.material.SetFloat(DISTORT_DISTANCE, 0.2f);

        CinemachineShake.Instance.SetTrauma(CrystalFixShake);

        foreach (var objective in CrystalFixObjectives)
        {
            if (objective.IsAdded && !objective.IsCompleted)
            {
                objective.CompleteObjective();
                return;
            }
        }
    }

    private void Update()
    {
        if (!_isActive)
        {
            var t = (Mathf.Sin(Time.time * InactiveColorSinSpeed) + 1) / 2;
            _currentColor = Color.Lerp(InactiveColor1, InactiveColor2, t);
        }
        else
        {
            _currentColor = ActiveColor;
        }

        CrystalRenderer.material.color = Mathfs.SmoothByHalfLife(CrystalRenderer.material.color, _currentColor, ChangeColorHalfTime);

        var pos = transform.position;
        pos.y = _startY + Mathf.Sin(Time.time * SineFloatSpeed) * SineFloatHeight;
        transform.position = pos;
    }
}

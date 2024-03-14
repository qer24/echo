using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerLookTilt : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs Input;
    [SerializeField] private CameraOffsetsController CameraOffsets;

    [Space]
    [SerializeField] private float HorizontalAimInfluence = 1f;
    [SerializeField] private float AimSmoothingHalfTime = 0.1f;

    [Space]
    [SerializeField] private float MaxOffset = 45f;

    private float _currentXOffset;
    private readonly CameraOffsetsController.RotationalOffset _tiltOffset = new();

    private void Start()
    {
        CameraOffsets.AddOffset(_tiltOffset);
    }

    private void Update()
    {
        var xOffset = HorizontalAimInfluence * Input.look.x / Time.deltaTime / 1000f;
        if (float.IsNaN(xOffset) || float.IsInfinity(xOffset))
        {
            xOffset = 0f;
        }

        _currentXOffset = Mathfs.SmoothByHalfLife(_currentXOffset, xOffset, AimSmoothingHalfTime);
        _currentXOffset = Mathf.Clamp(_currentXOffset, -MaxOffset, MaxOffset);

        var tilt = Quaternion.AngleAxis(_currentXOffset, Vector3.forward);
        _tiltOffset.Rotation = tilt;
    }
}

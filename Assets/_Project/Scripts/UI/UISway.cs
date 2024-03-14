using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class UISway : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs Input;
    [SerializeField] private Transform CameraOffsets;

    [Space]
    [SerializeField] private float CameraOffsetsInfluence = 1f;
    [SerializeField] private float HorizontalAimInfluence = 1f;
    [SerializeField] private float VerticalAimInfluence = 1f;
    [SerializeField] private float AimSmoothingHalfTime = 0.1f;

    [Space]
    [SerializeField] private float CameraOffsetsRotationInfluence = 1f;

    private Vector3 _startPos;
    private Vector3 _mousePos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        _mousePos = Mathfs.SmoothByHalfLife(_mousePos, MouseAim(), AimSmoothingHalfTime);

        transform.position = _startPos + _mousePos + CameraOffsets.localPosition * CameraOffsetsInfluence;
        transform.rotation = Quaternion.Slerp(Quaternion.identity, CameraOffsets.localRotation, CameraOffsetsRotationInfluence);
    }

    private Vector3 MouseAim()
    {
        if (Input.look == Vector2.zero) return Vector3.zero;

        var delta = Input.look / Time.deltaTime / 1000f;
        if (float.IsNaN(delta.x) || float.IsInfinity(delta.x) || float.IsNaN(delta.y) || float.IsInfinity(delta.y))
        {
            delta = Vector2.zero;
        }

        var xOffset = HorizontalAimInfluence * delta.x;
        var yOffset = VerticalAimInfluence * delta.y;

        return new Vector3(xOffset, yOffset, 0f);
    }
}

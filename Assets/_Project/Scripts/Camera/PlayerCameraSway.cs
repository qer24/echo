using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class PlayerCameraSway : MonoBehaviour
{
    [SerializeField] private CameraOffsetsController CameraOffsetsController;
    [SerializeField] private FirstPersonController PlayerController;

    [Space]
    [SerializeField] private float BobAmplitudeWalk = 0.2f;
    [SerializeField] private float BobFrequencyWalk = 1f;
    [SerializeField] private float BobAmplitudeRun = 0.2f;
    [SerializeField] private float BobFrequencyRun = 2f;
    [SerializeField] private float BobHalfTime = 0.3f;

    [Space]
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private float IdleAmplitude = 0.1f;
    [SerializeField] private float IdleFrequency = 0.5f;
    [SerializeField] private float WalkAmplitude = 0.1f;
    [SerializeField] private float WalkFrequency = 1f;
    [SerializeField] private float RunAmplitude = 0.2f;
    [SerializeField] private float RunFrequency = 2f;
    [SerializeField] private float NoiseHalfTime = 0.3f;

    [Space]
    [SerializeField] private float TiltMultiplier;
    [SerializeField] private float TiltHalfTime = 0.3f;
    [SerializeField] private float LeanMultiplier = 0.5f;
    [SerializeField] private float LeanHalfTime = 0.3f;

    private readonly CameraOffsetsController.PositionalOffset _runBob = new();
    private readonly CameraOffsetsController.RotationalOffset _runTilt = new();

    private CinemachineBasicMultiChannelPerlin _noise;

    private float _currentTilt;
    private float _currentLean;

    private void Start()
    {
        CameraOffsetsController.AddOffset(_runBob);
        CameraOffsetsController.AddOffset(_runTilt);

        _noise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        var targetBob = Vector3.zero;
        var targetTilt = 0f;
        var targetLean = 0f;

        if (PlayerController.Grounded)
        {
            var speed = PlayerController.Speed;
            if (speed < PlayerController.MoveSpeed * 0.25f)
            {
                SmoothNoise(IdleAmplitude, IdleFrequency);
            }
            else if (speed < PlayerController.SprintSpeed * 0.75f)
            {
                SmoothNoise(WalkAmplitude, WalkFrequency);
                targetBob = FootStepMotion(BobAmplitudeWalk, BobFrequencyWalk);
                targetTilt = FootstepTilt(BobAmplitudeWalk, BobFrequencyWalk);
                targetLean = FootstepLean(BobAmplitudeWalk, BobFrequencyWalk);
            }
            else
            {
                SmoothNoise(RunAmplitude, RunFrequency);
                targetBob = FootStepMotion(BobAmplitudeRun, BobFrequencyRun);
                targetTilt = FootstepTilt(BobAmplitudeRun, BobFrequencyRun);
                targetLean = FootstepLean(BobAmplitudeRun, BobFrequencyRun);
            }
        }

        _runBob.Position = Mathfs.SmoothByHalfLife(_runBob.Position, targetBob, BobHalfTime);

        _currentTilt = Mathfs.SmoothByHalfLife(_currentTilt, targetTilt, TiltHalfTime);
        _currentLean = Mathfs.SmoothByHalfLife(_currentLean, targetLean, LeanHalfTime);

        _runTilt.Rotation = Quaternion.AngleAxis(_currentTilt * TiltMultiplier, Vector3.forward) *
                            Quaternion.AngleAxis(_currentLean * LeanMultiplier, Vector3.right);
    }

    private void SmoothNoise(float amplitude, float frequency)
    {
        _noise.m_AmplitudeGain = Mathfs.SmoothByHalfLife(_noise.m_AmplitudeGain, amplitude, NoiseHalfTime);
        _noise.m_FrequencyGain = Mathfs.SmoothByHalfLife(_noise.m_FrequencyGain, frequency, NoiseHalfTime);
    }

    private static Vector3 FootStepMotion(float amplitude, float frequency)
    {
        var pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;

        return pos;
    }

    private static float FootstepTilt(float amplitude, float frequency)
    {
        return Mathf.Sin(Time.time * frequency) * amplitude;
    }

    private static float FootstepLean(float amplitude, float frequency)
    {
        return (Mathf.Sin(Time.time * frequency) + 1) / 2 * amplitude;
    }
}

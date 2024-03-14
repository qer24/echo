using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using StarterAssets;
using UnityEngine;

public class PlayerFootstepTracker : MonoBehaviour
{
    [SerializeField] private FirstPersonController PlayerController;
    [SerializeField] private SoundObject PlayerSoundObject;

    [SerializeField] private EventReference FootstepSound, FootstepRunSound;

    [Space]
    [SerializeField] private float FootstepFrequencyWalk = 0.5f;
    [SerializeField] private float FootstepFrequencyRun = 0.25f;
    [SerializeField] private float RunSpeedTreshold = 0.75f;
    [SerializeField] private float StepTime = 0.25f;

    [Space]
    [SerializeField] private float BaseRadius = 7.5f;
    [SerializeField] private float WalkRadius = 9f;
    [SerializeField] private float RunRadius = 12;
    [SerializeField] private float RadiusLerpHalfLife = 0.3f;

    private float _currentRadius;
    private float _footstepTimer;

    private bool _step;

    public event Action<bool> OnStep;

    private void Start()
    {
        _currentRadius = BaseRadius;

        PlayerController.OnJump += () => StartCoroutine(Footstep(true));
    }

    private void Update()
    {
        if (PlayerController.Grounded && PlayerController.Speed > 0.05f)
        {
            _footstepTimer += Time.deltaTime;

            var isRunning = PlayerController.Speed >= PlayerController.SprintSpeed * RunSpeedTreshold;

            if (isRunning && _footstepTimer > FootstepFrequencyRun || !isRunning && _footstepTimer > FootstepFrequencyWalk)
            {
                _footstepTimer = 0;
                StartCoroutine(Footstep(isRunning));
            }
        }

        if (!_step) _currentRadius = Mathfs.SmoothByHalfLife(_currentRadius, BaseRadius, RadiusLerpHalfLife);

        PlayerSoundObject.Radius = _currentRadius;
    }

    private IEnumerator Footstep(bool isRunning)
    {
        if (isRunning) FootstepRunSound.Play();
        else FootstepSound.Play();

        OnStep?.Invoke(isRunning);

        var radius = isRunning ? RunRadius : WalkRadius;

        _step = true;

        var timer = 0f;
        while (timer < StepTime)
        {
            timer += Time.deltaTime;
            _currentRadius = Mathfs.SmoothByHalfLife(_currentRadius, radius, StepTime / 2);
            yield return null;
        }

        _step = false;
    }
}

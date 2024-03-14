using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepUI : MonoBehaviour
{
    [SerializeField] private PlayerFootstepTracker PlayerFootsteps;
    [SerializeField] private SimpleSlider LoudnessSlider;
    [SerializeField] private MinMaxFloat WalkingAmount;
    [SerializeField] private MinMaxFloat RunningAmount;

    [Space]
    [SerializeField] private float LoundessLerpHalfLife = 0.3f;
    [SerializeField] private float LoudnessDecaySpeed = 5f;

    private float _currentLoudness;

    private void Start()
    {
        PlayerFootsteps.OnStep += OnStep;
    }

    private void OnStep(bool isRunning)
    {
        _currentLoudness = isRunning ? RunningAmount.RandomValue : WalkingAmount.RandomValue;
    }

    private void Update()
    {
        _currentLoudness -= Time.deltaTime * LoudnessDecaySpeed;
        _currentLoudness = Mathf.Clamp(_currentLoudness, 0f, 1f);

        LoudnessSlider.Value = Mathfs.SmoothByHalfLife(LoudnessSlider.Value, _currentLoudness, LoundessLerpHalfLife);
        LoudnessSlider.UpdateSize();
    }
}

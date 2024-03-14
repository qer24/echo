using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMicUI : MonoBehaviour
{
    [SerializeField] private PlayerMicInput PlayerMicInput;
    [SerializeField] private MinMaxFloat LoudnessRange;
    [SerializeField] private SimpleSlider LoudnessSlider;

    [Space]
    [SerializeField] private float LoundessLerpHalfLife = 0.3f;

    private float _currentLoudness;

    private void Update()
    {
        var loudness = Mathf.InverseLerp(LoudnessRange.x, LoudnessRange.y, PlayerMicInput.Loudness);
        _currentLoudness = Mathfs.SmoothByHalfLife(_currentLoudness, loudness, LoundessLerpHalfLife);

        LoudnessSlider.Value = _currentLoudness;
        LoudnessSlider.UpdateSize();
    }
}

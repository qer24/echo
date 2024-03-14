using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class SliderText : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private string textBefore, textAfter;
    [SerializeField] private float displayMultiplier = 1;
    [SerializeField] private RoundingType roundingType;
    
    private enum RoundingType { None, Whole, Decimal, TwoDecimal, ThreeDecimal }

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        slider.onValueChanged.AddListener(UpdateText);
        UpdateText(slider.value);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        var multipliedValue = value * displayMultiplier;

        string roundedValue = roundingType switch
        {
            RoundingType.Whole => $"{multipliedValue:F0}",
            RoundingType.Decimal => $"{multipliedValue:F1}",
            RoundingType.TwoDecimal => $"{multipliedValue:F2}",
            RoundingType.ThreeDecimal => $"{multipliedValue:F3}",
            _ => multipliedValue.ToString(CultureInfo.CurrentCulture)
        };

        text.text = $"{textBefore}{roundedValue}{textAfter}";
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSlider : MonoBehaviour
{
    [SerializeField] private RectTransform Fill;
    [SerializeField] private MinMaxFloat SizeRange;

    [Space]
    [Range(0f, 1f)] public float Value;

    private void OnValidate() => UpdateSize();

    public void UpdateSize()
    {
        if (Fill == null) return;

        Fill.sizeDelta = new Vector2(Mathf.Lerp(SizeRange.x, SizeRange.y, Value), Fill.sizeDelta.y);
    }
}

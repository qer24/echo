using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnHover : MonoBehaviour
{
    [SerializeField] private float ScaleMultiplier = 1.1f;
    [SerializeField] private AutoTween ScaleTween;

    public void ScaleUp()
    {
        ScaleTween.Play(transform.LeanScale(Vector3.one * ScaleMultiplier, ScaleTween.Time)).setIgnoreTimeScale(true);
    }

    public void ScaleDown()
    {
        ScaleTween.Play(transform.LeanScale(Vector3.one, ScaleTween.Time)).setIgnoreTimeScale(true);
    }
}

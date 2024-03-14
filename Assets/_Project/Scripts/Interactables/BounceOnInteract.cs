using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOnInteract : MonoBehaviour
{
    [SerializeField] private AutoTween BounceTween;
    [SerializeField] private float ScaleMultiplier = 1.2f;

    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void Bounce()
    {
        transform.localScale = _originalScale * ScaleMultiplier;
        BounceTween.Play(transform.LeanScale(_originalScale, BounceTween.Time));
    }
}

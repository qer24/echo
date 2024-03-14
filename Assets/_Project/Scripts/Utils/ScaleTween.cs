using System;
// using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ScaleTween : MonoBehaviour
{
    public float Time;
    public float Delay;

    [Space]
    [SerializeField] private LeanTweenType Type;
    [SerializeField] private Vector3 TweenStartScale = Vector3.zero;
    [SerializeField] private Vector3 TweenEndScale = Vector3.one;

    [Space]
    public bool ScaleOnEnable = true;
    [SerializeField] private bool IgnoreTimeScale;

    // [HideInEditorMode]
    public bool Scaling;

    private ScaleTween[] _children;

    private int _tweenId;

    private void Awake()
    {
        _children = GetComponentsInChildren<ScaleTween>();

        Scaling = false;
    }

    private void OnEnable()
    {
        if (ScaleOnEnable) Scale();
    }

    public void ScaleButton()
    {
        Scale();
    }

    public void Scale(bool withChildren = false, bool reverse = false, Action callback = null, float? delayOverrite = null)
    {
        if (!gameObject.activeSelf) return;

        transform.localScale = reverse ? TweenEndScale : TweenStartScale;
        LeanTween.cancel(_tweenId);

        Scaling = true;
        float finalDelay = delayOverrite ?? Delay;
        _tweenId = transform.LeanScale(reverse ? TweenStartScale : TweenEndScale, Time)
                            .setIgnoreTimeScale(IgnoreTimeScale)
                            .setDelay(finalDelay)
                            .setEase(Type)
                            .setOnComplete(() =>
                                {
                                    callback?.Invoke();
                                    Scaling = false;
                                }
                            )
                            .uniqueId;

        if (!withChildren) return;
        foreach (var child in _children) child.Scale();
    }

    public void Scale(float delayOverrite, bool withChildren = false)
    {
        transform.localScale = TweenStartScale;
        LeanTween.cancel(_tweenId);

        Scaling = true;
        _tweenId = transform.LeanScale(TweenEndScale, Time).setIgnoreTimeScale(IgnoreTimeScale).setDelay(delayOverrite).setEase(Type).setOnComplete(() => Scaling = false).uniqueId;

        if (!withChildren) return;
        foreach (var child in _children) child.Scale();
    }

    public void Cancel()
    {
        if (_tweenId == 0) return;

        LeanTween.cancel(_tweenId);
        Scaling = false;
        transform.localScale = TweenEndScale;
    }
}

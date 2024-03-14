using System;
// using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class AutoTween
{
    public float Time;
    public LeanTweenType Type;

    [SerializeField] private bool HasDelay;
    [SerializeField]
    // [ShowIf("HasDelay")]
    public float Delay;

    private int? _uniqueId;

    [NonSerialized] public bool AutoCancel = true;
    [NonSerialized] public bool AutoSetTime = true;

    public LTDescr Play(LTDescr tween)
    {
        if (AutoCancel) Cancel();
        var newTween = tween.setEase(Type);
        newTween.setTime(AutoSetTime ? Time : tween.time);
        if (HasDelay) newTween.setDelay(Delay);
        _uniqueId = newTween.uniqueId;

        return newTween;
    }

    public void Cancel()
    {
        if (_uniqueId != null) LeanTween.cancel(_uniqueId.Value);
    }

    public AutoTween Clone()
    {
        return (AutoTween)MemberwiseClone();
    }
}

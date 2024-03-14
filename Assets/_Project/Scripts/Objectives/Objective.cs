using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objective", menuName = "ScriptableObjects/Objective")]
public class Objective : ScriptableObject
{
    [TextArea] public string ObjectiveDescription;
    public float AppearDelay;
    public Objective[] NextObjectives;

    [NonSerialized] private bool _isAdded;
    [NonSerialized] private bool _isCompleted;

    public bool IsAdded => _isAdded;
    public bool IsCompleted => _isCompleted;

    public event Action<Objective> OnObjectiveAdded;
    public event Action<Objective> OnObjectiveCompleted;

    public void AddObjective()
    {
        if (_isAdded) return;
        _isAdded = true;

        OnObjectiveAdded?.Invoke(this);
    }

    public void CompleteObjective()
    {
        if (_isCompleted) return;
        _isCompleted = true;

        OnObjectiveCompleted?.Invoke(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTrigger;
    [SerializeField, TagField] private string TriggerTag;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TriggerTag)) return;

        OnTrigger?.Invoke();
    }
}

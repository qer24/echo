using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoSpawner : MonoBehaviour
{
    [SerializeField] private Objective[] SpawningObjectives;

    private void Awake()
    {
        foreach (var objective in SpawningObjectives)
        {
            objective.OnObjectiveCompleted += SpawnWendigo;
        }
    }

    private void SpawnWendigo(Objective _)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).parent = null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectivesUI : MonoBehaviour
{
    [SerializeField] private Objective StartingObjective;
    [SerializeField] private Objective EndingObjective;
    [SerializeField] private Objective[] AllObjectives;
    [SerializeField] private GameObject ObjectivePreset;
    [SerializeField] private AutoTween ObjectiveFadeoutTween;

    private readonly Dictionary<Objective, CanvasGroup> _objectives = new();

    private IEnumerator Start()
    {
        StartingObjective.OnObjectiveAdded += AddObjective;
        StartingObjective.OnObjectiveCompleted += OnObjectiveCompleted;

        EndingObjective.OnObjectiveAdded += AddObjective;
        EndingObjective.OnObjectiveCompleted += OnObjectiveCompleted;

        foreach (var objective in AllObjectives)
        {
            objective.OnObjectiveAdded += AddObjective;
            objective.OnObjectiveCompleted += OnObjectiveCompleted;
        }

        yield return new WaitForSeconds(2f);

        StartingObjective.AddObjective();
    }

    private void AddObjective(Objective objective)
    {
        StartCoroutine(AddObjectiveCoroutine(objective));
    }

    private IEnumerator AddObjectiveCoroutine(Objective objective)
    {
        var objectiveGameObject = Instantiate(ObjectivePreset, transform);
        _objectives.Add(objective, objectiveGameObject.GetComponent<CanvasGroup>());

        objectiveGameObject.SetActive(false);

        yield return new WaitForSeconds(objective.AppearDelay);

        var objectiveText = objectiveGameObject.GetComponent<ObjectiveTypewriterUI>();
        objectiveText.SetObjectiveDescription(objective.ObjectiveDescription);

        objectiveGameObject.SetActive(true);
        objectiveText.StartTyping();
    }

    private void OnObjectiveCompleted(Objective objective)
    {
        StartCoroutine(ObjectiveCompletedCoroutine(objective));

        if (AllObjectives.All(o => o.IsCompleted))
        {
            EndingObjective.AddObjective();
        }
    }

    private IEnumerator ObjectiveCompletedCoroutine(Objective objective)
    {
        _objectives[objective].gameObject.GetComponentInChildren<TextMeshProUGUI>().fontStyle |= FontStyles.Strikethrough;
        ObjectiveFadeoutTween.Play(_objectives[objective].LeanAlpha(0, ObjectiveFadeoutTween.Time));
        Destroy(_objectives[objective].gameObject, ObjectiveFadeoutTween.Time);
        _objectives.Remove(objective);

        if (objective.NextObjectives.Length == 0) yield break;

        foreach (var nextObjective in objective.NextObjectives)
        {
            nextObjective.AddObjective();
        }
    }
}

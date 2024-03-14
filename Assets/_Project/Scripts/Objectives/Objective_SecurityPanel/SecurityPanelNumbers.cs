using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityPanelNumbers : MonoBehaviour
{
    [SerializeField] private Material[] NumberMaterials;
    [SerializeField] private GameObject[] NumberObjects;
    [SerializeField] private GameObject[] SecurityPanelObjects;

    public readonly int[] CodeNumbers = new int[3];

    private void Start()
    {
        AssignNumbers();
    }

    private void AssignNumbers()
    {
        foreach (var numberObject in NumberObjects)
        {
            numberObject.SetActive(false);
        }
        var numberParent = NumberObjects.GetRandomElement();
        numberParent.SetActive(true);

        foreach (var securityPanel in SecurityPanelObjects)
        {
            securityPanel.SetActive(false);
        }
        SecurityPanelObjects.GetRandomElement().SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            CodeNumbers[i] = new Squirrel3().Range(0, 10); // 0-9
            //Debug.Log(CodeNumbers[i]);
        }

        var firstNumberRenderer = numberParent.transform.GetChild(0).GetComponent<MeshRenderer>();
        var secondNumberRenderer = numberParent.transform.GetChild(1).GetComponent<MeshRenderer>();
        var thirdNumberRenderer = numberParent.transform.GetChild(2).GetComponent<MeshRenderer>();

        firstNumberRenderer.material = NumberMaterials[CodeNumbers[0]];
        secondNumberRenderer.material = NumberMaterials[CodeNumbers[1]];
        thirdNumberRenderer.material = NumberMaterials[CodeNumbers[2]];
    }
}

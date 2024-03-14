using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SecurityPanel : MonoBehaviour
{
    [SerializeField] private SecurityPanelNumbers SecurityPanelNumbers;
    [SerializeField] private Renderer[] NumberRenderers; // 0 - left (first digit), 1 - middle, 2 - right
    [SerializeField] private Material[] NumberMaterials;

    [Space]
    [SerializeField] private Renderer ButtonsRenderer;
    [SerializeField] private Renderer DiodeRenderer;
    [SerializeField] private Material ButtonsDoneMaterial;
    [SerializeField] private Material DiodeDoneMaterial;
    [SerializeField] private GameObject ButtonColliders;

    [Space]
    [SerializeField] private EventReference CodeCorrectSound;

    [Space]
    [SerializeField] private Objective FindPanelObjective;
    [SerializeField] private Objective EnterCodeObjective;
    [SerializeField] private float FindRadius;
    [SerializeField] private Transform Player;

    private readonly List<int> _inputNumbers = new();

    private Camera _playerCam;

    private void Awake()
    {
        _playerCam = Camera.main;
        transform.SetParent(null);
    }

    private void Start()
    {
        ClearNumbers();
    }

    private void Update()
    {
        if (FindPanelObjective.IsCompleted) return;

        var distanceSqr = Vector3.SqrMagnitude(Player.position - transform.position);
        if (distanceSqr < FindRadius * FindRadius)
        {
            //cast a ray from camera to panel
            var ray = _playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out var hit, FindRadius))
            {
                if (hit.transform.root == transform)
                {
                    FindPanelObjective.CompleteObjective();
                }
            }
        }
    }

    public void InputNumber(int number)
    {
        if (_inputNumbers.Count >= 3) ClearNumbers();

        _inputNumbers.Add(number);
        ChangeNumbers();
    }

    public void SubmitCode()
    {
        if (_inputNumbers.Count == 3)
        {
            if (_inputNumbers[0] == SecurityPanelNumbers.CodeNumbers[0] &&
                _inputNumbers[1] == SecurityPanelNumbers.CodeNumbers[1] &&
                _inputNumbers[2] == SecurityPanelNumbers.CodeNumbers[2])
            {
                CodeCorrect();
            }
        }

        ClearNumbers();
    }

    private void ChangeNumbers()
    {
        for (int i = 0; i < _inputNumbers.Count; i++)
        {
            NumberRenderers[i].material = NumberMaterials[_inputNumbers[i]];
            NumberRenderers[i].enabled = true;
        }
    }

    private void ClearNumbers()
    {
        foreach (var numberRend in NumberRenderers)
        {
            numberRend.enabled = false;
        }
        _inputNumbers.Clear();
    }

    private void CodeCorrect()
    {
        //Debug.Log("Code correct!");

        ButtonsRenderer.material = ButtonsDoneMaterial;
        DiodeRenderer.material = DiodeDoneMaterial;
        ButtonColliders.SetActive(false);
        CodeCorrectSound.Play(transform.position);

        EnterCodeObjective.CompleteObjective();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, FindRadius);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
// using Sirenix.OdinInspector;
using UnityEngine;

public class BoilerRepairMinigame : MonoBehaviour
{
    [SerializeField] private PlayerInteractor PlayerInteractor;

    [Space]
    [SerializeField] private GameObject MinigameUI;
    [SerializeField] private RectTransform Cursor;
    [SerializeField] private MinMaxFloat CursorRangeX;
    [SerializeField] private float CursorMoveTime = 1f;
    [SerializeField] private Easings.EaseType CursorEaseType;
    [SerializeField] private float WinThreshold = 0.1f;

    [Space]
    [SerializeField] private EventReference StartSound;
    [SerializeField] private EventReference WinSound;
    [SerializeField] private EventReference LoseSound;

    private bool _isPlaying;
    private bool _canWin;

    public event Action<bool> OnRepair;

    private void Start()
    {
        PlayerInteractor.OnInteractInput += OnInteract;
    }

    private void OnInteract()
    {
        if (!_isPlaying) return;

        if (_canWin)
        {
            WinSound.Play(PlayerInteractor.transform.position);
        }
        else
        {
            LoseSound.Play(PlayerInteractor.transform.position);
        }

        OnRepair?.Invoke(_canWin);

        StopAllCoroutines();
        MinigameUI.SetActive(false);
        _isPlaying = false;
    }

    // [Button]
    public void StartMinigame()
    {
        if (_isPlaying) return;

        StartCoroutine(MinigameRoutine());
    }

    private IEnumerator MinigameRoutine()
    {
        StartSound.Play(PlayerInteractor.transform.position);

        MinigameUI.SetActive(true);
        _isPlaying = true;

        var timer = -Time.deltaTime;

        while (timer < CursorMoveTime)
        {
            timer += Time.deltaTime;
            var t = timer / CursorMoveTime;
            t = CursorEaseType.Ease(t);

            var x = Mathf.Lerp(CursorRangeX.x, CursorRangeX.y, t);
            _canWin = t > 0.5f - WinThreshold && t < 0.5f + WinThreshold;
            Cursor.anchoredPosition = new Vector2(x, 0f);

            yield return null;
        }

        MinigameUI.SetActive(false);
        _isPlaying = false;

        OnRepair?.Invoke(false);
    }
}

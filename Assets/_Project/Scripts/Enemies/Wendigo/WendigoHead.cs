using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoHead : MonoBehaviour
{
    [SerializeField] private MinMaxFloat TwitchFrequency;
    [SerializeField] private MinMaxFloat TwitchAmountZ;
    [SerializeField] private MinMaxFloat TwitchAmountX;
    [SerializeField] private float RotationSmoothing;

    private Transform _playerCam;

    private Quaternion _currentRotation;

    private Quaternion _lookAtRotation;
    private Quaternion _twitchRotation;

    private void Awake()
    {
        _playerCam = Camera.main.transform;
        _currentRotation = transform.rotation;
    }

    private void Start()
    {
        StartCoroutine(TwitchCoroutine());
    }

    private IEnumerator TwitchCoroutine()
    {
        while (true)
        {
            Twitch();
            yield return new WaitForSeconds(TwitchFrequency.RandomValue);
        }
    }

    private void Twitch()
    {
        var rng = new Squirrel3();

        var twitchSignZ = rng.Bool() ? 1 : -1;
        var twitchSignX = rng.Bool() ? 1 : -1;

        var twitchZ = TwitchAmountZ.RandomValue * twitchSignZ;
        var twitchX = TwitchAmountX.RandomValue * twitchSignX;

        _twitchRotation = Quaternion.Euler(twitchX, 0, twitchZ);
    }

    void LateUpdate()
    {
        _lookAtRotation = Quaternion.LookRotation(_playerCam.position - transform.position);
        _currentRotation = Quaternion.Slerp(_currentRotation, _lookAtRotation * _twitchRotation, RotationSmoothing * Time.deltaTime);
        transform.rotation = _currentRotation;
    }
}

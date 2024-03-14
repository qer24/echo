using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMicInput : MonoBehaviour
{
    [SerializeField] private int DeviceIndex = 0;
    [SerializeField] private float MicSensitivity = 1;

    [SerializeField] private float LoundessFetchFrequency = 0.33f;

    private AudioSource _audioSource;
    private string _device;

    public float Loudness { get; private set; }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        SetDeviceIndex(DeviceIndex);

        // make sure the latency is 0
        while (!(Microphone.GetPosition(_device) > 0))
        {

        }
        _audioSource.Play();

        InvokeRepeating(nameof(FetchLoudness), 0, LoundessFetchFrequency);
    }

    private void FetchLoudness()
    {
        float[] waveData = new float[1024];
        int micPosition = Microphone.GetPosition(_device) - (waveData.Length + 1);
        if (micPosition < 0) return;
        _audioSource.clip.GetData(waveData, micPosition);

        Loudness = waveData.Select(sample => sample * sample).Prepend(0).Max() * MicSensitivity;
    }

    public void SetMicSensitivity(float newSensitivity)
    {
        MicSensitivity = newSensitivity;
    }

    public void SetDeviceIndex(int newIndex)
    {
        DeviceIndex = newIndex;
        _device = Microphone.devices[DeviceIndex];
        _audioSource.clip = Microphone.Start(_device, true, 10, 44100);
        _audioSource.loop = true;
    }

    // private void OnGUI()
    // {
    //     GUI.Label(new Rect(10, 10, 200, 50), "MicLoudness: " + Loudness);
    // }
}

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private const string SfxBus = "Bus:/SFX";
    private const string AmbienceBus = "Bus:/Ambience";

    public void UpdateSound(float value)
    {
        RuntimeManager.GetBus(SfxBus).setVolume(value);
    }

    public void UpdateAmbience(float value)
    {
        RuntimeManager.GetBus(AmbienceBus).setVolume(value);
    }
}

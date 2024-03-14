using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MicrophoneDropdown : MonoBehaviour
{
    [SerializeField] private PlayerMicInput PlayerMicInput;

    private void Start()
    {
        var dropdown = GetComponent<TMPro.TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(Microphone.devices.ToList());
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        PlayerMicInput.SetDeviceIndex(index);
    }
}

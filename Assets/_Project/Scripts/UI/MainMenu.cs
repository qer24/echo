using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private const string SfxBus = "Bus:/SFX";

    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseState.IsPaused = false;

        RuntimeManager.GetBus(SfxBus).stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene()
    {
        SceneTransition.Instance.LoadScene(1);
    }
}

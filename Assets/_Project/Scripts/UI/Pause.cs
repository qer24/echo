using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs Input;
    [SerializeField] private GameObject PauseMenu;


    private void Start()
    {
        TogglePause(false);

        Input.OnPauseInput += TogglePause;
    }

    private void TogglePause()
    {
        TogglePause(!PauseState.IsPaused);
    }

    private void TogglePause(bool isPaused)
    {
        PauseState.IsPaused = isPaused;
        SetCursorLock(!PauseState.IsPaused);
        PauseMenu.SetActive(PauseState.IsPaused);

        Time.timeScale = PauseState.IsPaused ? 0 : 1;

        PauseState.OnPause?.Invoke(PauseState.IsPaused);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && !PauseState.IsPaused)
        {
            SetCursorLock(true);
        }
    }

    private void SetCursorLock(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }

    public void Resume()
    {
        TogglePause();
    }

    public void Quit()
    {
        SceneTransition.Instance.LoadScene(0);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PauseState
{
    public static bool IsPaused;

    public static Action<bool> OnPause;
}

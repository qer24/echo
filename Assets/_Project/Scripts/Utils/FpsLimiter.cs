using System.Collections;
using System.Collections.Generic;
// using Sirenix.OdinInspector;
using UnityEngine;

public class FpsLimiter : MonoBehaviour
{
    // [Button]
    public void SetFps(int fps = 60)
    {
        Application.targetFrameRate = fps;
    }
}

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class Screenshot
{
    [MenuItem("Tools/Screenshot/Game View %#&s")]
    public static void Grab()
    {
        ScreenCapture.CaptureScreenshot("Screenshot.png", 1);
    }
}
#endif

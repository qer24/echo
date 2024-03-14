using System;
using System.Collections;
using System.Collections.Generic;
// using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundObjectsTracker : MonoBehaviour
{
    [SerializeField] private SoundObject[] TrackedObjects;
    [SerializeField] private int MaxSoundObjects = 20;

    private Texture2D _texture;
    private static readonly int SOUND_OBJECTS = Shader.PropertyToID("_SoundObjects");

    #if UNITY_EDITOR
    private GlobalKeyword _debugKeyword;
    private bool _debugOnExit;
    #endif

    private void Start()
    {
        #if UNITY_EDITOR
        _debugKeyword = GlobalKeyword.Create("_REVEAL_DEBUG");
        _debugOnExit = Shader.IsKeywordEnabled(_debugKeyword);

        Shader.DisableKeyword(_debugKeyword);
        #endif

        _texture = new Texture2D(MaxSoundObjects, 1, TextureFormat.RGBAFloat, false, true)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
            anisoLevel = 0
        };

        var colors = new Color[MaxSoundObjects];
        for (int i = 0; i < MaxSoundObjects; i++)
        {
            colors[i] = new Color(0, 0, 0, 0);
        }
        _texture.SetPixels(colors);
        _texture.Apply();

        //InvokeRepeating(nameof(UpdateSoundObjects), 0, 0.1f);
    }

    #if UNITY_EDITOR
    private void OnDestroy()
    {
        Shader.SetKeyword(_debugKeyword, _debugOnExit);
    }

    // [Button]
    [ContextMenu("ToggleKeyword")]
    private void ToggleKeyword()
    {
        _debugKeyword = GlobalKeyword.Create("_REVEAL_DEBUG");
        if (Shader.IsKeywordEnabled(_debugKeyword))
        {
            Shader.DisableKeyword(_debugKeyword);
        }
        else
        {
            Shader.EnableKeyword(_debugKeyword);
        }
    }
    #endif

    private void LateUpdate()
    {
        UpdateSoundObjects();
    }

    private void UpdateSoundObjects()
    {
        var colors = new Color[MaxSoundObjects];
        for (int i = 0; i < TrackedObjects.Length; i++)
        {
            var trackedObject = TrackedObjects[i];
            var trackedTransform = trackedObject.transform;
            var trackedObjectColor = new Vector4(trackedTransform.position.x, trackedTransform.position.y, trackedTransform.position.z, trackedObject.Radius);
            colors[i] = trackedObjectColor;
        }

        _texture.SetPixels(colors);
        _texture.Apply();

        Shader.SetGlobalTexture(SOUND_OBJECTS, _texture);
    }
}

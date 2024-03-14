using System;
using FMOD.Studio;
using FMODUnity;
// using Sirenix.Utilities;
using UnityEngine;

public static class AudioManager
{

    /// <summary>Creates a 2D Oneshot Sound Instance</summary>
    public static void Play(this EventReference sound)
    {
        if (sound.IsNull) return;

        try
        {
            PlayPausableOneShot(sound);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Sound: {sound} + not found!");
            Debug.LogError(e);
        }
    }

    /// <summary>Creates a 3D Oneshot Sound Instance</summary>
    public static void Play(this EventReference sound, Vector3 position)
    {
        if (sound.IsNull) return;

        try
        {
            PlayPausableOneShot(sound, position);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Sound: {sound} + not found!");
            Debug.LogError(e);
        }
    }

    /// <summary>Creates a 3D Oneshot Sound Instance</summary>
    public static void PlayAttached(this EventReference sound, GameObject gameObject)
    {
        if (sound.IsNull) return;

        try
        {
            PlayPausableOneShotAttached(sound, gameObject);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Sound: {sound} + not found!");
            Debug.LogError(e);
        }
    }

    public static void Play(string soundName, Vector3 position = new())
    {
        if (string.IsNullOrWhiteSpace(soundName)) return;

        try
        {
            var instance = RuntimeManager.CreateInstance(soundName);
            instance.set3DAttributes(position.To3DAttributes());
            instance.start();
            instance.release();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Sound: {soundName} + not found!");
            Debug.LogError(e);
        }
    }

    private static void PlayPausableOneShot(EventReference reference, Vector3 position = new())
    {
        var instance = RuntimeManager.CreateInstance(reference.Guid);
        PauseState.OnPause += isPaused => instance.setPaused(isPaused);
        instance.set3DAttributes(position.To3DAttributes());
        instance.start();
        instance.release();
    }

    private static void PlayPausableOneShotAttached(EventReference reference, GameObject gameObject)
    {
        var instance = RuntimeManager.CreateInstance(reference.Guid);
        PauseState.OnPause += isPaused => instance.setPaused(isPaused);
        if (gameObject.TryGetComponent(out Rigidbody rb)) RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform, rb);
        else RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform);
        instance.start();
        instance.release();
    }

    public static EventInstance CreatePausableInstance(this EventReference sound)
    {
        if (sound.IsNull) return new EventInstance();

        try
        {
            var instance = RuntimeManager.CreateInstance(sound);
            PauseState.OnPause += isPaused => instance.setPaused(isPaused);

            return instance;
        }
        catch (Exception)
        {
            Debug.LogWarning($"Sound: {sound} + not found!");
            return new EventInstance();
        }
    }
}

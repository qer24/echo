using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMicTracker : MonoBehaviour
{
    [SerializeField] private PlayerMicInput MicInput;
    [SerializeField] private SoundObject MicSoundObject;
    [SerializeField] private Transform PlayerTransform;

    [Space]
    [SerializeField] private float SoundWaveMicThreshold = 0.04f;
    [SerializeField] private float SoundWaveRadius = 10f;
    [SerializeField] private float SoundWaveCooldown = 0.1f;

    [Space]
    [SerializeField] private float SoundWaveHeight = 1.375f;
    [SerializeField] private float SoundWaveTravelTime = 0.9f;
    [SerializeField] private AnimationCurve SoundWaveEaseCurve;
    [SerializeField] private float SoundWaveDissipateTime = 0.5f;
    [SerializeField] private Easings.EaseType SoundWaveDissipateEaseType = Easings.EaseType.Linear;

    [Space]
    [SerializeField] private float SoundWaveTravelDistance = 10f;
    [SerializeField] private LayerMask SoundWaveLayerMask;

    public event Action OnSoundWave;
    private bool _soundWave;

    private void Start()
    {
        MicSoundObject.transform.SetParent(null);
    }

    private void Update()
    {
        if (_soundWave) return;

        if (MicInput.Loudness > SoundWaveMicThreshold) //|| Keyboard.current.eKey.isPressed)
        {
            OnSoundWave?.Invoke();
            StartCoroutine(SoundWave());
        }
    }

    private IEnumerator SoundWave()
    {
        MicSoundObject.Radius = SoundWaveRadius;
        _soundWave = true;

        var startPosition = PlayerTransform.position + Vector3.up * SoundWaveHeight;
        MicSoundObject.transform.SetPositionAndRotation(startPosition, PlayerTransform.rotation);
        var endPosition = MicSoundObject.transform.position + MicSoundObject.transform.forward * SoundWaveTravelDistance;

        //Debug.DrawRay(MicSoundObject.transform.position, Vector3.up * 0.5f, Color.green, 10f);

        var timer = 0f;
        var moving = true;
        while (timer < SoundWaveTravelTime)
        {
            var progress = timer / SoundWaveTravelTime;

            var beforeMovePos = MicSoundObject.transform.position;
            var afterMovePos = Vector3.Lerp(startPosition, endPosition, progress);

            if (moving && Physics.Linecast(beforeMovePos, afterMovePos, out var hit, SoundWaveLayerMask))
            {
                //Debug.Log(hit.collider.name);
                moving = false;
            }
            else if (moving)
            {
                MicSoundObject.transform.position = afterMovePos;
            }

            MicSoundObject.Radius = SoundWaveEaseCurve.Evaluate(progress) * SoundWaveRadius;

            timer += Time.deltaTime;
            yield return null;
        }

        var radius = MicSoundObject.Radius;

        timer = 0f;
        while (timer < SoundWaveDissipateTime)
        {
            timer += Time.deltaTime;

            var progress = timer / SoundWaveDissipateTime;
            var easedProgress = SoundWaveDissipateEaseType.Ease(progress);
            MicSoundObject.Radius = Mathf.Lerp(radius, 0, easedProgress);

            yield return null;
        }

        MicSoundObject.Radius = 0;

        yield return new WaitForSeconds(SoundWaveCooldown);
        _soundWave = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class WendigoKillPlayer : MonoBehaviour
{
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private GameObject KillCamera;
    [SerializeField] private Transform NormalCamera;

    [Space]
    [SerializeField] private float StandDistance = 2f;
    [SerializeField] private AutoTween MoveTween;

    [Space]
    [SerializeField] private Animator Animator;
    [SerializeField] private float CrossFadeTime;

    [Space]
    [SerializeField] private Transform SoundSource;
    [SerializeField] private EventReference KillSound;

    [Space]
    [SerializeField] private float CameraFallDelay = 1.05f;
    [SerializeField] private float ShakeAmount = 0.75f;
    [SerializeField] private LayerMask CameraFallLayerMask;
    [SerializeField] private AutoTween CameraFallTween;
    [SerializeField] private GameObject KillVolume;

    [Space]
    [SerializeField] private float SceneTransitionDelay;

    public void Kill()
    {
        var playerDir = PlayerInput.transform.position - transform.position;
        playerDir.y = 0;
        transform.forward = playerDir.normalized;

        var pos = PlayerInput.transform.position - transform.forward * StandDistance;
        MoveTween.Play(transform.LeanMove(pos, MoveTween.Time));

        PlayerInput.enabled = false;

        KillCamera.transform.position = NormalCamera.position;
        KillCamera.transform.rotation = NormalCamera.rotation;
        KillCamera.SetActive(true);

        KillSound.Play(SoundSource.position);

        WaitUtils.Wait(CameraFallDelay, true, () =>
        {
            CinemachineShake.Instance.SetTrauma(ShakeAmount);

            var ray = new Ray(KillCamera.transform.position + Vector3.forward * new Squirrel3().Range(0.5f, 1f) + Vector3.right * new Squirrel3().Range(0.5f, 1f), Vector3.down);
            if (Physics.Raycast(ray, out var hit, 100f, CameraFallLayerMask))
            {
                CameraFallTween.Play(KillCamera.transform.LeanMove(hit.point + Vector3.up * 0.1f, CameraFallTween.Time));
            }

            KillVolume.SetActive(true);
        });

        WaitUtils.Wait(SceneTransitionDelay, true, () =>
        {
            SceneTransition.Instance.LoadScene(0);
        });

        Animator.CrossFade("Attack", CrossFadeTime);
    }
}

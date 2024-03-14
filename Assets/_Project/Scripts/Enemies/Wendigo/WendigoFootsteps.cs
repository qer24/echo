using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class WendigoFootsteps : MonoBehaviour
{
    [SerializeField] private EventReference FootstepSound;

    public void PlayFootstepSound(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight < 0.5f) return;

        FootstepSound.Play(transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

[RequireComponent(typeof(AudioSource))]
public class AudioElement : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioManager.AudioType type;
    public bool EntityListen;
    public NetworkIdentity networkIdentity;
    private bool wasPlaying;

    private void Awake()
    {
        if (type < 0)
            throw new System.Exception("Missing audio type at " + gameObject.name);
        wasPlaying = false;
    }

    private void Update()
    {
        if (EntityListen)
        {
            if (audioSource.isPlaying)
            {
                AudioManager.instance.AudioUpdate(gameObject, type, true);
                wasPlaying = true;
            }
            else if (wasPlaying)
            {
                AudioManager.instance.AudioUpdate(gameObject, type, false);
                wasPlaying = false;
            }
        }
    }
}

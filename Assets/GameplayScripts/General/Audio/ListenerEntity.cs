using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerEntity : MonoBehaviour
{
    public float soundIncreaseMultiplier = 1f;
    public float soundDecreaseMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.receiveAudio += Listen;
    }

    virtual public void Listen(Vector3 position, AudioManager.AudioType type, bool isPlaying) { }

    virtual public void StopListen(Vector3 position, AudioManager.AudioType type, bool isPlaying) { }
}

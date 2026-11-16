using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SleeperLogic : ListenerEntity
{
    public float distanceAffected;
    EnemyStruct SleeperStruct;
    bool toleranceIncreasing;

    private void Update()
    {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Docile"))
        {
            if (!toleranceIncreasing)
            {
                float sound = gameObject.GetComponent<Animator>().GetFloat("Tolerance") - Time.deltaTime * soundDecreaseMultiplier;
                if (sound < 0)
                    sound = 0;
                gameObject.GetComponent<Animator>().SetFloat("Tolerance", sound);
            }

            if (gameObject.GetComponent<Animator>().GetFloat("Tolerance") > 0)
            {
                float animProgress = gameObject.GetComponent<Animator>().GetFloat("Tolerance") / 2f;
                gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1 - animProgress, 1 - animProgress, 1);
            }
        }
    }

    public override void Listen(Vector3 position, AudioManager.AudioType type, bool isPlaying)
    {
        float distance = (transform.position - position).magnitude;
        float sound = (int)type * Time.deltaTime * soundIncreaseMultiplier * (distanceAffected / distance);
        sound += gameObject.GetComponent<Animator>().GetFloat("Tolerance");
        if (sound > 2.1)
            sound = 2.1f;
        gameObject.GetComponent<Animator>().SetFloat("Tolerance", sound);
        toleranceIncreasing = isPlaying;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestScript : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        Animator animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        if (animator)
        {
            Vector3 newPosition = transform.position;
            newPosition.x += animator.GetFloat("Runspeed") * Time.deltaTime;
            transform.position = newPosition;
        }
    }
}

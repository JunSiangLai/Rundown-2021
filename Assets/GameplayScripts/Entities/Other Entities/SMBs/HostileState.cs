using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HostileState : SceneLinkedSMB<SleeperLogic>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        Vector3 direction = Vector3.zero;
        float distance = float.MaxValue;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            float magnitude = (player.transform.position - animator.transform.position).magnitude;
            if (magnitude < distance)
            {
                distance = magnitude;
                direction = (player.transform.position - animator.transform.position).normalized;
            }
        }
        animator.transform.position += direction * Time.deltaTime;
        animator.transform.forward = direction;
    }

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}

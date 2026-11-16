using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<EntityStruct>(out EntityStruct entity))
        {
            entity.TakeDamage();
        }
    }
}

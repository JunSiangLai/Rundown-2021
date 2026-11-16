using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class EntityStruct : NetworkBehaviour
{
    [SyncVar]
    [SerializeField] public float health;

    float getHealth()
    {
        return health;
    }

    [Command(requiresAuthority = false)]
    virtual public void TakeDamage() { }
}

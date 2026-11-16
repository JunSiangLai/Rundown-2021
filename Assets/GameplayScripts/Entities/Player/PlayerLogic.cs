using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class PlayerLogic : EntityStruct
{
    Weapons weapon;

    private void Start()
    {
        if (!isServer)
            return;

        health = 200f;
    }

    public override void TakeDamage()
    {
        health -= 20 * Time.deltaTime;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fort : Enemy {

    public bool aimAtPlayer = true;

    protected override void AcquireTarget()
    {
        List<GameObject> targets = new List<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                if (players[i].transform.position.y <= transform.position.y)
                {
                    targets.Add(players[i]);
                }
            }
        }

        if (targets.Count == 1)
        {
            targetPlayer = targets[0];
        }
        else if (targets.Count > 1)
        {
            GetClosestPlayer(targets);
        }
        else
        {
            targetPlayer = null;
        }
    }

    protected override void FixedUpdate()
    {
        if (aimAtPlayer && targetPlayer != null)
        {
            transform.up = targetPlayer.transform.position - transform.position;
        }
        base.FixedUpdate();
    }
    protected override void AutoFire()
    {
        base.AutoFire();
    }
}

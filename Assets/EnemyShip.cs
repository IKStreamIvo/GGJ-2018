using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy {

    public bool moveTowardsPlayers = true;
    public float movementSpeed = 20f;


    protected override void AcquireTarget()
    {
        List<GameObject> targets = new List<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            targets.Add(players[i]);
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
        if (moveTowardsPlayers && targetPlayer != null)
        {
            transform.up = targetPlayer.transform.position - transform.position;
            transform.GetComponent<Rigidbody2D>().velocity = transform.up * movementSpeed;
        }
        base.FixedUpdate();
    }

    protected override void AutoFire()
    {
        base.AutoFire();
    }
}

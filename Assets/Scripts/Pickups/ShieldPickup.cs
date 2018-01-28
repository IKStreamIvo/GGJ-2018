using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : Pickup
{

    public float invincibiltyDuration = 4f;
    public override void getPickup(Ship playerShip)
    {
        Debug.Log("Shield collected");
        GameManager.instance.turnInvincibleOn(playerShip, invincibiltyDuration);
    }
}

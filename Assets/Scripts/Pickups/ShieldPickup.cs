using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : Pickup
{
    public float invincibiltyDuration = 4f;
    public GameObject defaultShieldPrefab;
    public override void getPickup(Ship playerShip)
    {
        Debug.Log("Shield collected");
        if (playerShip.ShieldPrefab == null)
        {
            if (this.defaultShieldPrefab == null) return;
            playerShip.ShieldPrefab = defaultShieldPrefab;
        }
        GameManager.instance.TurnInvincibleOn(playerShip, invincibiltyDuration);
    }
}

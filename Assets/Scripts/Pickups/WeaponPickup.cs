using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{

    public int updgradeDuration = 8;
    public override void getPickup(Ship playerShip)
    {
        Debug.Log("Weapon collected");
        GameManager.instance.TurnWeaponUpgradeOn(playerShip, updgradeDuration);
    }
}

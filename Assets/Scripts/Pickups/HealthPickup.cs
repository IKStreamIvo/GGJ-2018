using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup {

    [Range(0f,1f)]
    public float restoredHealthPercentage = 0.5f;

    public override void getPickup(Ship playerShip)
    {
        base.getPickup(playerShip);
        Debug.Log("Health restore");

        float healthAmount = GameManager.instance.maxTeamHealth * restoredHealthPercentage;
        if (GameManager.instance.currentTeamHealth + healthAmount >= GameManager.instance.maxTeamHealth)
        {
            healthAmount = GameManager.instance.maxTeamHealth - GameManager.instance.currentTeamHealth;
        }

        GameManager.instance.applyDamage(-healthAmount);
    }
}

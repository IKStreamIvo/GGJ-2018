using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : Pickup
{

    public int BonusScore = 1500;

    public override void getPickup(Ship playerShip)
    {
        Debug.Log("Score collected");
        GameManager.instance.AddScore(BonusScore);

    }
}

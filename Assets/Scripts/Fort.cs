using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fort : MonoBehaviour {

    public int health = 500;
    public int dpm = 50;

    GameObject[] players;

    GameObject targetPlayer;

    float cooldown;
	
	// Update is called once per frame
	void Update () {
        if (players == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        List<GameObject> targets = new List<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].transform.position.y <= transform.position.y)
            {
                targets.Add(players[i]);
            }
        }

        if (targets.Count == 1)
        {
            targetPlayer = targets[0];
        } else if (targets.Count > 1)
        {
            GetClosestPlayer(targets);
        } else
        {
            targetPlayer = null;
        }
	}

    void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            transform.right = targetPlayer.transform.position - transform.position;
        }
    }

    private void GetClosestPlayer(List<GameObject> targets)
    {
        float distanceTarget1 = Vector2.Distance(targets[0].transform.position, transform.position);
        float distanceTarget2 = Vector2.Distance(targets[1].transform.position, transform.position);

        if (distanceTarget1 < distanceTarget2)
        {
            targetPlayer = targets[0];
        } else
        {
            targetPlayer = targets[1];
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fort : MonoBehaviour {

    public int health = 500;
    public float fireRate = (50f/60f);

    public GameObject bullet;
    public Transform bulletSpawn;

    GameObject[] players;

    GameObject targetPlayer;

    float lastShot = 0f;

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
            transform.up = targetPlayer.transform.position - transform.position;
        }
        if (targetPlayer != null && Time.time > fireRate + lastShot)
        {
            Fire();
        }
    }

    private void GetClosestPlayer(List<GameObject> targets)
    {
        float distanceTarget1 = Vector2.Distance(targets[0].transform.position, transform.position);
        float distanceTarget2 = Vector2.Distance(targets[1].transform.position, transform.position);

        if (distanceTarget1 < distanceTarget2)
        {
            targetPlayer = targets[0];
        }
        else
        {
            targetPlayer = targets[1];
        }

    }

    void Fire()
    {
        if (bullet != null && bulletSpawn != null)
        {
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            lastShot = Time.time;
        }
        else
        {
            Debug.LogError("Fort is trying to fire, but it has no bullet assigned!");
        }
    }
}

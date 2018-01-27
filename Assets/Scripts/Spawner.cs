using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<GameObject> spawnableObstacles;
    public List<GameObject> spawnableForts;

    public float spawnDelay = 0f;
    public float spawnTime = 3f;

    public float obstacleVelocity = 3f;

    public List<Vector2> spawnPoints;
    public int targetLayer = 1;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnObstacles", spawnDelay, spawnTime);
	}

    void SpawnObstacles()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Vector2 spawnPoint = spawnPoints[i];
            GameObject obstacle = spawnableObstacles[Random.Range(0, spawnableObstacles.Count)];
            obstacle = Instantiate(obstacle, spawnPoint, Quaternion.Euler(0,0,0), transform);
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -obstacleVelocity);

            PlaceForts(obstacle);
        }
    }

    // Delete gameObjects that leave the screen
    void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }

    void PlaceForts(GameObject obstacle)
    {
        List<Transform> fortSpawns = new List<Transform>();
        for (int i = 0; i < obstacle.transform.childCount; i++)
        {
            Transform child = obstacle.transform.GetChild(i);
            if (child.tag == "Fort Spawn")
            {
                fortSpawns.Add(child);
            }
        }
        for (int i = 0; i < fortSpawns.Count; i++)
        {
            Vector2 spawnPoint = fortSpawns[i].position;
            Quaternion rot = Quaternion.Euler(0, -1, 0);

            GameObject fort = spawnableForts[Random.Range(0, spawnableForts.Count)];
            fort = Instantiate(fort, spawnPoint, rot, obstacle.transform);
        }
    }
}

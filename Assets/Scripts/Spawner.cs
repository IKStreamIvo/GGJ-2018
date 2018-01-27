using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour {

    public List<GameObject> spawnableObstacles;
    public List<GameObject> spawnableShips;
    public List<GameObject> spawnableForts;

    public List<Transform> obstacleSpawnPoints;
    public List<Transform> shipSpawnPoints;

    public int targetZ = 1;

    public bool enableObstacleSpawn = true;
    public float obstacleSpawnDelay = 0f;
    public float obstacleSpawnInterval = 3f;
    public float obstacleVelocity = 3f;
    [Range(0, 1f)]
    public float fortSpawnChance = 1f;

    public bool enableShipSpawn = true;
    public float shipSpawnDelay = 0f;
    public float shipSpawnInterval = 3f;

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnObstacles", obstacleSpawnDelay, obstacleSpawnInterval);
        InvokeRepeating("SpawnShips", shipSpawnDelay, shipSpawnInterval);
    }

    void SpawnObstacles()
    {
        if (!enableObstacleSpawn) return;
        List<GameObject> spawnableObstaclesPool = new List<GameObject>(spawnableObstacles);

        for (int i = 0; i < obstacleSpawnPoints.Count; i++)
        {
            Vector2 spawnPoint = obstacleSpawnPoints[i].position;
            int randomIndex = Random.Range(0, spawnableObstaclesPool.Count);
            GameObject obstacle = spawnableObstaclesPool[randomIndex];
            spawnableObstaclesPool.RemoveAt(randomIndex);
            obstacle = Instantiate(obstacle, spawnPoint, Quaternion.Euler(0,0,0), transform);
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -obstacleVelocity);

            PlaceForts(obstacle);
        }
    }

    void SpawnShips()
    {
        if (!enableShipSpawn) return;
        for (int i = 0; i < shipSpawnPoints.Count; i++)
        {
            Vector2 spawnPoint = shipSpawnPoints[i].position;
            GameObject ship = spawnableShips[Random.Range(0, spawnableShips.Count)];
            ship = Instantiate(ship, spawnPoint, Quaternion.Euler(Vector2.up), transform);
        }
    }

    // Delete gameObjects that leave the screen
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Player outside of destroy zone!");
            return;
        }
        Destroy(collision.gameObject);
    }

    void PlaceForts(GameObject obstacle)
    {
        List<Transform> fortSpawns = new List<Transform>();
        for (int i = 0; i < obstacle.transform.childCount; i++)
        {
            Transform child = obstacle.transform.GetChild(i);
            if (child.tag == "Fort Spawn" && Random.Range(0, 1f) < fortSpawnChance) 
            {
                fortSpawns.Add(child);
            }
        }
        for (int i = 0; i < fortSpawns.Count; i++)
        {
            Vector2 spawnPoint = fortSpawns[i].position;
            Quaternion rot = Quaternion.Euler(0, 0, 180);

            GameObject fort = spawnableForts[Random.Range(0, spawnableForts.Count)];
            fort = Instantiate(fort, spawnPoint, rot, obstacle.transform);
            Debug.Log(fort + " " + i);
        }
    }
}

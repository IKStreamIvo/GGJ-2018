using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health = 500f;
    public float fireRate = 75f;
    public float bulletSpeed = 3f;

    public GameObject explosionPrefab;

    //public float rotationSpeed = 5f;

    public bool disableAutoFire = false;
    public GameObject bullet;
    public List<Transform> bulletSpawns = new List<Transform>();
    public bool alternateFire;

    public int collisionDamage = 1;

    protected GameObject[] players;
    protected GameObject targetPlayer;

    protected float lastShot = 0f;
    protected int lastSpawn = 0;
    protected Rigidbody2D rb;
    public float damage = 50f;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (GameManager.instance.GameOver)
            return;

        if (players == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        AcquireTarget();
    }

    protected virtual void AcquireTarget() {}

    protected void GetClosestPlayer(List<GameObject> targets)
    {
        if (targets[0] == null && targets[1] == null)
            return;

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

    protected virtual void FixedUpdate()
    {
        if (!disableAutoFire && targetPlayer != null)
        {
            AutoFire();
        }
    }

    protected virtual void AutoFire()
    {
        if (bullet != null)
        {
            if (Time.time > (60f/fireRate) + lastShot)
            {
                if (!alternateFire)
                {
                    for (int i = 0; i < bulletSpawns.Count; i++)
                    {
                        if (gameObject.layer == 10)
                        {
                            //fort
                            AudioManager.instance.PlaySound(AudioManager.Sound.FortLaser);
                        }
                        else
                        {
                            //ship
                            AudioManager.instance.PlaySound(AudioManager.Sound.EnemyLaser);
                        }

                        Transform bulletSpawn = bulletSpawns[i];
                        GameObject bulletGO = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
                        bulletGO.GetComponent<Bullet>().damage = damage;
                        Rigidbody2D rbb = bulletGO.GetComponent<Rigidbody2D>();
                        rbb.velocity = bulletGO.transform.up * bulletSpeed;
                        lastShot = Time.time;
                    }
                } else
                {
                    if (gameObject.layer == 10)
                    {
                        //fort
                        AudioManager.instance.PlaySound(AudioManager.Sound.FortLaser);
                    }
                    else
                    {
                        //ship
                        AudioManager.instance.PlaySound(AudioManager.Sound.EnemyLaser);
                    }

                    lastSpawn = (lastSpawn + 1) % bulletSpawns.Count;
                    Transform bulletSpawn = bulletSpawns[lastSpawn];
                    GameObject bulletGO = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
                    bulletGO.GetComponent<Bullet>().damage = damage;
                    Rigidbody2D rbb = bulletGO.GetComponent<Rigidbody2D>();
                    rbb.velocity = bulletGO.transform.up * bulletSpeed;
                }
                lastShot = Time.time;
            }
        }
        else
        {
            Debug.LogError("Fort is trying to fire, but it has no bullet assigned!");
        }
    }

    protected virtual void ApplyDamage(float damage)
    {
        AudioManager.instance.PlaySound(AudioManager.Sound.TakeDamage2);
        health -= damage;
        if (health <= 0)
        {
            AudioManager.instance.PlaySound(AudioManager.Sound.EnemyExplode);
            GameManager.instance.Explosion(transform.position, explosionPrefab);
            Debug.Log(gameObject.name + " " + explosionPrefab);
            Destroy(transform.gameObject);
        }
    }

    /*private void OnDestroy()
    {
        Debug.LogError(gameObject.name + "Destroyed");
    }*/

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.applyDamage(collisionDamage);
            AudioManager.instance.PlaySound(AudioManager.Sound.EnemyExplode);
            GameManager.instance.Explosion(transform.position, explosionPrefab);
            Debug.Log(gameObject.name + " " + explosionPrefab);
            Destroy(transform.gameObject);
        } else if (collision.transform.CompareTag("Player Bullet"))
        {
            ApplyDamage(collision.transform.GetComponent<Bullet>().damage);
            Destroy(collision.transform.gameObject);
        }
    }
}

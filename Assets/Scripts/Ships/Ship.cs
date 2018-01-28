using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject ShieldPrefab;
    public float damage = 150f;
    public float shootingSpeed = (50f / 60f);
    public float moveSpeed = 3f;
    public float bulletSpeed = 5f;

    [Header("Powerups")]
    public float damageMultiplier = 1f;
    public float shootingSpeedMultiplier = 1f;
    public float speedMultiplier = 1f;

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public CircleCollider2D coll;
    [HideInInspector]
    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }
}

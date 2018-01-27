using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float speedMultiplier = 1f;

    public Rigidbody2D rb;
    public CircleCollider2D coll;
    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Debug.Log("-health (Bullet)");
            Destroy(collision.collider.gameObject);
        } else if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("-health (Ship)");
            Destroy(collision.collider.gameObject);
        }
    }
}

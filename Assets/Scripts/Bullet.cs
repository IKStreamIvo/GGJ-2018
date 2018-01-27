using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    public float speed = 4f;
    public float ttl = 10f;
    public float damage = 0;
    

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        Destroy(transform.gameObject, ttl);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.instance.applyDamage(damage);
            Destroy(transform.gameObject);
        }
    }
}

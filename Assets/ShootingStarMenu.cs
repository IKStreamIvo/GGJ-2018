using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStarMenu : MonoBehaviour {

    public float speed = 2f;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        GetComponent<Rigidbody2D>().velocity = -transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position = startPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = startPos;
    }
}

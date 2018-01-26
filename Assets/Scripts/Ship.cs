using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public bool player;

    public float speed = 2f;

    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float hor = 0f;
        float ver = 0f;
        if (player)
        {
            hor = Input.GetAxis("Player 1 Horizontal");
            ver = Input.GetAxis("Player 1 Vertical");
        }
        else
        {
            hor = Input.GetAxis("Player 2 Horizontal");
            ver = Input.GetAxis("Player 2 Vertical");
        }

        rb.velocity = new Vector2(hor, ver) * speed;
    }
}

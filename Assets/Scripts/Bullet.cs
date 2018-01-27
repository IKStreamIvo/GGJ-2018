using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    public float speed = 10f;
    public float damage = 100f;

	// Use this for initialization
	void Start () {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
	}
	
    /*void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player hit!");

        if (collision.transform.tag == "Player")
        {
            Destroy(transform.gameObject);
        }
    }*/
}

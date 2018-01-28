using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pickup : MonoBehaviour {


    public void Start()
    {
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.down * 2f;
    }
    public virtual void getPickup(Ship playerShip)
    {
        Debug.Log("Pickup collected");
        AudioManager.instance.PlaySound(AudioManager.Sound.ItemPickup);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            getPickup(collision.transform.gameObject.GetComponent<Ship>());
            Destroy(transform.gameObject);
        }
    }
}

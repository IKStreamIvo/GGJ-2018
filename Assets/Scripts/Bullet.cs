using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    public float damage = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(collision.collider.name.Contains("1"))
                AudioManager.instance.PlaySound(AudioManager.Sound.TakeDamage1);
            else
                AudioManager.instance.PlaySound(AudioManager.Sound.TakeDamage3);

            GameManager.instance.applyDamage(damage);
            Destroy(transform.gameObject);
        } else if (collision.collider.CompareTag("Obstacle"))
        {
            Destroy(transform.gameObject);
        }
    }
}

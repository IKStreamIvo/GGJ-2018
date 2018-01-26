using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public bool player;

    public float moveSpeed = 2f;
    public float rotateSpeed = 5f;

    public Rigidbody2D rb;

    private Vector2 movement;
    private float movHor;
    private float movVer;
    private float rotHor;
    private float rotVer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Position
        if (player)
        {
            movHor = Input.GetAxisRaw("P1MovHor");
            movVer = Input.GetAxisRaw("P1MovVer");
        }
        else
        {
            movHor = Input.GetAxisRaw("P2MovHor");
            movVer = Input.GetAxisRaw("P2MovVer");
        }

        //Rotation
        if (player)
        {
            rotHor = Input.GetAxisRaw("P1RotHor");
            rotVer = Input.GetAxisRaw("P1RotVer");
        }
        else
        {
            rotHor = Input.GetAxisRaw("P2RotHor");
            rotVer = Input.GetAxisRaw("P2RotVer");
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor, movVer).normalized * moveSpeed;

        if (rotHor != 0f || rotVer != 0f)
        {
            float angley = Mathf.Atan2(rotVer, rotHor) * Mathf.Rad2Deg;
            angley = (angley + 360f) % 360f;
            float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angley, rotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }

    private void OnGUI()
    {
        if (player) {
            GUILayout.Label("movHor: " + movHor + " movVer: " + movVer + "\nrotHor: " + rotHor + " rotVer: " + rotVer);
        }
    }
}

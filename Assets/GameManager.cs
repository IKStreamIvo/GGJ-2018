using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject Ship1Prefab;
    public GameObject Ship2Prefab;
    public Vector2 Ship1Spawn;
    public Vector2 Ship2Spawn;
    public GameObject ShipTetherPrefab;

    public float moveSpeed = 3f;
    public float rotateSpeed = 3f;

    public Ship ship1;
    public Ship ship2;
    public ShipTether tether;

    private Vector2 p1movement;
    private Vector2 p2movement;
    private Vector2 p1rotation;
    private Vector2 p2rotation;

    public float teleportDistance = 5f;
    public float fullyChargedValue = 3f;
    public float chargeSpeed = 1f;

    private float p1charge;
    private float p2charge;

    void Start ()
    {
        //Spawn players	
        ship1 = Instantiate(Ship1Prefab, Ship1Spawn, Quaternion.identity).GetComponent<Ship>();
        ship2 = Instantiate(Ship2Prefab, Ship2Spawn, Quaternion.identity).GetComponent<Ship>();

        //Spawn tether
        tether = Instantiate(ShipTetherPrefab).GetComponent<ShipTether>();

        tether.ship1 = ship1;
        tether.ship2 = ship2;
    }
	
	void Update ()
    {
        p1movement = new Vector2(Input.GetAxisRaw("P1MovHor"), Input.GetAxisRaw("P1MovVer"));
        p2movement = new Vector2(Input.GetAxisRaw("P2MovHor"), Input.GetAxisRaw("P2MovVer"));

        p1rotation = new Vector2(Input.GetAxisRaw("P1RotHor"), Input.GetAxisRaw("P1RotVer"));
        p2rotation = new Vector2(Input.GetAxisRaw("P2RotHor"), Input.GetAxisRaw("P2RotVer"));

        //Teleport
        if (tether.line.enabled)
        {
            ///Get input
            int p1tp = Mathf.CeilToInt(Input.GetAxisRaw("P1Teleport"));
            int p2tp = Mathf.CeilToInt(Input.GetAxisRaw("P2Teleport"));
            if (p1tp == 1)
            {
                p2charge = 0f;
                p1charge += chargeSpeed * Time.deltaTime;
            }
            else if (p2tp == 1)
            {
                p1charge = 0f;
                p2charge += chargeSpeed * Time.deltaTime;
            }
            else if (p1charge >= fullyChargedValue)
            {
                Debug.Log("Fully released key p1");
            }
            else if (p2charge >= fullyChargedValue)
            {

                Debug.Log("Fully released key p2");
            }
            else
            {
                p1charge = 0f;
                p2charge = 0f;
            }

            //Check if done
            if (p1charge >= fullyChargedValue)
            {
                //Teleport
                ///get direction
                float angle = ship1.transform.eulerAngles.z;
                Vector3 direction = new Vector3(Mathf.Sin(-Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
                ///cast that point for collisions
                //Physics2D.CircleCast();
                ///teleport
                ship1.transform.position = ship1.transform.position + direction * teleportDistance;
            }
            else if (p2charge >= fullyChargedValue)
            {
                //Teleport
            }
        }
        else
        {
            p1charge = 0f;
            p2charge = 0f;
        }
        
    }

    private void FixedUpdate()
    {
        ship1.rb.velocity = p1movement.normalized * moveSpeed;
        ship2.rb.velocity = p2movement.normalized * moveSpeed;

        if (p1rotation.x != 0f || p1rotation.y != 0f)
        {
            float angley = Mathf.Atan2(p1rotation.y, p1rotation.x) * Mathf.Rad2Deg;
            angley = (angley + 360f) % 360f;
            float angle = Mathf.LerpAngle(ship1.transform.rotation.eulerAngles.z, angley, rotateSpeed * Time.deltaTime);
            ship1.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        if (p2rotation.x != 0f || p2rotation.y != 0f)
        {
            float angley = Mathf.Atan2(p2rotation.y, p2rotation.x) * Mathf.Rad2Deg;
            angley = (angley + 360f) % 360f;
            float angle = Mathf.LerpAngle(ship2.transform.rotation.eulerAngles.z, angley, rotateSpeed * Time.deltaTime);
            ship2.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }
}

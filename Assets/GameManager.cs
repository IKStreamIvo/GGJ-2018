using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

    public GameObject Ship1Prefab;
    public GameObject Ship2Prefab;
    public Vector2 Ship1Spawn;
    public Vector2 Ship2Spawn;
    public GameObject ShipTetherPrefab;
    public GameObject bulletPrefab;

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

    public float fireRate = (50f / 60f);
    float lastShotp1 = 0f;
    float lastShotp2 = 0f;

    private float minX, minY, maxX, maxY;
    private BoxCollider2D stageBounds;

    bool playerIndexSet = false;
    List<PlayerIndex> playerIndex = new List<PlayerIndex>();
    List<GamePadState> state = new List<GamePadState>();
    List<GamePadState> prevState = new List<GamePadState>();

    void Start ()
    {
        stageBounds = GetComponent<BoxCollider2D>();
        //Spawn players	
        ship1 = Instantiate(Ship1Prefab, Ship1Spawn, Quaternion.identity).GetComponent<Ship>();
        ship2 = Instantiate(Ship2Prefab, Ship2Spawn, Quaternion.identity).GetComponent<Ship>();

        //Spawn tether
        tether = Instantiate(ShipTetherPrefab).GetComponent<ShipTether>();

        tether.ship1 = ship1;
        tether.ship2 = ship2;
    }

    void SetGameBounds()
    {
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(Camera.main.transform.position);
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1,1,Camera.main.transform.position.z));

        minX = bottomCorner.x;
        minY = bottomCorner.y;
        maxX = topCorner.x;
        maxY = topCorner.y;

        Transform shipTransform = ship1.transform;
        
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                shipTransform = ship2.transform;
            }

            Vector3 pos = shipTransform.position;
            // Horizontal contraint
            if (pos.x < minX) pos.x = minX;
            if (pos.x > maxX) pos.x = maxX;

            // vertical contraint-
            if (pos.y < minY) pos.y = minY;
            if (pos.y > maxY) pos.y = maxY;

            // Update position
            shipTransform.position = pos;
        }

        stageBounds.size = new Vector2(maxX - minX, maxY - minY);
    }
	
	void Update ()
    {
        SetGameBounds();
        
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
                ship1.animator.SetBool("IsCharging", true);
                ship2.animator.SetBool("IsCharging", false);
            }
            else if (p2tp == 1)
            {
                p1charge = 0f;
                p2charge += chargeSpeed * Time.deltaTime;
                ship2.animator.SetBool("IsCharging", true);
                ship1.animator.SetBool("IsCharging", false);
            }
            else if (p1charge >= fullyChargedValue)
            {
                //Teleport
                ///get direction
                float angle = ship1.transform.eulerAngles.z;
                Vector3 direction = new Vector3(Mathf.Sin(-Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
                Vector3 targetPos = ship1.transform.position + direction * teleportDistance;
                float shipSize = ship1.coll.radius;
                ///cast that point for collisions
                Collider2D hit = Physics2D.OverlapCircle(targetPos, shipSize - .2f);
                if(hit != null)
                {
                    targetPos = ship1.transform.position + direction * (Mathf.Abs(hit.Distance(ship1.coll).distance/2f));
                }
                ///teleport
                ship1.transform.position = targetPos;
                p1charge = 0f;
                ship1.animator.SetBool("IsCharging", false);
                ship1.animator.SetBool("FullyCharged", false);
            }
            else if (p2charge >= fullyChargedValue)
            {
                //Teleport
                ///get direction
                float angle = ship2.transform.eulerAngles.z;
                Vector3 direction = new Vector3(Mathf.Sin(-Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
                Vector3 targetPos = ship2.transform.position + direction * teleportDistance;
                float shipSize = ship2.coll.radius;
                ///cast that point for collisions
                Collider2D hit = Physics2D.OverlapCircle(targetPos, shipSize - .2f);
                if (hit != null)
                {
                    targetPos = ship2.transform.position + direction * (Mathf.Abs(hit.Distance(ship2.coll).distance / 2f));
                }
                ///teleport
                ship2.transform.position = targetPos;
                p2charge = 0f;
                ship2.animator.SetBool("IsCharging", false);
                ship2.animator.SetBool("FullyCharged", false);
            }
            else
            {
                ship1.animator.SetBool("IsCharging", false);
                ship2.animator.SetBool("IsCharging", false);
                p1charge = 0f;
                p2charge = 0f;
            }

            if (p1charge >= fullyChargedValue)
            {
                ship1.animator.SetBool("FullyCharged", true);
            }
            else if (p2charge >= fullyChargedValue)
            {
                ship2.animator.SetBool("FullyCharged", true);
            }
        }
        else
        {
            ship1.animator.SetBool("IsCharging", false);
            ship2.animator.SetBool("IsCharging", false);
            p1charge = 0f;
            p2charge = 0f;
        }
    }

    private void FixedUpdate()
    {
        //ControllerSetup();

        p1movement = new Vector2(Input.GetAxisRaw("JoyInputHor"), Input.GetAxisRaw("P1MovVer"));
        /*if(p1movement == new Vector2(0f, 0f))
        {
            try
            {
                p1movement = new Vector2(state[0].ThumbSticks.Left.X, state[0].ThumbSticks.Left.Y);
            }
            catch { }
        }*/
        p2movement = new Vector2(Input.GetAxisRaw("P2MovHor"), Input.GetAxisRaw("P2MovVer"));
        if (p2movement == new Vector2(0f, 0f))
        /*{
            try
            {
                Debug.Log(state[0].PacketNumber + "    " + state[1].PacketNumber);
                p2movement = new Vector2(state[1].ThumbSticks.Left.X, state[1].ThumbSticks.Left.Y);
            }
            catch { }
        }*/

        p1rotation = new Vector2(Input.GetAxisRaw("P1RotHor"), Input.GetAxisRaw("P1RotVer"));
        p2rotation = new Vector2(Input.GetAxisRaw("P2RotHor"), Input.GetAxisRaw("P2RotVer"));

        ship1.rb.velocity = p1movement.normalized * moveSpeed;
        ship2.rb.velocity = p2movement.normalized * moveSpeed;

        if (p1rotation.x != 0f || p1rotation.y != 0f)
        {
            float angley = Mathf.Atan2(p1rotation.y, p1rotation.x) * Mathf.Rad2Deg;
            angley = (angley + 360f) % 360f;
            float angle = Mathf.LerpAngle(ship1.transform.rotation.eulerAngles.z, angley, rotateSpeed * Time.fixedDeltaTime);
            ship1.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        if (p2rotation.x != 0f || p2rotation.y != 0f)
        {
            float angley = Mathf.Atan2(p2rotation.y, p2rotation.x) * Mathf.Rad2Deg;
            angley = (angley + 360f) % 360f;
            float angle = Mathf.LerpAngle(ship2.transform.rotation.eulerAngles.z, angley, rotateSpeed * Time.fixedDeltaTime);
            ship2.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }

        //Fire bullets
        if (Input.GetButton("P1Fire"))
        {
            if (bulletPrefab != null)
            {
                if (Time.time > fireRate + lastShotp1)
                {
                    Instantiate(bulletPrefab, ship1.transform.position + ship1.transform.forward * ship1.coll.radius, ship1.transform.rotation);
                    lastShotp1 = Time.time;
                }
            }
        }
        if (Input.GetButton("P2Fire"))
        {
            if (bulletPrefab != null)
            {
                if (Time.time > fireRate + lastShotp1)
                {
                    Instantiate(bulletPrefab, ship2.transform.position + ship2.transform.forward * ship2.coll.radius, ship2.transform.rotation);
                    lastShotp1 = Time.time;
                }
            }
        }
    }

    void ControllerSetup()
    {
        if (!playerIndexSet || !prevState[0].IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected && !playerIndex.Contains(testPlayerIndex))
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex.Add(testPlayerIndex);
                    playerIndexSet = true;
                }
            }
        }

        try
        {
            prevState[0] = state[0];
            state.Add(GamePad.GetState(playerIndex[0]));
            prevState[1] = state[1];
            state.Add(GamePad.GetState(playerIndex[1]));
        }
        catch { }
    }
}

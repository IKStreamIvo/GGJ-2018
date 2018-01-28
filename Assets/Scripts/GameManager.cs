using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool GameOver;

    public float maxTeamHealth = 1000f;
    float currentTeamHealth;

    public RectTransform healthBar;
    float healthScale;

    public GameUI gameUI;
    public GameObject explosionPrefab;
    public GameObject teleportPointer;

    public GameObject[] ShipPrefabs;
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

    private float p1charge = 0f;
    private float p2charge = 0f;

    public float fireRate = (50f / 60f);
    float lastShotp1 = 0f;
    float lastShotp2 = 0f;

    private float minX, minY, maxX, maxY;
    private BoxCollider2D stageBounds;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start ()
    {
        stageBounds = GetComponent<BoxCollider2D>();
        //Spawn players	
        ship1 = Instantiate(ShipPrefabs[SelectedShip.Ship1], Ship1Spawn, Quaternion.identity).GetComponent<Ship>();
        ship2 = Instantiate(ShipPrefabs[SelectedShip.Ship2], Ship2Spawn, Quaternion.identity).GetComponent<Ship>();

        //Spawn tether
        tether = Instantiate(ShipTetherPrefab).GetComponent<ShipTether>();

        tether.ship1 = ship1;
        tether.ship2 = ship2;
        currentTeamHealth = maxTeamHealth;
        healthScale = healthBar.sizeDelta.x / currentTeamHealth;
    }

    void SetGameBounds()
    {
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(Camera.main.transform.position);
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1,1,Camera.main.transform.position.z));

        minX = bottomCorner.x;
        minY = bottomCorner.y;
        maxX = topCorner.x;
        maxY = topCorner.y;

        if (ship1 != null)
        {
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
        }
        stageBounds.size = new Vector2(maxX - minX, maxY - minY);
    }

    bool p1tpDown;
    bool p1fullyCharged;
    bool p2tpDown;
    bool p2fullyCharged;

	void Update ()
    {
        SetGameBounds();

        if(tether.line == null)
        {
            return;
        }

        //Teleport
        if (tether.line.enabled)
        {
            float p1tp = Input.GetAxisRaw("P1Teleport");
            float p2tp = Input.GetAxisRaw("P2Teleport");

            //GetButton
            ///key down and was previously pressed?
            if (p1tp == 1 && p1tpDown)
            {
                float angle = ship1.transform.eulerAngles.z;
                Vector3 direction = new Vector3(Mathf.Sin(-Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
                Vector2 targetPos = ship1.transform.position + direction * teleportDistance;
                float shipSize = ship1.coll.radius;
                ///cast that point for collisions
                int layerMask = LayerMask.GetMask("Obstacles");
                Collider2D colls = Physics2D.OverlapCircle(targetPos, shipSize, layerMask);
                if (colls != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(ship1.transform.position, new Vector2(direction.x, direction.y), teleportDistance, layerMask);
                    if (hit.collider != null)
                    {
                        targetPos = new Vector3(hit.point.x, hit.point.y - shipSize);
                    }
                }
                teleportPointer.transform.position = targetPos;

                p1charge += chargeSpeed * Time.deltaTime;
                if (p1charge >= fullyChargedValue)
                {
                    if (!p1fullyCharged)
                        AudioManager.instance.PlaySound(AudioManager.Sound.Player1Charged);
                    p1fullyCharged = true;
                    ship1.animator.SetBool("FullyCharged", true);
                }
            }
            if (p2tp == 1 && p2tpDown)
            {
                teleportPointer.SetActive(true);
                float angle = ship2.transform.eulerAngles.z;
                Vector3 direction = new Vector3(Mathf.Sin(-Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
                Vector2 targetPos = ship2.transform.position + direction * teleportDistance;
                float shipSize = ship2.coll.radius;
                ///cast that point for collisions
                int layerMask = LayerMask.GetMask("Obstacles");
                Collider2D colls = Physics2D.OverlapCircle(targetPos, shipSize, layerMask);
                if (colls != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(ship2.transform.position, new Vector2(direction.x, direction.y), teleportDistance, layerMask);
                    if (hit.collider != null)
                    {
                        targetPos = new Vector3(hit.point.x, hit.point.y - shipSize);
                    }
                }
                teleportPointer.transform.position = targetPos;

                p2charge += chargeSpeed * Time.deltaTime;
                if (p2charge >= fullyChargedValue)
                {
                    if (!p2fullyCharged)
                        AudioManager.instance.PlaySound(AudioManager.Sound.Player2Charged);
                    p2fullyCharged = true;
                    ship2.animator.SetBool("FullyCharged", true);
                }
            }

            //GetButtonDown
            if (p1tp == 1 && !p2tpDown) //key down + other not charging
            {
                p1tpDown = true;
                ship1.animator.SetBool("IsCharging", true);

                teleportPointer.SetActive(true);
                teleportPointer.GetComponent<SpriteRenderer>().sprite = ship1.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            if (p2tp == 1 && !p1tpDown) //key down + other not charging
            {
                p2tpDown = true;
                ship2.animator.SetBool("IsCharging", true);

                teleportPointer.SetActive(true);
                teleportPointer.GetComponent<SpriteRenderer>().sprite = ship2.GetComponentInChildren<SpriteRenderer>().sprite;
            }

            //GetButtonUp
            if (p1tp == 0 && p1tpDown)
            {
                if (p1fullyCharged)
                {
                    //Teleport
                    ///get direction
                    float angle = ship1.transform.eulerAngles.z;
                    Vector3 direction = new Vector3(Mathf.Sin(-Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
                    Vector2 targetPos = ship1.transform.position + direction * teleportDistance;
                    float shipSize = ship1.coll.radius;
                    ///cast that point for collisions
                    int layerMask = LayerMask.GetMask("Obstacles");
                    Collider2D colls = Physics2D.OverlapCircle(targetPos, shipSize, layerMask);
                    if (colls != null)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(ship1.transform.position, new Vector2(direction.x, direction.y), teleportDistance, layerMask);
                        if (hit.collider != null)
                        {
                            targetPos = new Vector3(hit.point.x, hit.point.y - shipSize);
                        }
                    }
                    ///teleport
                    ship1.transform.position = targetPos;
                }
                p1tpDown = false;
                p1fullyCharged = false;
                p1charge = 0f;
                ship1.animator.SetBool("IsCharging", false);
                ship1.animator.SetBool("FullyCharged", false);
                teleportPointer.SetActive(false);

            }
            if (p2tp == 0 && p2tpDown)
            {
                if (p2fullyCharged)
                {
                    //Teleport
                    ///get direction
                    float angle = ship2.transform.eulerAngles.z;
                    Vector3 direction = new Vector3(Mathf.Sin(-Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
                    Vector2 targetPos = ship2.transform.position + direction * teleportDistance;
                    float shipSize = ship2.coll.radius;
                    ///cast that point for collisions
                    int layerMask = LayerMask.GetMask("Obstacles");
                    Collider2D colls = Physics2D.OverlapCircle(targetPos, shipSize, layerMask);
                    if (colls != null)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(ship2.transform.position, new Vector2(direction.x, direction.y), teleportDistance, layerMask);
                        if (hit.collider != null)
                        {
                            targetPos = new Vector3(hit.point.x, hit.point.y - shipSize);
                        }
                    }
                    ///teleport
                    ship2.transform.position = targetPos;
                }
                p2tpDown = false;
                p2fullyCharged = false;
                p2charge = 0f;
                ship2.animator.SetBool("IsCharging", false);
                ship2.animator.SetBool("FullyCharged", false);
                teleportPointer.SetActive(false);
            }
        }
        else
        {
            ship1.animator.SetBool("IsCharging", false);
            ship2.animator.SetBool("IsCharging", false);
            p1charge = 0f;
            p2charge = 0f;
            teleportPointer.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (GameOver)
            return;

        p1movement = new Vector2(Input.GetAxisRaw("P1MovHor"), Input.GetAxisRaw("P1MovVer"));
        p2movement = new Vector2(Input.GetAxisRaw("P2MovHor"), Input.GetAxisRaw("P2MovVer"));

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
        int p1fire = Mathf.CeilToInt(Input.GetAxis("P1Fire"));
        int p2fire = Mathf.CeilToInt(Input.GetAxis("P2Fire"));
        if (p1fire == 1)
        {
            if (ship1.BulletPrefab != null)
            {
                if (Time.time > fireRate + lastShotp1)
                {
                    AudioManager.instance.PlaySound(AudioManager.Sound.PlayerLaser1);

                    GameObject bullet = Instantiate(ship1.BulletPrefab, ship1.transform.position + ship1.transform.forward * ship1.coll.radius, ship1.transform.rotation);
                    bullet.GetComponent<Bullet>().damage = ship1.damage;
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.velocity = bullet.transform.up * ship1.bulletSpeed;
                    Destroy(bullet, 10);
                    lastShotp1 = Time.time;
                }
            }
        }
        if (p2fire == 1)
        {
            if (ship2.BulletPrefab != null)
            {
                if (Time.time > fireRate + lastShotp2)
                {
                    AudioManager.instance.PlaySound(AudioManager.Sound.PlayerLaser2);

                    GameObject bullet = Instantiate(ship2.BulletPrefab, ship2.transform.position + ship2.transform.forward * ship2.coll.radius, ship2.transform.rotation);
                    bullet.GetComponent<Bullet>().damage = ship2.damage;
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.velocity = bullet.transform.up * ship2.bulletSpeed;
                    Destroy(bullet, 10);
                    lastShotp2 = Time.time;
                }
            }
        }
    }

    public void applyDamage(float damage)
    {
        currentTeamHealth -= damage;
        if (currentTeamHealth <= 0)
        {
            AudioManager.instance.PlaySound(AudioManager.Sound.PlayerExplode);
            Explosion(ship1.transform.position);
            Explosion(ship2.transform.position);
            Destroy(ship1.gameObject);
            Destroy(ship2.gameObject);
            Destroy(tether.gameObject);
            GameOver = true;
            int score = 15;
            gameUI.GameOver(score);
        }
        healthBar.sizeDelta = new Vector2(currentTeamHealth * healthScale, healthBar.sizeDelta.y);
        healthBar.GetChild(0).GetComponent<Text>().text = (((float)currentTeamHealth/(float)maxTeamHealth) * 100f).ToString() + "%";
    }

    public void Explosion(Vector2 position, GameObject prefab = null)
    {
        if (prefab == null)
            prefab = explosionPrefab;
        Instantiate(prefab, position, Quaternion.identity, GameObject.Find("Background").transform.GetChild(1));
    }
}

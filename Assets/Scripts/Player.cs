using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] protected Transform turret;
    [SerializeField] protected float speed;
    [SerializeField] protected float boostSpeed;
    [SerializeField] protected int boostFuelCost;
    protected float defaultSpeed;
    [SerializeField] protected float rotationSpeed;
    protected Rigidbody2D rigidbody;
    
    [Header("Audio")]
    [SerializeField] protected AudioClip mgSfx;
    [SerializeField] protected AudioClip cannonSfx;
    [SerializeField] protected AudioClip deathSfx;

    [Header("Weapons")]
    [SerializeField] protected int maxGoreFuel;
    [SerializeField] protected int currentGoreFuel;
    [SerializeField] protected int collisionDamage;
    [SerializeField] protected Transform primaryFirePoint;
    [SerializeField] protected Transform secondaryFirePoint;
    [SerializeField] protected GameObject primaryProjectile;
    [SerializeField] protected GameObject secondaryProjectile;
    [SerializeField] protected float primaryFireDelay;
    [SerializeField] protected float secondaryFireDelay;
    [SerializeField] protected int primaryWeaponFuelCost;
    [SerializeField] protected int secondaryWeaponFuelCost;
    protected GameObject projectileParent;
    protected float primaryTimer;
    protected float secondaryTimer;
    protected float boostTimer;

    [Header("Death")] 
    [SerializeField] protected GameObject deathFx;
    [SerializeField] protected GameObject deathDecal;
    [SerializeField] protected float decalScale;
    [SerializeField] protected bool isAlive = true;

    public int CollisionDamage => collisionDamage;


    private void OnEnable()
    {
        GameEvents.AddFuel += AddFuel;
        GameEvents.PlayerKilled += Death;
    }

    private void OnDisable()
    {
        GameEvents.AddFuel -= AddFuel;
        GameEvents.PlayerKilled -= Death;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.SetFuel(maxGoreFuel);
        
        rigidbody = GetComponent<Rigidbody2D>();
        defaultSpeed = speed;

        projectileParent = new GameObject("Projectiles");
    }

    // Update is called once per frame
    void Update()
    {
        primaryTimer += Time.deltaTime;
        secondaryTimer += Time.deltaTime;
        boostTimer += Time.deltaTime;

        if (isAlive)
        {
            TurretRotation();
            SpeedBoost();
            FireCannon();
            FireAuxiliary();
        }
    }
    
    private void FixedUpdate()
    {
        if (isAlive)
        {
            TankMovement();
        }
    }

    private void SpeedBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentGoreFuel >= boostFuelCost && boostTimer >= 0.1f)
        {
            speed = boostSpeed;
            boostTimer = 0f;
            SubtractFuel(boostFuelCost);
        }
        else
        {
            speed = defaultSpeed;
        }
    }

    private void TurretRotation()
    {
        if (turret != null)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = point - turret.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            turret.eulerAngles = new Vector3(0, 0, angle - 90);
        }
    }

    private void TankMovement()
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rigidbody.velocity = (Vector2)transform.up * movementVector.y * speed * Time.fixedDeltaTime;
        rigidbody.MoveRotation(transform.rotation * quaternion.Euler(0,0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));
    }

    private void Death()
    {
        if (isAlive)
        {
            isAlive = false;
            speed = 0f;

            if (deathFx != null)
            {
                GameObject tempFx = Instantiate(deathFx, transform.position, Quaternion.identity);
            }
        
            if (deathDecal != null)
            {
                Vector3 fixedRotation = new Vector3(0, 90, -90);
                Vector3 correctedTransform = new Vector3(transform.position.x, transform.position.y, -1);
                
                GameObject decal = Instantiate(deathDecal, correctedTransform, Quaternion.identity);
                decal.transform.eulerAngles = fixedRotation;
                decal.transform.localScale *= decalScale;
            }
            
            GameEvents.PlaySfx(deathSfx);
        
            turret.gameObject.SetActive(false);

            GetComponent<Animator>().enabled = false;
            
            GameEvents.CameraShake(0.5f, 4.0f, 1.0f);
        }
    }

    private void FireCannon()
    {
        if (Input.GetButton("Fire1") && currentGoreFuel >= primaryWeaponFuelCost && primaryTimer >= primaryFireDelay)
        {
            SubtractFuel(primaryWeaponFuelCost);
            GameEvents.PlaySfx(cannonSfx);
            GameEvents.CameraShake(0.5f, 2.0f, 1.0f);

            GameObject tempPrimaryShot = Instantiate(primaryProjectile, primaryFirePoint.position, Quaternion.identity);
            tempPrimaryShot.transform.parent = projectileParent.transform;
            tempPrimaryShot.GetComponent<Projectile>().Direction = turret.transform.up;

            primaryTimer = 0f;
        }
    }
    
    private void FireAuxiliary()
    {
        if (Input.GetButton("Fire2") && currentGoreFuel >= secondaryWeaponFuelCost && currentGoreFuel > 0 && secondaryTimer >= secondaryFireDelay)
        {
            SubtractFuel(secondaryWeaponFuelCost);
            GameEvents.PlaySfx(mgSfx);
            GameEvents.CameraShake(0.2f, 0.5f, 1.0f);
            
            GameObject tempSecondaryShot = Instantiate(secondaryProjectile, secondaryFirePoint.position, Quaternion.identity);
            tempSecondaryShot.transform.parent = projectileParent.transform;
            tempSecondaryShot.GetComponent<Projectile>().Direction = turret.transform.up;

            secondaryTimer = 0f;
        }
    }

    private void AddFuel(int fuel)
    {
        if (currentGoreFuel <= maxGoreFuel)
        {
            currentGoreFuel += fuel;
        }
    }

    private void SubtractFuel(int fuel)
    {
        if (currentGoreFuel > 0 && (currentGoreFuel - fuel) >= 0)
        {
            currentGoreFuel -= fuel;
            GameEvents.SubtractFuel(fuel);
        }
    }
}

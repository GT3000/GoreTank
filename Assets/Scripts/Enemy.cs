using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyType
{
    Harmless,
    Aggressive,
    Ranged,
    Boss
}

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected AudioClip deathSfx;
    protected float defaultSpeed;
    [SerializeField] protected int score;
    [SerializeField] protected int fuelValue;
    [SerializeField] protected bool crushable;
    [SerializeField] protected bool randomMove;
    [SerializeField] protected GameObject gibbedModel;
    [SerializeField] protected GameObject deathFx;
    [SerializeField] protected List<GameObject> deathDecals;
    [SerializeField] protected float decalScale;
    protected Transform target;
    protected bool isDead;

    public bool Crushable => crushable;
    public EnemyType EnemyType => enemyType;

    // Start is called before the first frame update
    protected void Start()
    {
        target = FindObjectOfType<Player>().transform;
        
        defaultSpeed = speed;
        
        if (randomMove)
        {
            StartCoroutine(RandomMovement());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.gameObject.GetComponentInParent<Player>())
        {
            if (crushable && !isDead)
            {
                TakeDamage(col.gameObject.GetComponentInParent<Player>().CollisionDamage);
                GameEvents.CameraShake(0.5f, 1.5f, 1.0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.gameObject.GetComponentInParent<Player>())
        {
            if (crushable && !isDead)
            {
                TakeDamage(col.gameObject.GetComponentInParent<Player>().CollisionDamage);
                GameEvents.CameraShake(0.5f, 1.5f, 1.0f);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            if (damage >= health)
            {
                isDead = true;
                Death();
            }
            else if (damage < health)
            {
                health -= damage;

                if (health <= 0)
                {
                    isDead = true;
                    Death();
                }
            }
        }
    }

    private void Death()
    {
        if (enemyType == EnemyType.Aggressive || enemyType == EnemyType.Ranged)
        {
            GameEvents.EnemyKilled();
        }
        
        if (gibbedModel != null)
        {
            GameObject gibbedEnemy = Instantiate(gibbedModel, transform.position, Quaternion.identity);
            
            foreach (var gib in gibbedEnemy.GetComponentsInChildren<Gib>())
            {
                gib.FuelValue = fuelValue;
            }
        }

        if (deathFx != null)
        {
            GameObject tempFx = Instantiate(deathFx, transform.position, Quaternion.identity);
        }
        
        if (deathDecals != null)
        {
            int randomDecal = Random.Range(0, deathDecals.Count);
            Vector3 randomRotation = new Vector3(Random.Range(0, 360), 90, -90);
            Vector3 correctedTransform = new Vector3(transform.position.x, transform.position.y, -1);
            
            GameObject decal = Instantiate(deathDecals[randomDecal], correctedTransform, Quaternion.identity);
            decal.transform.eulerAngles = randomRotation;
            decal.transform.localScale *= decalScale;
        }

        GameEvents.ChangeScore(score);
        
        GameEvents.PlaySfx(deathSfx);
        
        Destroy(gameObject);
    }

    private IEnumerator RandomMovement()
    {
        int randomDirection = Random.Range(0, 3);
        Vector3 direction = Vector3.zero;

        switch (randomDirection)
        {
            case 0:
                direction = Vector3.up;
                break;
            case 1:
                direction = Vector3.right;
                break;
            case 2: 
                direction = Vector3.down;
                break;
            case 3:
                direction = Vector3.left;
                break;
        }
        
        transform.Translate(direction * speed * Time.deltaTime);

        yield return new WaitForSeconds(0.25f);

        StartCoroutine(RandomMovement());
    }
}

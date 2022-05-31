using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected bool primaryWeapon;
    [SerializeField] protected bool penetrates;
    [SerializeField] protected int numberOfEnemies;
    [SerializeField] protected bool enemyProjectile;
    protected Vector3 direction;
    protected int currentHit;
    protected float currentTimer;

    public int Damage => damage;
    public Vector3 Direction
    {
        set => direction = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        
        Move(direction);

        if (currentTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        
        if (col.CompareTag("Enemy") && col.GetComponent<Enemy>() != null && !enemyProjectile)
        {
            Enemy targetEnemy = col.GetComponent<Enemy>();

            if (targetEnemy.EnemyType == EnemyType.Harmless && primaryWeapon)
            {
                targetEnemy.TakeDamage(damage);
                //sail right through the enemy
            }
            else
            {
                targetEnemy.TakeDamage(damage);

                currentHit++;

                if (penetrates && currentHit > numberOfEnemies)
                {
                    Destroy(gameObject);
                }
            }
            
            if(!primaryWeapon && targetEnemy.EnemyType == EnemyType.Harmless )
            {
                targetEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if(!primaryWeapon)
            {
                targetEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (col.CompareTag("Player") && enemyProjectile)
        {
            //TODO Kill player
            GameEvents.PlayerKilled();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        
        if (col.gameObject.CompareTag("Enemy") && col.gameObject.GetComponent<Enemy>() != null && !enemyProjectile)
        {
            Enemy targetEnemy = col.gameObject.GetComponent<Enemy>();

            if (targetEnemy.EnemyType == EnemyType.Harmless && primaryWeapon)
            {
                targetEnemy.TakeDamage(damage);
                //sail right through the enemy
            }
            else
            {
                targetEnemy.TakeDamage(damage);

                currentHit++;

                if (penetrates && currentHit > numberOfEnemies)
                {
                    Destroy(gameObject);
                }
            }
            
            if(!primaryWeapon && targetEnemy.EnemyType == EnemyType.Harmless )
            {
                targetEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if(!primaryWeapon)
            {
                targetEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (col.gameObject.CompareTag("Player") && enemyProjectile)
        {
            //TODO Kill player
            GameEvents.PlayerKilled();
        }
    }
}

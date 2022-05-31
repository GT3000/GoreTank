using System.Collections;
using UnityEngine;

public class Ranged : Enemy
{
    [Header("Ranged")] 
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float fireRange;
    [SerializeField] protected float delayBeforeFire;
    [SerializeField] protected float delayAfterFire;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        StartCoroutine(RangedAttackBehavior());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator RangedAttackBehavior()
    {
        Vector3 targetDirection = target.position - transform.position;

        if (Vector3.Distance(transform.position, target.position) < fireRange)
        {
            yield return new WaitForSeconds(delayBeforeFire);
            
            //TODO Fire projectile at player
            FireProjectile(targetDirection);

            yield return new WaitForSeconds(delayAfterFire);
        }
        else
        {
            transform.Translate(targetDirection * speed * Time.deltaTime);
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }

        yield return null;

        StartCoroutine(RangedAttackBehavior());
    }

    private void FireProjectile(Vector3 direction)
    {
        GameObject floaterShot = Instantiate(projectile, transform.position, Quaternion.identity);
        //projectile.transform.parent = projectileParent.transform;
        floaterShot.GetComponent<Projectile>().Direction = direction;
    }
}

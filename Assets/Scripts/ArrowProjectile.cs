using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private Transform target;      
    private int damage;            
    public float speed = 12f;      
    public float hitDistance = 0.12f; 
    public float lifeTime = 5f;   

    float lifeTimer = 0f;

    public void SetTarget(Transform targetEnemy, int towerDamage)
    {
        target = targetEnemy;
        damage = towerDamage;
        lifeTimer = 0f;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        if (target == null)
        {
            
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position);
        float distanceThisFrame = speed * Time.deltaTime;

       
        if (direction.sqrMagnitude <= distanceThisFrame * distanceThisFrame)
        {
            HitTarget();
            return;
        }

        Vector2 move = direction.normalized * distanceThisFrame;
        transform.position = (Vector2)transform.position + move;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class CannonProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 8f;
    public float explosionRadius = 2f;
    public int damage = 30;
    public float lifetime = 3f;

    private Enemy targetEnemy;
    private Vector3 targetPosition;

    public void SetTarget(Enemy enemy)
    {
        targetEnemy = enemy;
        if (enemy != null)
        {
            targetPosition = enemy.transform.position;
        }
    }

    void Update()
    {
        if (targetEnemy == null)
        {
            MoveTowards(targetPosition);
            lifetime -= Time.deltaTime;
            if (lifetime <= 0f) Explode();
            return;
        }

        targetPosition = targetEnemy.transform.position;
        MoveTowards(targetPosition);
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            Explode();
        }
    }

    void Explode()
    {
        Debug.Log("Cannon exploded at" + transform.position);

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= explosionRadius)
            {
                enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

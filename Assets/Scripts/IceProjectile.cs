using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 6f;
    public float slowMultiplier = 0.5f;
    public float slowDuration = 2f;

    private Enemy target;

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.transform.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        float distanceThisFrame = speed * Time.deltaTime;
        if (Vector2.Distance(transform.position, target.transform.position) <= distanceThisFrame)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        target.ApplySlow(slowMultiplier, slowDuration);
        Debug.Log("Frost hit: " + target.name + " slowed for " + slowDuration + "s");

        Destroy(gameObject);
    }
}


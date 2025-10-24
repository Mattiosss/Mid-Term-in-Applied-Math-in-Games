using UnityEngine;
using System.Collections;

public class CurvedProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 8f;
    public float curveHeight = 1f;
    public float lifeTime = 5f;

    [Header("Damage / Effects")]
    public int damage = 0;
    public bool isCannon = false;
    public float explosionRadius = 2f;
    public bool isFrost = false;
    public float slowMultiplier = 0.5f;
    public float slowDuration = 2f;

    private Transform target;
    private Vector3 startPos;
    private Vector3 endPos;
    private float t = 0f;

    public void Launch(Transform targetEnemy, int projectileDamage = 0)
    {
        target = targetEnemy;
        startPos = transform.position;
        endPos = target != null ? target.position : transform.position + transform.right * 5f;
        t = 0f;

        if (!isCannon)
            damage = projectileDamage;

        if (isFrost) lifeTime = 5f;

        StartCoroutine(MoveCurve());
    }

    private IEnumerator MoveCurve()
    {
        float elapsedLife = 0f;

        while (t < 1f)
        {
            if (target == null && !isCannon)
            {
                Destroy(gameObject);
                yield break;
            }

            t += Time.deltaTime * speed / Vector3.Distance(startPos, endPos);
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * curveHeight;
            transform.position = currentPos;

            if (t < 1f)
            {
                Vector3 dir = (Vector3.Lerp(startPos, endPos, t + 0.01f) - currentPos).normalized;
                if (dir != Vector3.zero)
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }

            elapsedLife += Time.deltaTime;
            if (!isCannon && elapsedLife > lifeTime)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }

        if (isFrost && target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null) enemy.ApplySlow(slowMultiplier, slowDuration);
        }
        else if (isCannon)
        {
            Explode();
        }
        else if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= explosionRadius)
                enemy.TakeDamage(damage);
        }
    }
}

using UnityEngine;

public class FrostTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 5f;
    public float fireRate = 0.8f;

    [Header("References")]
    public Transform firePoint;                 
    public IceProjectile frostProjectilePrefab; 

    [Header("Audio")]
    public AudioClip frostShootSFX; 
    private AudioSource audioSource;

    private float fireCountdown = 0f;
    private Enemy targetEnemy;
    public float rotationSpeed = 5f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        UpdateTarget();

        if (targetEnemy == null)
            return;

        Vector3 direction = targetEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        targetEnemy = nearestEnemy;
    }

    void Shoot()
    {
        if (frostProjectilePrefab == null || firePoint == null || targetEnemy == null)
            return;

        IceProjectile newFrost = Instantiate(frostProjectilePrefab, firePoint.position, firePoint.rotation);
        newFrost.SetTarget(targetEnemy);

        if (frostShootSFX != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f); 
            audioSource.PlayOneShot(frostShootSFX);
        }

        Debug.Log($"{name} shot frost bolt at {targetEnemy.name}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

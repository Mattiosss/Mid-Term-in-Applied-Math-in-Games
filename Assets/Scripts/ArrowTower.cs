using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 5f;
    public float fireRate = 1f;
    public int damage = 15;

    [Header("References")]
    public Transform firePoint;                 
    public ArrowProjectile arrowPrefab;        

    [Header("Audio")]
    public AudioClip shootSFX; 
    private AudioSource audioSource;

    private float fireCountdown = 0f;
    private Transform targetEnemy;
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

        Vector3 direction = targetEnemy.position - transform.position;
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
        Transform nearestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        targetEnemy = nearestEnemy;
    }

    void Shoot()
    {
        if (arrowPrefab == null || firePoint == null || targetEnemy == null)
            return;

        ArrowProjectile newArrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        newArrow.SetTarget(targetEnemy, damage);

        // 🔊 Play shoot sound
        if (shootSFX != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f); 
            audioSource.PlayOneShot(shootSFX);
        }

        Debug.Log($"{name} shot arrow at {targetEnemy.name}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

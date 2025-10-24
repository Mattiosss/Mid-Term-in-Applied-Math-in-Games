using UnityEngine;

public class CannonTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 6f;
    public float fireRate = 1f;
    public float rotationSpeed = 180f;
    public float rotationOffset = 90f;

    [Header("Projectile Settings")]
    public CurvedProjectile cannonPrefab;
    public Transform firePoint;
    public int damage = 30;
    public float explosionRadius = 2f;
    public float projectileCurveHeight = 2f;

    [Header("Audio")]
    public AudioClip cannonShootSFX; 
    private AudioSource audioSource;

    private float fireCooldown = 0f;
    private Transform targetEnemy;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; 
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        targetEnemy = GetNearestEnemy();
        if (targetEnemy == null) return;

        RotateTowards(targetEnemy.position);

        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    void RotateTowards(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - rotationOffset;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            rotationSpeed * Time.deltaTime
        );
    }

    Transform GetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist && dist <= range)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }

    void Shoot()
    {
        if (cannonPrefab == null || firePoint == null || targetEnemy == null)
            return;

        CurvedProjectile proj = Instantiate(cannonPrefab, firePoint.position, firePoint.rotation);
        proj.speed = 8f;
        proj.curveHeight = projectileCurveHeight;
        proj.isCannon = true;
        proj.damage = damage;
        proj.explosionRadius = explosionRadius;
        proj.isFrost = false;
        proj.Launch(targetEnemy);

        if (cannonShootSFX != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(cannonShootSFX);
        }

        Debug.Log($"{name} fired cannonball at {targetEnemy.name}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

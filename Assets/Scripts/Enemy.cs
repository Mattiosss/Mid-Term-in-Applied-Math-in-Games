using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 50;
    public float moveSpeed = 2f;

    [Header("Path Following")]
    public Transform[] pathPoints;
    private int currentPoint = 0;

    private int currentHealth;
    private float currentSpeed;
    private float slowTimer = 0f;
    private float currentSlowMultiplier = 1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private EnemyHealthBar healthBar;

    [Header("Feedback")]
    public AudioClip hitSound;           
    public GameObject hitEffectPrefab;    
    private AudioSource audioSource;

    public int CurrentHealth => currentHealth;
    public System.Action OnEnemyDeath;

    void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = moveSpeed;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        audioSource = gameObject.AddComponent<AudioSource>();

        if (pathPoints != null && pathPoints.Length > 0)
        {
            transform.position = pathPoints[0].position;
            currentPoint = 1;
        }
        else
        {
            Debug.LogWarning("Enemy has no path points assigned!");
        }
    }

    void Update()
    {
        HandleSlowEffect();
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (pathPoints == null || pathPoints.Length == 0) return;
        if (currentPoint >= pathPoints.Length) return;

        Transform targetPoint = pathPoints[currentPoint];
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        transform.position += (Vector3)(direction * currentSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPoint++;
            if (currentPoint >= pathPoints.Length)
            {
                ReachEnd();
            }
        }
    }

    void HandleSlowEffect()
    {
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;

            if (slowTimer <= 0f)
            {
                currentSpeed = moveSpeed;
                currentSlowMultiplier = 1f;
                if (spriteRenderer != null)
                    spriteRenderer.color = originalColor;
            }
        }
    }

    public void ApplySlow(float multiplier, float duration)
    {
        if (multiplier >= currentSlowMultiplier && slowTimer > 0f)
            return;

        currentSlowMultiplier = multiplier;
        currentSpeed = moveSpeed * multiplier;
        slowTimer = duration;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.Lerp(originalColor, Color.cyan, 0.5f);

        Debug.Log($"{name} slowed: {multiplier * 100}% speed for {duration}s");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        if (healthBar != null)
            healthBar.ShowOnDamage();

        if (hitSound != null && audioSource != null)
            audioSource.PlayOneShot(hitSound);

        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        if (spriteRenderer != null)
            StartCoroutine(FlashColor());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator FlashColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddCoins(10);

        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }

    void ReachEnd()
    {
        Debug.Log(name + " reached the end!");
        GameManager.Instance?.TakeDamage(1);
        Destroy(gameObject);
    }
}

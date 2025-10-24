using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    public Slider healthSlider;
    public Canvas worldCanvas;
    [Header("Behavior")]
    public float fadeDelay = 2f;      
    public float fadeSpeed = 3f;      
    private Enemy enemy;
    private CanvasGroup canvasGroup;
    private float fadeTimer;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("EnemyHealthBar: No Enemy script found in parent!");
        }

        if (worldCanvas != null)
        {
            worldCanvas.worldCamera = Camera.main;
        }

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f; 
    }

    void Update()
    {
        if (enemy == null || healthSlider == null)
            return;

        float targetValue = (float)enemy.CurrentHealth / enemy.maxHealth;
        healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, Time.deltaTime * 10f);

        if (worldCanvas != null)
            worldCanvas.transform.rotation = Quaternion.identity;

        if (fadeTimer > 0f)
        {
            fadeTimer -= Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime * fadeSpeed);
        }
        else
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, Time.deltaTime * fadeSpeed);
        }
    }
    public void ShowOnDamage()
    {
        fadeTimer = fadeDelay;
    }
}

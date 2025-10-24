using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class TowerUpgradeManager : MonoBehaviour
{
    public static TowerUpgradeManager Instance;

    [Header("Upgrade Control")]
    private bool canUpgradeTowers = false;
    private bool upgradeMode = false;

    [Header("Upgrade Popup")]
    public GameObject upgradeTextPrefab; 

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (upgradeMode && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                TowerUpgrade tower = hit.collider.GetComponent<TowerUpgrade>();
                if (tower != null)
                {
                    tower.UpgradeTower();
                    DisableUpgradeMode();
                }
            }
        }
    }

    public void EnableUpgrades()
    {
        canUpgradeTowers = true;
        Debug.Log("Tower upgrades are now available!");
    }

    public bool CanUpgradeTowers()
    {
        return canUpgradeTowers;
    }

    public void EnableUpgradeMode()
    {
        if (!canUpgradeTowers)
        {
            Debug.Log("Cannot enter upgrade mode yet — upgrades not unlocked!");
            return;
        }

        upgradeMode = true;
        Debug.Log("Upgrade mode: Click a tower to upgrade it!");
    }

    public void DisableUpgradeMode()
    {
        upgradeMode = false;
        Debug.Log("Exited upgrade mode.");
    }

    public void ShowUpgradeText()
    {
        if (upgradeTextPrefab == null)
        {
            Debug.LogWarning("TowerUpgradeManager: No upgradeTextPrefab assigned!");
            return;
        }

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("TowerUpgradeManager: No Canvas found in the scene!");
            return;
        }

        GameObject textGO = Instantiate(upgradeTextPrefab, canvas.transform);

        TextMeshProUGUI tmpText = textGO.GetComponent<TextMeshProUGUI>();
        if (tmpText != null)
            tmpText.text = "+ Upgrade";

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        if (textRect != null)
        {
            textRect.anchoredPosition = Vector2.zero;
            textRect.localScale = Vector3.zero;
        }

        StartCoroutine(PopMoveFadeText(textGO, 0.5f, 1f, 50f));
    }

    private IEnumerator PopMoveFadeText(GameObject textGO, float delay, float fadeDuration, float moveDistance)
    {
        yield return new WaitForSeconds(delay);

        TextMeshProUGUI tmpText = textGO.GetComponent<TextMeshProUGUI>();
        RectTransform rect = textGO.GetComponent<RectTransform>();
        if (tmpText == null || rect == null) yield break;

        Color originalColor = tmpText.color;
        float timer = 0f;
        float popDuration = 0.3f;

        while (timer < popDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(0f, 1f, timer / popDuration);
            rect.localScale = Vector3.one * scale;
            yield return null;
        }
        rect.localScale = Vector3.one;

        timer = 0f;
        Vector2 startPos = rect.anchoredPosition;
        Vector2 targetPos = startPos + Vector2.up * moveDistance;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, timer / fadeDuration);
            yield return null;
        }

        Destroy(textGO);
    }
}

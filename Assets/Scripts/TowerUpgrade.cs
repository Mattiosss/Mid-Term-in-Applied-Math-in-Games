using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrade : MonoBehaviour
{
    [Header("Tower Stats")]
    public int level = 1;
    public int maxLevel = 10;
    public float damage = 10f;
    public float range = 3f;
    public float upgradeMultiplier = 1.25f;
    public int currentUpgradeCost = 20;
    public float costMultiplier = 1.5f;

    [Header("Upgrade Button (Scene UI)")]
    public Button upgradeButton;          
    public GameObject upgradeUIPrefab;    

    void Start()
    {
        if (upgradeUIPrefab != null)
        {
            upgradeButton = upgradeUIPrefab.GetComponentInChildren<Button>();

            if (upgradeButton != null)
            {
                upgradeButton.onClick.AddListener(UpgradeTower);
            }
            else
            {
                Debug.LogWarning("TowerUpgrade: No Button found in Upgrade UI!");
            }
        }
        else
        {
            Debug.LogWarning("TowerUpgrade: Upgrade UI reference not assigned!");
        }
    }

    public void UpgradeTower()
    {
        if (TowerUpgradeManager.Instance == null || !TowerUpgradeManager.Instance.CanUpgradeTowers())
        {
            Debug.Log("Upgrades are not available yet!");
            return;
        }

        if (level >= maxLevel)
        {
            Debug.Log(name + " is already at max level!");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("TowerUpgrade: No GameManager found!");
            return;
        }

        if (!GameManager.Instance.SpendCoins(currentUpgradeCost))
        {
            Debug.Log("Not enough coins to upgrade!");
            return;
        }

        level++;
        damage *= upgradeMultiplier;
        range *= 1.1f;

        Debug.Log($"{name} upgraded to Level {level}! Damage: {damage}, Range: {range}");

        if (TowerUpgradeManager.Instance != null)
        {
            TowerUpgradeManager.Instance.ShowUpgradeText();
        }

        currentUpgradeCost = Mathf.RoundToInt(currentUpgradeCost * costMultiplier);
    }

    public void HighlightUpgrade()
    {
        if (upgradeUIPrefab != null)
            upgradeUIPrefab.SetActive(true);
    }

    public void HideUpgrade()
    {
        if (upgradeUIPrefab != null)
            upgradeUIPrefab.SetActive(false);
    }
}

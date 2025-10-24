using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Tower Placement")]
    public TowerPlacement towerPlacement;

    [Header("Tower Prefabs")]
    public GameObject arrowTowerPrefab;
    public GameObject frostTowerPrefab;
    public GameObject cannonTowerPrefab;

    [Header("Currency System")]
    public int startingCoins = 50;
    public TextMeshProUGUI coinText;
    private int currentCoins;

    [Header("Player Health System")]
    public int startingHealth = 10;          
    public TextMeshProUGUI healthText;        
    private int currentHealth;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentCoins = startingCoins;
        currentHealth = startingHealth;
        UpdateCoinUI();
        UpdateHealthUI();
    }

    public void OnArrowTowerButtonClicked()
    {
        towerPlacement.StartPlacingTower(arrowTowerPrefab);
    }

    public void OnFrostTowerButtonClicked()
    {
        towerPlacement.StartPlacingTower(frostTowerPrefab);
    }

    public void OnCannonTowerButtonClicked()
    {
        towerPlacement.StartPlacingTower(cannonTowerPrefab);
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
        Debug.Log($"Coins added: +{amount}, Total = {currentCoins}");
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            UpdateCoinUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough coins!");
            return false;
        }
    }

    public int GetCurrentCoins()
    {
        return currentCoins;
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = $"Coins: {currentCoins}";
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log($"Player took {amount} damage! HP: {currentHealth}/{startingHealth}");

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("💀 Game Over!");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"HP: {currentHealth}";
    }
}

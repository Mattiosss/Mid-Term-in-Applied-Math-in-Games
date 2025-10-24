using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject goblinPrefab;
    public GameObject witchPrefab;
    public GameObject trollPrefab;
    public Transform spawnPoint;

    [Header("Wave Settings")]
    public float timeBetweenWaves = 5f;
    public float timeBetweenSpawns = 0.5f;
    public int totalWaves = 10;

    [Header("UI Settings")]
    public TextMeshProUGUI waveText;
    public float waveTextDisplayTime = 2f;

    private int currentWave = 1;

    void Start()
    {
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        while (currentWave <= totalWaves)
        {
            yield return StartCoroutine(ShowWaveText($"Wave {currentWave}"));

            switch (currentWave)
            {
                case 1:
                    yield return StartCoroutine(SpawnWave(goblinPrefab, 3));
                    break;

                case 2:
                    if (TowerUpgradeManager.Instance != null)
                        TowerUpgradeManager.Instance.EnableUpgrades();

                    yield return StartCoroutine(SpawnWave(goblinPrefab, 5));
                    break;

                case 3:
                    yield return StartCoroutine(SpawnWave(goblinPrefab, 7));
                    break;

                case 4:
                    yield return StartCoroutine(SpawnWave(goblinPrefab, 10));
                    break;

                case 5:
                    yield return StartCoroutine(SpawnWave(witchPrefab, 2));
                    break;

                case 6:
                    yield return StartCoroutine(SpawnWave(witchPrefab, 4));
                    break;

                case 7:
                    yield return StartCoroutine(SpawnWave(witchPrefab, 6));
                    break;

                case 8:
                    yield return StartCoroutine(SpawnWave(trollPrefab, 4));
                    break;

                case 9:
                    yield return StartCoroutine(SpawnWave(trollPrefab, 6));
                    break;

                case 10:
                    yield return StartCoroutine(SpawnWave(trollPrefab, 10));
                    break;
            }

            Debug.Log($"Wave {currentWave} completed!");
            currentWave++;

            if (currentWave <= totalWaves)
                yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("All waves completed!");
        yield return StartCoroutine(ShowWaveText("All Waves Completed!"));
    }

    IEnumerator SpawnWave(GameObject enemyPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log($"Spawning {enemyPrefab.name} #{i + 1} during Wave {currentWave}");

            EnemyPathFollower pathFollower = enemy.GetComponent<EnemyPathFollower>();
            if (pathFollower == null)
                pathFollower = enemy.AddComponent<EnemyPathFollower>();

            if (enemyPrefab == witchPrefab)
                pathFollower.speed = 1.5f;
            else if (enemyPrefab == trollPrefab)
                pathFollower.speed = 1.2f;
            else
                pathFollower.speed = 2f;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    IEnumerator ShowWaveText(string text)
    {
        if (waveText == null) yield break;

        waveText.text = text;
        waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveTextDisplayTime);
        waveText.gameObject.SetActive(false);
    }
}

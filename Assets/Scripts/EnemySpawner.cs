using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;   
    public Transform[] pathPoints;      
    public float spawnInterval = 2f;    

    private float spawnTimer = 0f;

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || pathPoints.Length == 0)
        {
            Debug.LogWarning("Spawner is missing enemies or path points!");
            return;
        }

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject chosenEnemy = enemyPrefabs[randomIndex];

        GameObject enemyObj = Instantiate(chosenEnemy, pathPoints[0].position, Quaternion.identity);

        Enemy enemyScript = enemyObj.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.pathPoints = pathPoints;
        }
    }
}


using System.Collections;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 4;
    public int enemiesToSpawn = 5;
    public float spawnRadius = 20f;
    public float spawnInterval = 5f;

    private int currentEnemyCount = 0;

    private void Start()
    {
        StartSpawning();
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, spawnRadius);
            int enemyCount = 0;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    enemyCount++;
                }
            }

            if (enemyCount < maxEnemies)
            {
                int enemiesToSpawnCount = Mathf.Min(enemiesToSpawn, maxEnemies - enemyCount);
                for (int i = 0; i < enemiesToSpawnCount; i++)
                {
                    Vector3 spawnPosition = GetRandomSpawnPosition();
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    currentEnemyCount++;
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomAngle = Random.Range(0f, 360f);
        Vector3 spawnPosition = transform.position + Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward * spawnRadius;
        return spawnPosition;
    }
}

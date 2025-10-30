using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using NavMeshPlus.Components;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float timeBetweenSpawn = 3f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private int maxAttempts = 10;
    [SerializeField] private float spawnRadius = 15f;

    [Header("NavMesh Settings")]
    [SerializeField] private NavMeshSurface surface;

    [Header("References")]
    [SerializeField] private Transform player;

    public float safeDistanceFromPlayer = 5f;

    private void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        if (player == null)
        {
            Debug.LogError("❌ EnemySpawner: Player not found! Hãy gán Player trong Inspector hoặc tag Player.");
        }
    }

    private bool FindValidSpawnPosition(out Vector3 position)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomPoint = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Vector3 testPosition = new Vector3(randomPoint.x, randomPoint.y, 0);

            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(testPosition, player.position);
                if (distanceToPlayer < safeDistanceFromPlayer)
                    continue;
            }

            NavMeshHit hit;
            if (NavMesh.SamplePosition(testPosition, out hit, 0.5f, NavMesh.AllAreas))
            {
                position = hit.position;
                return true;
            }
        }

        position = Vector3.zero;
        return false;
    }

    public IEnumerator SpawnRound(int enemyCount, float bonusHP)
    {
        int spawned = 0;

        while (spawned < enemyCount)
        {
            Vector3 spawnPos;
            if (FindValidSpawnPosition(out spawnPos))
            {
                GameObject chosenPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject enemy = Instantiate(chosenPrefab, spawnPos, Quaternion.identity);

                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    if (player != null)
                    {
                        enemyScript.player = player;
                        enemyScript.player2 = player.GetComponent<PlayerMovement>();
                    }

                    // tăng máu cho enemy theo round
                    enemyScript.maxHealth += bonusHP;
                    enemyScript.currentHealth = enemyScript.maxHealth;
                }

                spawned++;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
public class OnDestroyCallback : MonoBehaviour
{
    public System.Action OnDestroyed;
    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using NavMeshPlus.Components; // Bạn có dùng thư viện này

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private int maxAttempts = 10;
    [SerializeField] private float spawnInterval = 0.5f; // Thời gian giãn cách giữa mỗi lần spawn

    [Header("References")]
    [SerializeField] private Transform player;

    // Biến này sẽ được RoundManager đọc để biết khi nào hết quái
    public int currentEnemies { get; private set; } = 0;
    public float safeDistanceFromPlayer = 5f;

    private void Start()
    {
        // Tự động tìm player nếu chưa gán
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        // XÓA Coroutine tự động spawn. Giờ Spawner sẽ chờ lệnh.
        // StartCoroutine(SpawnLoop()); // <-- XÓA DÒNG NÀY
    }

    // Coroutine này được gọi bởi RoundManager
    public IEnumerator SpawnRound(int enemiesToSpawn, float bonusHP)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy(bonusHP);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy(float bonusHP) // <-- Thêm bonusHP
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        Vector3 spawnPos;
        if (FindValidSpawnPosition(out spawnPos))
        {
            GameObject chosenPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(chosenPrefab, spawnPos, Quaternion.identity);

            // Gán player & player2 và MÁU CỘNG THÊM
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null && player != null)
            {
                enemyScript.player = player;
                enemyScript.player2 = player.GetComponent<PlayerMovement>();
                enemyScript.Setup(bonusHP); // <-- GỌI HÀM SETUP HP
            }

            currentEnemies++;

            // Khi enemy bị hủy, giảm số lượng đếm
            enemy.AddComponent<OnDestroyCallback>().OnDestroyed += () => currentEnemies--;
        }
    }

    // Hàm FindValidSpawnPosition không thay đổi
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
                {
                    continue;
                }
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
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private  TextMeshProUGUI roundText; // Kéo từ Canvas vào

    [Header("Round Settings")]
    [SerializeField] private float timeBetweenRounds = 5f;
    [SerializeField] private int baseEnemiesPerRound = 5;
    [SerializeField] private float hpIncreasePerRound = 20f;
    [SerializeField] private float roundTextDisplayTime = 3f;

    private int currentRound = 0;
    private bool roundActive = false;

    private void Start()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<EnemySpawner>();
        }

        StartCoroutine(RoundLoop());
    }

    private IEnumerator RoundLoop()
    {
        while (true)
        {
            currentRound++;
            roundActive = true;

            int enemiesThisRound = baseEnemiesPerRound + (currentRound - 1) * 2;
            float bonusHP = (currentRound - 1) * hpIncreasePerRound;

            // ✅ Hiển thị text Round
            if (roundText != null)
            {
                roundText.gameObject.SetActive(true);
                roundText.text = $"ROUND {currentRound}";
                yield return new WaitForSeconds(roundTextDisplayTime);
                roundText.gameObject.SetActive(false);
            }

            Debug.Log($"--- ROUND {currentRound} START ---");
            Debug.Log($"Spawning {enemiesThisRound} enemies (HP +{bonusHP})");

            // Gọi spawner spawn enemy theo round
            yield return StartCoroutine(spawner.SpawnRound(enemiesThisRound, bonusHP));

            // Đợi đến khi không còn enemy nào
            while (FindObjectsOfType<Enemy>().Length > 0)
                yield return null;

            roundActive = false;
            Debug.Log($"Round {currentRound} completed!");

            yield return new WaitForSeconds(timeBetweenRounds);
        }
    }
}

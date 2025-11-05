using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // <-- Thêm thư viện này

public class RoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private TextMeshProUGUI roundText;

    [Header("Shop References")]
    [Tooltip("Kéo Panel chính của Shop (ví dụ: Head panel) vào đây")]
    [SerializeField] private GameObject shopCanvasObject;
    [Tooltip("Kéo GameObject chứa script ShopManager (ShoppingSlotManager) vào đây")]
    [SerializeField] private ShopManager shopManager;
    [Tooltip("Tạo một nút 'Continue' trên shop và kéo vào đây")]
    [SerializeField] private Button continueButton;

    [Header("Round Settings")]
    [SerializeField] private float timeBetweenRounds = 5f; // Sẽ là thời gian mua sắm
    [SerializeField] private int baseEnemiesPerRound = 5;
    [SerializeField] private float hpIncreasePerRound = 20f;
    [SerializeField] private float roundTextDisplayTime = 3f;

    [Header("Shop Settings")]
    [Tooltip("Thời gian chờ (giây) sau khi hết quái trước khi shop mở")]
    [SerializeField] private float timeBeforeShopOpens = 5f;

    private int currentRound = 0;
    private bool roundActive = false;

    private void Start()
    {
        // Tự động tìm components nếu chưa gán
        if (spawner == null) spawner = FindObjectOfType<EnemySpawner>();
        if (shopManager == null) shopManager = FindObjectOfType<ShopManager>();

        // Gán sự kiện cho nút "Continue"
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(EndShoppingPhase);
        }
        else
        {
            Debug.LogError("Nút 'Continue' chưa được gán trên RoundManager!");
        }

        // Ẩn shop khi bắt đầu
        if (shopCanvasObject != null)
            shopCanvasObject.SetActive(false);

        // Bắt đầu vòng lặp game
        StartCoroutine(RoundLoop());
    }

    private IEnumerator RoundLoop()
    {
        while (true)
        {
            // --- 1. BẮT ĐẦU ROUND ---
            currentRound++;
            roundActive = true;
            int enemiesThisRound = baseEnemiesPerRound + (currentRound - 1) * 2;
            float bonusHP = (currentRound - 1) * hpIncreasePerRound;

            // Hiển thị text "ROUND X"
            yield return StartCoroutine(ShowRoundText());

            // --- 2. SPAWN QUÁI ---
            Debug.Log($"--- ROUND {currentRound} START ---");
            Debug.Log($"Spawning {enemiesThisRound} enemies (HP +{bonusHP})");
            yield return StartCoroutine(spawner.SpawnRound(enemiesThisRound, bonusHP));

            // --- 3. CHỜ DIỆT HẾT QUÁI ---
            // Cách này hiệu quả hơn 'FindObjectsOfType' rất nhiều
            while (spawner.currentEnemies > 0)
            {
                yield return null;
            }

            // --- 4. KẾT THÚC ROUND / MỞ SHOP ---
            roundActive = false;
            Debug.Log($"Round {currentRound} completed!");
            yield return new WaitForSecondsRealtime(timeBeforeShopOpens);

            StartShoppingPhase();

            // --- 5. CHỜ NGƯỜI CHƠI MUA SẮM ---
            // Coroutine sẽ bị "đóng băng" ở đây cho đến khi EndShoppingPhase()
            // được gọi (bằng nút Continue) và set roundActive = true
            yield return new WaitUntil(() => roundActive == true);
        }
    }

    private IEnumerator ShowRoundText()
    {
        if (roundText != null)
        {
            roundText.gameObject.SetActive(true);
            roundText.text = $"ROUND {currentRound}";
            // Dùng WaitForSecondsRealtime để không bị ảnh hưởng bởi Time.timeScale
            yield return new WaitForSecondsRealtime(roundTextDisplayTime);
            roundText.gameObject.SetActive(false);
        }
    }

    private void StartShoppingPhase()
    {
        // 1. Dừng game
        Time.timeScale = 0f;

        // 2. Bật UI Shop
        if (shopCanvasObject != null)
            shopCanvasObject.SetActive(true);

        // 3. Random item mới
        if (shopManager != null)
            shopManager.GenerateShopItems();
    }

    // Hàm này được gọi bởi 'continueButton'
    public void EndShoppingPhase()
    {
        // 1. Tắt UI Shop
        if (shopCanvasObject != null)
            shopCanvasObject.SetActive(false);

        // 2. Chạy lại game
        Time.timeScale = 1f;

        // 3. Đặt cờ 'roundActive' thành true để Coroutine RoundLoop chạy tiếp
        roundActive = true;
    }
}

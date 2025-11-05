using UnityEngine;
using UnityEngine.UI;
using TMPro; // Có thể cần nếu bạn dùng TextMeshPro cho thanh HP

public class PlayerStats : MonoBehaviour
{
    // Singleton (Giống PlayerCurrency) để các script khác dễ dàng truy cập
    public static PlayerStats Instance;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private Image healthBar; // Kéo thanh HP UI vào đây
    [SerializeField] private TextMeshProUGUI hpText; // <-- THÊM MỚI (Kéo HP_Text vào đây)
    [SerializeField] private GameObject gameOverScreen; // Kéo màn hình "Game Over" vào

    [Header("Base Combat Stats")]
    [Tooltip("Sát thương gốc của Player (sẽ được súng đọc)")]
    [SerializeField] private float baseDamage = 10f;

    [Tooltip("Tốc độ đánh gốc (1.0 = 100% speed)")]
    [SerializeField] private float baseAttackSpeed = 1.0f;

    [Tooltip("Tốc độ di chuyển gốc (1.0 = 100% speed)")]
    [SerializeField] private float baseMoveSpeed = 1.0f; // <-- THÊM MỚI

    [Header("Bonus Stats (Từ Items)")]
    // Các script khác (như PlayerInventory) sẽ cộng dồn vào đây
    public float bonusMaxHealth = 0f; // <-- THÊM MỚI
    public float bonusDamageFlat = 0f;
    public float bonusAttackSpeedPercent = 0f; // 0.1 = +10a%
    public float bonusMoveSpeedPercent = 0f; // <-- THÊM MỚI

    public bool isDead = false;

    private void Awake()
    {
        // Thiết lập Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHPBar(); // Hàm này giờ sẽ cập nhật cả text

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Ẩn màn hình Game Over khi bắt đầu
        }
        // Time.timeScale = 1f; // (Nếu bạn dừng game khi chết)
    }

    // --- Các hàm Lấy Chỉ số ---

    // Súng sẽ gọi hàm này để biết sát thương cuối cùng
    public float GetTotalDamage()
    {
        return baseDamage + bonusDamageFlat;
    }

    // Súng sẽ gọi hàm này để biết tốc độ đánh
    // (1.0 = 100%, 1.1 = 110%)
    public float GetTotalAttackSpeedMultiplier()
    {
        return baseAttackSpeed + bonusAttackSpeedPercent;
    }

    // PlayerMovement sẽ gọi hàm này để biết tốc độ di chuyển
    public float GetTotalMoveSpeedMultiplier() // <-- THÊM MỚI
    {
        // Trả về 1.0 (gốc) + 0.1 (bonus 10%) = 1.1
        return baseMoveSpeed + bonusMoveSpeedPercent;
    }

    // --- Các hàm Thêm Chỉ số (Item sẽ gọi) ---
    // (Đây là ví dụ, bạn có thể gọi chúng từ PlayerInventory)
    public void AddDamage(float amount)
    {
        bonusDamageFlat += amount;
    }

    public void AddAttackSpeed(float percent) // Gửi 0.1 cho 10%
    {
        bonusAttackSpeedPercent += percent;
    }

    public void AddSpeed(float percent) // Gửi 0.1 cho 10% // <-- THÊM MỚI
    {
        bonusMoveSpeedPercent += percent;
    }

    public void AddMaxHealth(float amount)
    {
        bonusMaxHealth += amount;
        currentHealth += amount; // Hồi lại số máu vừa được cộng
        UpdateHPBar(); // Cập nhật UI ngay lập tức
    }

    // --- Các phương thức giống Enemy ---


    public float GetTotalMaxHealth() // <-- Bạn cũng cần hàm này
    {
        return maxHealth + bonusMaxHealth;
    }

    // --- CÁC HÀM MỚI DÙNG CHO HIỂN THỊ UI ---

    public string GetMaxHealthDisplay()
    {
        // "F0" = làm tròn, không có số thập phân
        return GetTotalMaxHealth().ToString("F0");
    }

    public string GetDamageDisplay()
    {
        return GetTotalDamage().ToString("F0");
    }

    public string GetMoveSpeedDisplay()
    {
        // Giả sử baseMoveSpeed của bạn là 1.0 (100%)
        // Hàm này sẽ hiển thị bonus %
        return GetTotalMoveSpeedMultiplier().ToString("F0");
    }

    public string GetAttackSpeedDisplay()
    {
        // Hiển thị bonus %
        // Ví dụ: 0.1 -> "+10%"
        float percent = GetTotalAttackSpeedMultiplier() * 100f;
        if (percent > 0)
        {
            return $"+{percent.ToString("F0")}%";
        }
        return "0%";
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Không nhận sát thương nếu đã chết

        if (PlayerHitFeedback.Instance != null)
        {
            PlayerHitFeedback.Instance.PlayHitEffect();
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Không cho máu < 0

        UpdateHPBar(); // Hàm này sẽ cập nhật cả thanh fill và text

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player đã chết!");

        // Dừng di chuyển
        GetComponent<PlayerMovement>().enabled = false;
        // (Tắt các script súng ở đây)
        // ...

        // Hiển thị màn hình Game Over
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Dừng game (tùy chọn)
        Time.timeScale = 0f;
    }

    private void UpdateHPBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        // --- THÊM MỚI: Cập nhật Text ---
        if (hpText != null)
        {
            // Dùng "F0" để làm tròn (ví dụ 99.5 -> 100), không hiển thị số thập phân
            hpText.text = $"{currentHealth.ToString("F0")} / {maxHealth.ToString("F0")}";
        }
    }
}


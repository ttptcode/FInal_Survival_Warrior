using UnityEngine;
using TMPro;

// Gắn script này vào Panel "Stats"
public class StatsDisplayUIPlayer : MonoBehaviour
{
    [Header("Text Fields (Kéo từ Hierarchy)")]
    [SerializeField] private TextMeshProUGUI maxHPStatText;
    [SerializeField] private TextMeshProUGUI damageStatText;
    [SerializeField] private TextMeshProUGUI speedStatText;
    [SerializeField] private TextMeshProUGUI attackSpeedStatText;

    private PlayerStats playerStats;

    void Start()
    {
        // Tìm PlayerStats
        if (PlayerStats.Instance != null)
        {
            playerStats = PlayerStats.Instance;
        }
        else
        {
            Debug.LogError("StatsDisplayUI không tìm thấy PlayerStats.Instance!");
            return;
        }

        // Cập nhật UI lần đầu
        UpdateStats();
    }

    // Update() liên tục để đảm bảo chỉ số luôn đúng
    // (Nếu bạn muốn tối ưu hơn, có thể dùng Event-based)
    void Update()
    {
        if (playerStats == null) return;

        UpdateStats();
    }

    // Hàm này lấy dữ liệu từ PlayerStats và gán vào Text
    private void UpdateStats()
    {
        maxHPStatText.text = playerStats.GetMaxHealthDisplay();
        damageStatText.text = playerStats.GetDamageDisplay();
        speedStatText.text = playerStats.GetMoveSpeedDisplay();
        attackSpeedStatText.text = playerStats.GetAttackSpeedDisplay();
    }
}
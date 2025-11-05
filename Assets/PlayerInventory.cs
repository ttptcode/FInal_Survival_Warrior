using UnityEngine;
using System.Collections.Generic;
using System; // Cần dùng cho 'Action'

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance; // Dùng Singleton để ShopManager dễ dàng truy cập

    // Dùng Dictionary để lưu trữ item và số lượng stack
    // Key: Dữ liệu item (ScriptableObject)
    // Value: Số lượng (int)
    public Dictionary<ShopItemData, int> ownedItems = new Dictionary<ShopItemData, int>();

    // Event này sẽ thông báo cho UI Grid biết khi nào cần "vẽ lại"
    public static event Action OnInventoryChanged;

    private PlayerStats playerStats;

    private void Awake()
    {
        // Thiết lập Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Lấy Instance của PlayerStats khi bắt đầu
        if (PlayerStats.Instance != null)
        {
            playerStats = PlayerStats.Instance;
        }
        else
        {
            Debug.LogError("PlayerInventory không tìm thấy PlayerStats.Instance!");
        }
    }

    public void AddItem(ShopItemData item)
    {
        // --- 1. Cập nhật kho đồ (Code cũ của bạn) ---
        if (ownedItems.ContainsKey(item))
        {
            ownedItems[item]++;
        }
        else
        {
            ownedItems.Add(item, 1);
        }

        // --- 2. ÁP DỤNG CHỈ SỐ (Logic mới) ---
        if (playerStats != null && item.stats != null)
        {
            // Đọc qua từng dòng stat có trên item
            foreach (ItemStat stat in item.stats)
            {
                ApplyStat(stat); // Gọi hàm helper để áp dụng
            }
        }
        else
        {
            Debug.LogError("Không thể áp dụng chỉ số: PlayerStats hoặc item.stats bị null!");
        }

        // --- 3. Phát tín hiệu cho UI Grid (Code cũ của bạn) ---
        OnInventoryChanged?.Invoke();
        Debug.Log($"Đã thêm {item.itemName}. Tổng cộng: {ownedItems[item]}");
    }
    private void ApplyStat(ItemStat stat)
    {
        // Dùng switch để gọi đúng hàm Add... bên trong PlayerStats
        // (Giả sử bạn đã có enum StatType từ script ItemStat.cs)
        switch (stat.stat)
        {
            case StatType.MaxHP:
                // Giả sử stat.value là giá trị cộng thẳng (ví dụ: 5)
                playerStats.AddMaxHealth(stat.value);
                break;
            case StatType.Damage:
                // Giả sử stat.value là giá trị cộng thẳng (ví dụ: 2)
                playerStats.AddDamage(stat.value);
                break;
            case StatType.Speed:
                // Giả sử stat.value là % (ví dụ: 0.1 cho 10%)
                playerStats.AddSpeed(stat.value);
                break;
            case StatType.AttackSpeed:
                // Giả sử stat.value là % (ví dụ: 0.05 cho 5%)
                playerStats.AddAttackSpeed(stat.value);
                break;
            default:
                Debug.LogWarning($"Chưa xử lý cho StatType: {stat.stat}");
                break;
        }
    }
}
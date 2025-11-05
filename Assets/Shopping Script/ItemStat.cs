using UnityEngine;

// Enum để định nghĩa tất cả các loại chỉ số trong game
public enum StatType
{
    // 4 chỉ số bạn yêu cầu
    MaxHP,
    Damage,
    Speed,          // Tốc độ di chuyển
    AttackSpeed,    // Tốc độ tấn công

    // Các chỉ số phổ biến khác
    CritChance,
    CritDamage,
    LifeSteal,
    HPRegeneration,
    Armor,
    Range,
    Knockback
    // ... Thêm bất kỳ chỉ số nào bạn muốn
}

// Enum để định nghĩa loại chỉ số: là cộng thẳng (Flat) hay theo phần trăm (Percent)
public enum StatModType
{
    Flat,     // Ví dụ: +5 HP
    Percent   // Ví dụ: +5% HP
}

// [System.Serializable] rất quan trọng
// Nó cho phép Unity hiển thị lớp này trong Inspector
[System.Serializable]
public class ItemStat
{
    public StatType stat;   // Chỉ số nào sẽ bị thay đổi?
    public float value;     // Thay đổi bao nhiêu? (Ví dụ: 5 hoặc 0.05)
    public StatModType type; // Kiểu thay đổi là gì?

    // (Tùy chọn) Constructor để tạo code dễ dàng hơn
    public ItemStat(StatType stat, float value, StatModType type)
    {
        this.stat = stat;
        this.value = value;
        this.type = type;
    }
}
using UnityEngine;
using TMPro;

public class StatDisplayLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;

    // Hàm này sẽ được gọi bởi ShopItemSlot
    public void Setup(ItemStat stat)
    {
        if (textComponent == null)
            textComponent = GetComponent<TextMeshProUGUI>();

        textComponent.text = FormatStat(stat);
    }

    // Hàm này biến dữ liệu (Stat: MaxHP, Value: 5, Type: Flat)
    // thành một chuỗi string ("+5 Max HP")
    private string FormatStat(ItemStat stat)
    {
        string prefix = stat.value > 0 ? "+" : ""; // Thêm dấu "+" nếu là số dương
        string valueStr = "";
        string statName = "";

        // Chuyển loại chỉ số thành string
        switch (stat.stat)
        {
            case StatType.MaxHP:
                statName = "Max HP";
                break;
            case StatType.Damage:
                statName = "Damage";
                break;
            case StatType.Speed:
                statName = "Speed";
                break;
            case StatType.AttackSpeed:
                statName = "Attack Speed";
                break;
            // Thêm các stat khác của bạn ở đây...
            default:
                statName = stat.stat.ToString(); // Tạm thời dùng tên enum
                break;
        }

        // Chuyển kiểu giá trị (Flat hay Percent)
        if (stat.type == StatModType.Flat)
        {
            valueStr = prefix + stat.value.ToString(); // Ví dụ: "+5"
        }
        else if (stat.type == StatModType.Percent)
        {
            // Nhân với 100 để hiển thị (0.05 -> 5%)
            valueStr = prefix + (stat.value * 100).ToString() + "%"; // Ví dụ: "+5%"
        }

        // Thay đổi màu sắc dựa trên stat (giống Brotato)
        if (stat.value > 0)
            textComponent.color = Color.green; // Màu xanh cho chỉ số tốt
        else
            textComponent.color = Color.red;   // Màu đỏ cho chỉ số xấu (curse)

        // Trả về chuỗi cuối cùng
        return $"{valueStr} {statName}"; // Ví dụ: "+5 Max HP" hoặc "-10% Speed"
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // <-- Thêm thư viện này

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Brotato/Shop Item")]
public class ShopItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string itemType;
    public Sprite itemIcon;
    public Color borderColor = Color.white;

    [TextArea(5, 10)]
    public string description;

    [Header("Stats")]
    public int cost;
    public int currentStack = 1;
    public int maxStack = 1;

    // === DÒNG QUAN TRỌNG NHẤT ===
    // Thêm dòng này để chứa tất cả các chỉ số của item
    public List<ItemStat> stats;

}
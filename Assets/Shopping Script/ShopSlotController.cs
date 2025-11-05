using UnityEngine;
using UnityEngine.UI;
using TMPro; // Đừng quên thư viện này
using System.Collections.Generic;

public class ShopSlotController : MonoBehaviour
{
    [Header("UI References (Tự kéo vào)")]
    [Tooltip("Kéo 'item_shopping' (Image) của ô này vào")]
    [SerializeField] private Image itemIcon;

    [Tooltip("Kéo 'item_shopping_text' (Text) của ô này vào")]
    [SerializeField] private TextMeshProUGUI itemName;

    [Tooltip("Kéo 'item_shopping_stat' (Container) của ô này vào")]
    [SerializeField] private Transform statContainer;

    // === THÊM MỚI: Các tham chiếu cho Nút Mua ===
    [Header("Buy Button")]
    [Tooltip("Kéo Nút Mua (BuyButton) vào đây")]
    [SerializeField] private Button buyButton;

    [Tooltip("Kéo Text hiển thị giá tiền (CostText) vào đây")]
    [SerializeField] private TextMeshProUGUI costText;
    // === KẾT THÚC THÊM MỚI ===

    [Header("Prefab (Kéo từ Project)")]
    [Tooltip("Kéo Prefab của 1 dòng text (StatLine_Prefab) vào đây")]
    [SerializeField] private GameObject statLinePrefab;

    private ShopItemData currentItem; // Lưu lại item đang hiển thị

    private ShopManager shopManager; // === THÊM MỚI: Tham chiếu đến Manager ===

    // Hàm này được gọi bởi ShopManager

    private void Start()
    {
        // === THÊM MỚI: Tự động tìm ShopManager ===
        // (Vì ShopSlotManager là cha của Shopping1, 2, 3, 4)
        shopManager = GetComponentInParent<ShopManager>();
    }
    public void DisplayItem(ShopItemData item)
    {
        currentItem = item;

        // Cập nhật UI cơ bản
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;

        // === THÊM MỚI: Cập nhật giá tiền trên nút ===
        if (costText != null)
        {
            // Lấy giá tiền từ ScriptableObject và chuyển thành chữ
            costText.text = item.cost.ToString();
        }
        // === KẾT THÚC THÊM MỚI ===

        // 1. Xóa tất cả các dòng stat cũ (để chuẩn bị cho Reroll)
        foreach (Transform child in statContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Tạo các dòng stat mới từ dữ liệu
        if (item.stats != null && item.stats.Count > 0)
        {
            foreach (ItemStat stat in item.stats)
            {
                // Tạo một StatLine mới từ Prefab
                GameObject statLineGO = Instantiate(statLinePrefab, statContainer);

                // Ra lệnh cho nó tự hiển thị (cần có script StatDisplayLine.cs)
                statLineGO.GetComponent<StatDisplayLine>().Setup(stat);
            }
        }
    }

    // Bạn có thể thêm các hàm cho nút Mua/Khóa ở đây
    public void OnBuyButtonCicked()
    {
        // Thay vì chỉ Debug.Log, hãy gọi Manager
        if (currentItem != null && shopManager != null)
        {
            shopManager.BuyItem(currentItem);
        }
        else
        {
            Debug.LogError("Không thể mua item: currentItem hoặc shopManager bị null!");
        }
    }
}
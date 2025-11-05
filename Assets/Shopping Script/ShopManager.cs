using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Item Pool")]
    [Tooltip("Kéo TẤT CẢ các ScriptableObject (data) item của bạn vào đây")]
    [SerializeField] private List<ShopItemData> itemPool;

    [Header("Shop Slots")]
    [Tooltip("Kéo 4 GameObject Shopping1, Shopping2, Shopping3, Shopping4 vào đây")]
    [SerializeField] private List<ShopSlotController> shopSlots;

    // (Bạn cũng có thể thêm tham chiếu cho nút Reroll và Text tiền ở đây)



    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;

    private PlayerCurrency playerCurrency;

    // === THÊM MỚI: UI Thông báo ===
    [Header("UI Feedback")]
    [Tooltip("Kéo Text (TMP) dùng để hiển thị thông báo vào đây")]
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float notificationDisplayTime = 2.5f;


    void Start()
    {
        if (PlayerCurrency.Instance != null)
        {
            playerCurrency = PlayerCurrency.Instance;
        }
        else
        {
            Debug.LogError("ShopManager không tìm thấy PlayerCurrency.Instance!");
        }
        // Tự động random item khi mới mở shop (để test)
        GenerateShopItems();
    }

    // Hàm này được gọi bởi Start hoặc khi nhấn nút Reroll
    public void GenerateShopItems()
    {
        // Tạo một danh sách các item đã chọn để tránh trùng lặp (tùy chọn)
        List<ShopItemData> availableItems = new List<ShopItemData>(itemPool);

        // Duyệt qua từng ô shop (Shopping1, 2, 3, 4)
        foreach (ShopSlotController slot in shopSlots)
        {
            if (availableItems.Count == 0)
            {
                Debug.LogWarning("Không còn item trong kho để random!");
                break;
            }

            // Lấy một item ngẫu nhiên
            int randomIndex = Random.Range(0, availableItems.Count);
            ShopItemData randomItem = availableItems[randomIndex];

            // Xóa item đó khỏi kho tạm để không bị random lại
            availableItems.RemoveAt(randomIndex);

            // Ra lệnh cho ô shop đó: "Hãy hiển thị item này!"
            slot.DisplayItem(randomItem);
        }
    }
    public void BuyItem(ShopItemData itemToBuy)
    {
        // === LOGIC MUA HÀNG ĐÃ CẬP NHẬT ===

        if (playerInventory == null || playerCurrency == null)
        {
            Debug.LogError("ShopManager bị thiếu tham chiếu PlayerInventory hoặc PlayerCurrency!");
            return;
        }

        // 1. Kiểm tra xem có đủ tiền không
        if (playerCurrency.CanAfford(itemToBuy.cost))
        {
            // 2. Nếu đủ, tiêu tiền
            playerCurrency.SpendMoney(itemToBuy.cost);

            // 3. Thêm item vào kho đồ
            playerInventory.AddItem(itemToBuy);

            Debug.Log($"Mua thành công {itemToBuy.itemName}!");

            // (Tùy chọn: Sau khi mua, bạn có thể vô hiệu hóa nút đó
            // hoặc Reroll một item mới vào vị trí đó)
        }
        else
        {
            // 4. Nếu không đủ tiền, thông báo
            Debug.Log("You are not enough currency");
            StartCoroutine(ShowNotification("Not enought currency", Color.red));

        }
    }

    private IEnumerator ShowNotification(string message, Color color)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            notificationText.color = color;
            notificationText.gameObject.SetActive(true);

            // Chờ một khoảng thời gian
            yield return new WaitForSeconds(notificationDisplayTime);

            // Ẩn thông báo
            notificationText.gameObject.SetActive(false);
        }
        else
        {
            // Fallback về Console nếu không có Text UI
            Debug.LogWarning(message);
        }
    }

}
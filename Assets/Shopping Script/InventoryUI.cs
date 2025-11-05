using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryIconPrefab; // Kéo 'InventoryIcon_Prefab' vào đây

    private void OnEnable()
    {
        // Đăng ký lắng nghe sự kiện từ Kho đồ
        PlayerInventory.OnInventoryChanged += UpdateInventoryDisplay;
    }

    private void OnDisable()
    {
        // Hủy đăng ký khi tắt
        PlayerInventory.OnInventoryChanged -= UpdateInventoryDisplay;
    }

    // Hàm này sẽ "vẽ lại" toàn bộ Grid
    private void UpdateInventoryDisplay()
    {
        // 1. Xóa tất cả các icon cũ
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 2. Vẽ các icon mới dựa trên dữ liệu từ PlayerInventory
        // 'transform' ở đây chính là cái Grid (vì script này gắn trên Grid)
        foreach (var itemEntry in PlayerInventory.Instance.ownedItems)
        {
            ShopItemData item = itemEntry.Key;
            int stackCount = itemEntry.Value;

            // Tạo một icon mới từ Prefab
            GameObject iconGO = Instantiate(inventoryIconPrefab, transform);

            // Ra lệnh cho nó hiển thị đúng icon và stack
            iconGO.GetComponent<InventoryIconController>().Setup(item, stackCount);
        }
    }
}
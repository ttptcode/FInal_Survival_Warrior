using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryIconController : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI stackText;

    public void Setup(ShopItemData item, int stackCount)
    {
        // Gán icon
        if (iconImage != null)
            iconImage.sprite = item.itemIcon;

        // Xử lý hiển thị stack
        if (stackCount > 1 && stackText != null)
        {
            stackText.text = "x" + stackCount.ToString();
            stackText.gameObject.SetActive(true);
        }
        else if (stackText != null)
        {
            // Ẩn text đi nếu chỉ có 1 item
            stackText.gameObject.SetActive(false);
        }
    }
}
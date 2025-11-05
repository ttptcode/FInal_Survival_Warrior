using UnityEngine;
using TMPro; // Cần dùng thư viện TextMeshPro

public class PlayerCurrency : MonoBehaviour
{
    // Singleton pattern để các script khác (như Coin) dễ dàng truy cập
    public static PlayerCurrency Instance;

    [Header("UI References")]
    [Tooltip("Kéo 'Money_text' (TextMeshPro) của bạn vào đây")]
    [SerializeField] private TextMeshProUGUI moneyText;

    private int currentMoney;

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
        // Yêu cầu: set money_text ban đầu là 0
        currentMoney = 0;
        UpdateMoneyText();
    }

    // Hàm này sẽ được gọi bởi script Coin
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyText();
    }

    // Cập nhật text hiển thị
    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = currentMoney.ToString();
        }
    }

    public bool CanAfford(int amount)
    {
        return currentMoney >= amount;
    }

    // === HÀM MỚI ĐỂ TIÊU TIỀN ===
    // Trả về true nếu tiêu thành công, false nếu không đủ tiền
    public bool SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            currentMoney -= amount;
            UpdateMoneyText();
            return true; // Mua thành công
        }
        else
        {
            return false; // Không đủ tiền
        }
    }

}

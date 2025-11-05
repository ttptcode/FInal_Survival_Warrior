using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Đảm bảo luôn có Collider2D
public class Coin : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int value = 1; // Giá trị của đồng coin

    // === THÊM MỚI: Hiệu ứng Magnet ===
    [Header("Magnet Effect")]
    [Tooltip("Bán kính Player phải đi vào để hút coin")]
    [SerializeField] private float magnetRadius = 5f;
    [Tooltip("Tốc độ coin bay về phía Player")]
    [SerializeField] private float moveSpeed = 12f;
    [Tooltip("Khoảng cách đủ gần để tự động nhặt")]
    [SerializeField] private float collectionDistance = 0.5f;

    private Transform playerTransform;
    private bool isChasing = false;
    private bool isCollected = false; // Cờ để tránh nhặt 2 lần
    // === KẾT THÚC THÊM MỚI ===

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;

        // Cố gắng tìm Player ngay lập tức bằng Singleton
        if (PlayerStats.Instance != null)
        {
            playerTransform = PlayerStats.Instance.transform;
        }
    }

    // === THÊM MỚI: Hàm Update() để xử lý hút ===
    private void Update()
    {
        if (isCollected) return; // Đã nhặt rồi, không làm gì nữa

        // Nếu chưa tìm thấy Player (ví dụ: Player vừa mới spawn)
        if (playerTransform == null)
        {
            if (PlayerStats.Instance != null)
                playerTransform = PlayerStats.Instance.transform;
            else
                return; // Vẫn không tìm thấy, bỏ qua frame này
        }

        // Tính khoảng cách từ coin đến Player
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Nếu player vào tầm hút, bắt đầu đuổi theo
        if (distance <= magnetRadius)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            // Di chuyển về phía player
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            // Nếu đã đủ gần, "nhặt"
            if (distance <= collectionDistance)
            {
                CollectCoin();
            }
        }
    }
    // === KẾT THÚC THÊM MỚI ===

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu player vô tình chạy đè lên coin trước khi nó bị hút
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    // === THÊM MỚI: Hàm thu thập (để tránh lặp code) ===
    private void CollectCoin()
    {
        if (isCollected) return; // Đảm bảo chỉ chạy 1 lần
        isCollected = true;

        // Tìm PlayerCurrency (thông qua Singleton) và cộng tiền
        if (PlayerCurrency.Instance != null)
        {
            PlayerCurrency.Instance.AddMoney(value);
        }
        else
        {
            Debug.LogError("Không tìm thấy PlayerCurrency.Instance!");
        }

        // Phát âm thanh
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoinCollectSound();
        }

        // Tự hủy đồng coin sau khi nhặt
        Destroy(gameObject);
    }
}
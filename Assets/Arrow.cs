using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Arrow : MonoBehaviour
{
    private float damage;
    private Rigidbody2D rb;

    [Header("Arrow Stats")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 3f;

    [Header("Collision")]
    [Tooltip("Các layer sẽ phá hủy mũi tên (ví dụ: Wall, Obstacle)")]
    [SerializeField] private string[] destroyOnLayers; // <-- GIỐNG HỆT BULLET.CS

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Hàm này được gọi bởi EnemyWarriorBow
    public void Setup(float damage, Vector2 direction)
    {
        this.damage = damage;

        // Bắn mũi tên
        rb.linearVelocity = direction.normalized * speed;

        // Xoay mũi tên theo hướng bay
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Tự hủy sau một thời gian
        Destroy(gameObject, lifetime);
    }

    // === ĐÃ CẬP NHẬT HÀM NÀY ===
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Kiểm tra nếu trúng Player (mục tiêu của Arrow)
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null) // Kiểm tra bằng GetComponent an toàn hơn CompareTag
        {
            playerStats.TakeDamage(damage);
            Destroy(gameObject); // Hủy mũi tên
            return; // Dừng lại
        }

        // 2. Logic va chạm với tường (GIỐNG HỆT BULLET.CS)
        int otherLayer = other.gameObject.layer;
        foreach (string layerName in destroyOnLayers)
        {
            if (otherLayer == LayerMask.NameToLayer(layerName))
            {
                Destroy(gameObject);
                return; // Dừng lại
            }
        }
    }
}

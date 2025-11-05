using UnityEngine;

// Đây là phiên bản "dumb" của Bullet
// Nó không biết gì về PlayerStats
public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifetime = 3f;
    [SerializeField] protected string[] destroyOnLayers;
    [SerializeField] protected GameObject bloodPrefab;

    // Biến này sẽ được 'Gun' gán giá trị
    protected float damage;

    protected Rigidbody2D rb;

    // Hàm Start() giờ rất đơn giản
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }

    // Hàm này được gọi TỪ BÊN NGOÀI (bởi Gun)
    public virtual void Setup(float finalDamage)
    {
        // Nhận và lưu lại sát thương
        this.damage = finalDamage;
    }

    // Hàm va chạm giờ chỉ dùng 'damage' đã được gán
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Gây sát thương bằng giá trị đã được truyền vào
            enemy.TakeDamage(damage);
            GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            Destroy(blood, 1f);
            Destroy(gameObject);
            return;
        }

        // Logic va chạm với tường giữ nguyên
        int otherLayer = other.gameObject.layer;
        foreach (string layerName in destroyOnLayers)
        {
            if (otherLayer == LayerMask.NameToLayer(layerName))
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}

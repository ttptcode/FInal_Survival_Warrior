// File: Bullet.cs (Đã nâng cấp)

using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 10f; // Đổi thành protected
    [SerializeField] protected float lifetime = 3f; // Đổi thành protected
    [SerializeField] protected string[] destroyOnLayers; // Đổi thành protected
    public float damage = 10f;

    protected Rigidbody2D rb; // Đổi thành protected

    // Đổi Start() thành virtual để lớp con có thể thêm chức năng
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }

    // Rất quan trọng: Đổi thành protected virtual
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // Đạn thường sẽ tự hủy ngay
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
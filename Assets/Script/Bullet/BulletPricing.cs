// File: BulletPricing.cs (Phiên bản cuối cùng, hoạt động chính xác)

using UnityEngine;
using System.Collections.Generic;

public class BulletPricing : Bullet
{
    [Header("Piercing Properties")]
    [SerializeField] private int pierceCount = 2;

    // THAY ĐỔI 1: Danh sách này giờ sẽ lưu các Enemy, không phải Collider
    private List<Enemy> alreadyHit = new List<Enemy>();

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            // THAY ĐỔI 2: Kiểm tra xem đã bắn trúng "Enemy" này chưa
            if (alreadyHit.Contains(enemy))
            {
                return; // Nếu đã bắn trúng Enemy này rồi, bỏ qua
            }

            // THAY ĐỔI 3: Thêm "Enemy" vào danh sách
            alreadyHit.Add(enemy);

            enemy.TakeDamage(damage);

            if (pierceCount > 0)
            {
                pierceCount--;
            }
            else
            {
                Destroy(gameObject);
            }
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
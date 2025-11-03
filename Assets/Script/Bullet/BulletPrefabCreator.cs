// using UnityEngine;

// public class BulletPrefabCreator : MonoBehaviour
// {
//     [Header("Bullet Settings")]
//     public Sprite bulletSprite;
//     public float bulletScale = 1f;
//     public Color bulletColor = Color.white;
    
//     [Header("Physics")]
//     public bool useGravity = false;
//     public float mass = 0.1f;
//     public float drag = 0f;
//     public float angularDrag = 0.5f;
    
//     [Header("Collision")]
//     public bool isTrigger = true;
//     public PhysicsMaterial2D physicsMaterial;
    
//     [ContextMenu("Create Bullet Prefab")]
//     public void CreateBulletPrefab()
//     {
//         // Tạo GameObject cho bullet
//         GameObject bulletPrefab = new GameObject("Bullet");
        
//         // Thêm SpriteRenderer
//         SpriteRenderer spriteRenderer = bulletPrefab.AddComponent<SpriteRenderer>();
//         if (bulletSprite != null)
//         {
//             spriteRenderer.sprite = bulletSprite;
//         }
//         spriteRenderer.color = bulletColor;
//         spriteRenderer.sortingOrder = 10; // Đảm bảo bullet hiển thị trên các object khác
        
//         // Thêm Collider2D
//         CircleCollider2D collider = bulletPrefab.AddComponent<CircleCollider2D>();
//         collider.isTrigger = isTrigger;
//         collider.radius = 0.1f; // Bán kính nhỏ cho bullet
//         if (physicsMaterial != null)
//         {
//             collider.sharedMaterial = physicsMaterial;
//         }
        
//         // Thêm Rigidbody2D
//         Rigidbody2D rb = bulletPrefab.AddComponent<Rigidbody2D>();
//         rb.gravityScale = useGravity ? 1f : 0f;
//         rb.mass = mass;
//         rb.linearDamping = drag;
//         rb.angularDamping = angularDrag;
//         rb.freezeRotation = true; // Không cho bullet xoay
        
//         // Thêm Bullet script
//         Bullet bulletScript = bulletPrefab.AddComponent<Bullet>();
        
//         // Thiết lập scale
//         bulletPrefab.transform.localScale = Vector3.one * bulletScale;
        
//         // Lưu prefab
//         string prefabPath = "Assets/BulletPrefab.prefab";
//         #if UNITY_EDITOR
//         UnityEditor.PrefabUtility.SaveAsPrefabAsset(bulletPrefab, prefabPath);
//         Debug.Log($"Bullet prefab created at: {prefabPath}");
//         #endif
        
//         // Xóa object tạm thời
//         DestroyImmediate(bulletPrefab);
//     }
    
//     [ContextMenu("Create Gun 1 Prefab")]
//     public void CreateGun1Prefab()
//     {
//         // Tạo GameObject cho gun
//         GameObject gunPrefab = new GameObject("Gun1");
        
//         // Thêm SpriteRenderer
//         SpriteRenderer spriteRenderer = gunPrefab.AddComponent<SpriteRenderer>();
//         spriteRenderer.sortingOrder = 5; // Hiển thị sau player nhưng trước background
        
//         // Thêm Gun script
//         Gun gunScript = gunPrefab.AddComponent<Gun>();
        
//         // Thiết lập gun data
//         gunScript.gunData = new GunData();
//         gunScript.gunData.gunName = "Gun 1";
//         gunScript.gunData.fireRate = 0.3f;
//         gunScript.gunData.damage = 15f;
//         gunScript.gunData.bulletSpeed = 15f;
//         gunScript.gunData.maxAmmo = 30;
//         gunScript.gunData.ammo = 30;
//         gunScript.gunData.reloadTime = 1.5f;
        
//         // Lưu prefab
//         string prefabPath = "Assets/Gun1Prefab.prefab";
//         #if UNITY_EDITOR
//         UnityEditor.PrefabUtility.SaveAsPrefabAsset(gunPrefab, prefabPath);
//         Debug.Log($"Gun 1 prefab created at: {prefabPath}");
//         #endif
        
//         // Xóa object tạm thời
//         DestroyImmediate(gunPrefab);
//     }
// }

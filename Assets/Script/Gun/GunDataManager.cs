// using UnityEngine;

// [CreateAssetMenu(fileName = "New Gun Data", menuName = "Gun System/Gun Data")]
// public class GunDataManager : ScriptableObject
// {
//     [Header("Gun Information")]
//     public string gunName = "Gun 1";
//     public Sprite gunSprite;
    
//     [Header("Gun Stats")]
//     public float fireRate = 0.5f;
//     public float damage = 10f;
//     public float bulletSpeed = 10f;
//     public int maxAmmo = 30;
//     public float reloadTime = 2f;
    
//     [Header("Bullet Settings")]
//     public GameObject bulletPrefab;
//     public Sprite bulletSprite;
    
//     [Header("Audio")]
//     public AudioClip fireSound;
//     public AudioClip reloadSound;
    
//     [Header("Effects")]
//     public GameObject muzzleFlash;
//     public GameObject hitEffect;
    
//     // Tạo GunData từ ScriptableObject này
//     public GunData ToGunData()
//     {
//         GunData data = new GunData();
//         data.gunName = gunName;
//         data.fireRate = fireRate;
//         data.damage = damage;
//         data.bulletSpeed = bulletSpeed;
//         data.ammo = maxAmmo;
//         data.maxAmmo = maxAmmo;
//         data.reloadTime = reloadTime;
//         data.gunSprite = gunSprite;
//         data.bulletPrefab = bulletPrefab;
        
//         return data;
//     }
// }

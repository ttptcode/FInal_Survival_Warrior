// using UnityEngine;
// using UnityEngine.InputSystem;

// public class GunSystemSetup : MonoBehaviour
// {
//     [Header("Setup Instructions")]
//     [TextArea(10, 20)]
//     public string setupInstructions = @"
// HƯỚNG DẪN SETUP GUN SYSTEM:

// 1. TẠO BULLET PREFAB:
//    - Tạo GameObject mới tên 'Bullet'
//    - Add SpriteRenderer với sprite đạn từ Assets/Pixel Guns 2D/Guns/Bullets/
//    - Add CircleCollider2D (isTrigger = true)
//    - Add Rigidbody2D (gravityScale = 0, freezeRotation = true)
//    - Add Bullet.cs script
//    - Save as Prefab

// 2. TẠO GUN PREFAB:
//    - Tạo GameObject mới tên 'Gun1'
//    - Add SpriteRenderer với sprite súng từ Assets/Pixel Guns 2D/Guns/Guns/Gun 1.png
//    - Add Gun.cs script
//    - Thiết lập GunData trong inspector
//    - Assign bullet prefab vào gunData.bulletPrefab
//    - Save as Prefab

// 3. SETUP PLAYER:
//    - Add PlayerGunController.cs vào player
//    - Assign gun prefab vào currentGun
//    - Thiết lập Input Actions cho fire, reload, aim

// 4. INPUT SETUP:
//    - Trong InputSystem_Actions.inputactions, thêm:
//      * Fire action (Button)
//      * Reload action (Button) 
//      * Aim action (Vector2)

// 5. LAYER SETUP:
//    - Tạo layer 'Enemy' cho enemy objects
//    - Tạo layer 'Obstacle' cho vật cản
//    - Thiết lập layer trong Bullet.cs

// 6. TEST:
//    - Play scene
//    - Click chuột hoặc dùng input để bắn
//    - R để reload
//    - Di chuyển chuột để aim
// ";

//     [Header("Auto Setup")]
//     public bool autoSetup = false;
//     public GameObject bulletPrefab;
//     public GameObject gun1Prefab;
    
//     [ContextMenu("Auto Setup Gun System")]
//     public void AutoSetupGunSystem()
//     {
//         if (!autoSetup) return;
        
//         // Tìm player
//         GameObject player = GameObject.FindGameObjectWithTag("Player");
//         if (player == null)
//         {
//             Debug.LogError("Không tìm thấy Player! Hãy đảm bảo player có tag 'Player'");
//             return;
//         }
        
//         // Thêm PlayerGunController nếu chưa có
//         PlayerGunController gunController = player.GetComponent<PlayerGunController>();
//         if (gunController == null)
//         {
//             gunController = player.AddComponent<PlayerGunController>();
//         }
        
//         // Tạo gun holder
//         Transform gunHolder = player.transform.Find("GunHolder");
//         if (gunHolder == null)
//         {
//             GameObject gunHolderObj = new GameObject("GunHolder");
//             gunHolderObj.transform.SetParent(player.transform);
//             gunHolderObj.transform.localPosition = Vector3.zero;
//             gunController.gunHolder = gunHolderObj.transform;
//         }
        
//         // Tạo fire point
//         Transform firePoint = player.transform.Find("GunHolder/FirePoint");
//         if (firePoint == null)
//         {
//             GameObject firePointObj = new GameObject("FirePoint");
//             firePointObj.transform.SetParent(gunController.gunHolder);
//             firePointObj.transform.localPosition = new Vector3(0.5f, 0, 0);
//             gunController.firePoint = firePointObj.transform;
//         }
        
//         // Gắn gun nếu có prefab
//         if (gun1Prefab != null)
//         {
//             GameObject gunInstance = Instantiate(gun1Prefab, gunController.gunHolder);
//             Gun gunScript = gunInstance.GetComponent<Gun>();
//             if (gunScript != null)
//             {
//                 gunController.AttachGun(gunScript);
//             }
//         }
        
//         Debug.Log("Gun System đã được setup tự động!");
//     }
    
//     void Start()
//     {
//         if (autoSetup)
//         {
//             AutoSetupGunSystem();
//         }
//     }
// }

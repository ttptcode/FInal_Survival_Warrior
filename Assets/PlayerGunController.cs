// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerGunController : MonoBehaviour
// {
//     [Header("Gun Settings")]
//     public Gun currentGun;
//     public Transform gunHolder; // Vị trí gắn súng trên player
//     public Transform firePoint; // Điểm bắn đạn
    
//     [Header("Input")]
//     public InputActionReference fireAction;
//     public InputActionReference reloadAction;
//     public InputActionReference aimAction;
    
//     [Header("Aiming")]
//     public Camera playerCamera;
//     public float aimSensitivity = 1f;
    
//     private Vector2 aimInput;
//     private float aimAngle;
    
//     void Start()
//     {
//         // Thiết lập camera nếu chưa có
//         if (playerCamera == null)
//         {
//             playerCamera = Camera.main;
//         }
        
//         // Thiết lập gun holder nếu chưa có
//         if (gunHolder == null)
//         {
//             GameObject gunHolderObj = new GameObject("GunHolder");
//             gunHolderObj.transform.SetParent(transform);
//             gunHolderObj.transform.localPosition = Vector3.zero;
//             gunHolder = gunHolderObj.transform;
//         }
        
//         // Thiết lập fire point nếu chưa có
//         if (firePoint == null)
//         {
//             GameObject firePointObj = new GameObject("FirePoint");
//             firePointObj.transform.SetParent(gunHolder);
//             firePointObj.transform.localPosition = new Vector3(0.5f, 0, 0); // Phía trước súng
//             firePoint = firePointObj.transform;
//         }
        
//         // Gắn súng hiện tại
//         if (currentGun != null)
//         {
//             AttachGun(currentGun);
//         }
//     }
    
//     void Update()
//     {
//         HandleInput();
//         HandleAiming();
//     }
    
//     private void HandleInput()
//     {
//         // Bắn
//         if (fireAction != null && fireAction.action.WasPressedThisFrame())
//         {
//             Fire();
//         }
        
//         // Reload
//         if (reloadAction != null && reloadAction.action.WasPressedThisFrame())
//         {
//             Reload();
//         }
        
//         // Aim
//         if (aimAction != null)
//         {
//             aimInput = aimAction.action.ReadValue<Vector2>();
//         }
//     }
    
//     private void HandleAiming()
//     {
//         if (aimInput != Vector2.zero)
//         {
//             // Tính góc aim dựa trên input
//             aimAngle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
//         }
//         else
//         {
//             // Aim theo chuột
//             Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
//             Vector3 direction = (mousePos - transform.position).normalized;
//             aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//         }
        
//         // Cập nhật rotation của gun holder
//         if (gunHolder != null)
//         {
//             gunHolder.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
//         }
        
//         // Flip sprite của player nếu cần
//         SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
//         if (playerSprite != null)
//         {
//             if (aimAngle > 90f || aimAngle < -90f)
//             {
//                 playerSprite.flipX = true;
//             }
//             else
//             {
//                 playerSprite.flipX = false;
//             }
//         }
//     }
    
//     public void Fire()
//     {
//         if (currentGun != null)
//         {
//             currentGun.Fire();
//         }
//     }
    
//     public void Reload()
//     {
//         if (currentGun != null)
//         {
//             currentGun.StartReload();
//         }
//     }
    
//     public void AttachGun(Gun gun)
//     {
//         if (gun == null) return;
        
//         // Gỡ súng cũ nếu có
//         if (currentGun != null)
//         {
//             Destroy(currentGun.gameObject);
//         }
        
//         // Gắn súng mới
//         currentGun = gun;
//         gun.transform.SetParent(gunHolder);
//         gun.transform.localPosition = Vector3.zero;
//         gun.transform.localRotation = Quaternion.identity;
        
//         // Thiết lập fire point cho súng
//         if (gun.gunData.firePoint == null)
//         {
//             gun.gunData.firePoint = firePoint;
//         }
//     }
    
//     public void SwitchGun(Gun newGun)
//     {
//         AttachGun(newGun);
//     }
    
//     public bool HasGun()
//     {
//         return currentGun != null;
//     }
    
//     public int GetCurrentAmmo()
//     {
//         return currentGun != null ? currentGun.gunData.ammo : 0;
//     }
    
//     public int GetMaxAmmo()
//     {
//         return currentGun != null ? currentGun.gunData.maxAmmo : 0;
//     }
    
//     public bool IsReloading()
//     {
//         return currentGun != null && currentGun.IsReloading();
//     }
    
//     public float GetReloadProgress()
//     {
//         return currentGun != null ? currentGun.GetReloadProgress() : 0f;
//     }
// }

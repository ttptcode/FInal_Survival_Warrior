using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] protected Camera mainCam;
    [SerializeField] protected InputActionReference aimAction;
    [SerializeField] protected Transform firePos;
    [SerializeField] protected GameObject bullet; // Prefab của đạn (cần script Bullet.cs mới)
    [SerializeField] protected float shotDelay = 0.15f;
    [SerializeField] protected InputActionReference shoot;

    [Header("Base Stats")]
    [SerializeField] protected float baseDamage = 10f; // Sát thương GỐC của súng

    protected float nextShot;
    protected PlayerStats playerStats; // Tham chiếu đến Stats

    protected virtual void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        // Lấy PlayerStats từ cha (an toàn hơn)
        playerStats = GetComponentInParent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("Gun không tìm thấy PlayerStats!");
        }
    }

    protected virtual void OnEnable()
    {
        aimAction.action.Enable();
        shoot.action.Enable(); // <-- THÊM DÒNG NÀY ĐỂ SỬA LỖI

    }

    protected virtual void OnDisable()
    {
        aimAction.action.Disable();
        shoot.action.Disable(); // <-- THÊM DÒNG NÀY ĐỂ SỬA LỖI

    }

    protected virtual void Update()
    {
        RotateToMouse();
        Shoot();
    }

    protected void RotateToMouse()
    {
        if (this == null) return;
        Vector2 mouseScreenPos = aimAction.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Hàm Shoot đã được sửa lại
    protected virtual void Shoot()
    {
        // 1. Tính toán tốc độ bắn cuối cùng
        float attackSpeedMultiplier = 1.0f;
        if (playerStats != null)
        {
            attackSpeedMultiplier = playerStats.GetTotalAttackSpeedMultiplier();
        }
        float finalShotDelay = shotDelay / attackSpeedMultiplier;

        // 2. Kiểm tra có thể bắn
        if (shoot.action.ReadValue<float>() > 0 && Time.time >= nextShot)
        {
            // 3. Tính toán sát thương cuối cùng
            float finalDamage = baseDamage; // Sát thương gốc
            if (playerStats != null)
            {
                finalDamage += playerStats.GetTotalDamage(); // Cộng thêm bonus
            }

            // 4. Tạo viên đạn
            GameObject bulletGO = Instantiate(bullet, firePos.position, firePos.rotation);
            Bullet bulletScript = bulletGO.GetComponent<Bullet>();

            // 5. "Truyền" sát thương cuối cùng cho viên đạn
            if (bulletScript != null)
            {
                bulletScript.Setup(finalDamage);
            }
            else
            {
                // Lỗi này xảy ra nếu prefab 'bullet' của bạn không có script 'Bullet.cs'
                Debug.LogError("Prefab đạn thiếu script 'Bullet'!");
            }
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGunShootSound();
            }

            // 6. Đặt thời gian hồi chiêu
            nextShot = Time.time + finalShotDelay;
        }
    }
}


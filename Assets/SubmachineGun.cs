using UnityEngine;

public class SubmachineGun : Gun
{
    [Header("Submachine Gun Settings")]
    [SerializeField] private float spreadAngle = 5f; // góc lệch tối đa (độ)
    [SerializeField] private int bulletsPerShot = 1; // bắn 1 viên/lần
    [SerializeField] private bool isAutomatic = true; // tự động bắn khi giữ chuột

    // 'new' để báo cho Unity biết chúng ta cố tình giấu hàm Update() của 'Gun'
    private new void Update()
    {
        // Xoay súng (vẫn kế thừa từ Gun)
        RotateToMouse();

        // Quyết định logic bắn
        if (isAutomatic)
            AutoShoot();
        else
            Shoot(); // Dùng hàm bắn bán tự động (nhấn 1 lần 1 viên) của Gun
    }

    // Hàm này ghi đè logic bắn tự động
    private void AutoShoot()
    {
        // 1. Tính toán tốc độ bắn (GIỐNG HỆT GUN.CS)
        float attackSpeedMultiplier = 1.0f;
        if (playerStats != null)
        {
            attackSpeedMultiplier = playerStats.GetTotalAttackSpeedMultiplier();
        }
        float finalShotDelay = shotDelay / attackSpeedMultiplier;

        // 2. Kiểm tra xem có thể bắn không
        if (shoot.action.ReadValue<float>() > 0 && Time.time >= nextShot)
        {
            // 3. Tính toán sát thương (GIỐNG HỆT GUN.CS)
            // (Chúng ta có thể truy cập baseDamage vì nó là 'protected' trong Gun.cs)
            float finalDamage = baseDamage;
            if (playerStats != null)
            {
                finalDamage += playerStats.GetTotalDamage(); // Cộng thêm bonus
            }

            // 4. Bắn ra số lượng đạn đã định (thường là 1)
            for (int i = 0; i < bulletsPerShot; i++)
            {
                // Truyền sát thương cuối cùng vào hàm FireWithSpread
                FireWithSpread(finalDamage);
            }

            // 5. Đặt lại thời gian hồi chiêu
            nextShot = Time.time + finalShotDelay;
        }
    }

    // Hàm này giờ nhận 'damageToDeal' từ hàm AutoShoot()
    private void FireWithSpread(float damageToDeal)
    {
        // 1. Tính toán góc lệch (spread)
        float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
        Quaternion spreadRotation = firePos.rotation * Quaternion.Euler(0, 0, randomAngle);

        // 2. Tạo ra viên đạn
        GameObject bulletGO = Instantiate(bullet, firePos.position, spreadRotation);
        Bullet bulletScript = bulletGO.GetComponent<Bullet>();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGunShootSound();
        }

        // 3. "Truyền" sát thương cho viên đạn (BƯỚC QUAN TRỌNG BỊ THIẾU)
        if (bulletScript != null)
        {
            bulletScript.Setup(damageToDeal);
        }
        else
        {
            Debug.LogError("Prefab đạn của SubmachineGun thiếu script 'Bullet'!");
        }
    }
}

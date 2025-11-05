using UnityEngine;

// Lớp Shotgun kế thừa tất cả các thuộc tính và hàm từ lớp Gun
public class Shotgun : Gun
{
    [Header("Shotgun Specifics")]
    [SerializeField] private int bulletsPerShot = 8; // Số lượng viên đạn (mảnh) bắn ra trong 1 lần
    [SerializeField] private float spreadAngle = 25f; // Độ tỏa tối đa của chùm đạn (tính bằng độ)

    // Ghi đè (override) hàm Shoot() của lớp Gun
    protected override void Shoot()
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
            // (Giờ chúng ta có thể truy cập baseDamage vì nó là 'protected')
            float finalDamage = baseDamage;
            if (playerStats != null)
            {
                finalDamage += playerStats.GetTotalDamage(); // Cộng thêm bonus
            }

            // 4. Bắn ra một chùm đạn, truyền sát thương cho từng viên
            for (int i = 0; i < bulletsPerShot; i++)
            {
                // Truyền 'finalDamage' vào hàm
                FireWithSpread(finalDamage);
            }

            // 5. Đặt lại thời gian hồi chiêu
            nextShot = Time.time + finalShotDelay;
        }
    }

    // Hàm này giờ nhận 'damageToDeal' từ hàm Shoot()
    private void FireWithSpread(float damageToDeal)
    {
        // 1. Tính toán góc lệch
        float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
        Quaternion spreadRotation = firePos.rotation * Quaternion.Euler(0, 0, randomAngle);

        // 2. Tạo ra viên đạn
        GameObject bulletGO = Instantiate(bullet, firePos.position, spreadRotation);
        Bullet bulletScript = bulletGO.GetComponent<Bullet>();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayShotGunSound();
        }

        // 3. "Truyền" sát thương cho viên đạn (GIỐNG HỆT GUN.CS)
        if (bulletScript != null)
        {
            bulletScript.Setup(damageToDeal);
        }
        else
        {
            Debug.LogError("Prefab đạn của Shotgun thiếu script 'Bullet'!");
        }
    }
}

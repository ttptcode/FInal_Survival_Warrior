using UnityEngine;

public class SubmachineGun : Gun
{
    [Header("Submachine Gun Settings")]
    [SerializeField] private float spreadAngle = 5f; // góc lệch tối đa (độ)
    [SerializeField] private int bulletsPerShot = 1; // bắn 1 viên/lần (giữ nguyên từ Gun)
    [SerializeField] private bool isAutomatic = true; // tự động bắn khi giữ chuột

    private new void Update()
    {
        RotateToMouse();

        // Nếu là súng tự động thì bắn liên tục khi giữ chuột
        if (isAutomatic)
            AutoShoot();
        else
            Shoot(); // Dùng hàm bắn mặc định của Gun
    }

    private void AutoShoot()
    {
        if (shoot.action.ReadValue<float>() > 0 && Time.time >= nextShot)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                FireWithSpread();
            }

            nextShot = Time.time + shotDelay;
        }
    }

    private void FireWithSpread()
    {
        // Tạo độ lệch ngẫu nhiên quanh hướng bắn
        float randomAngle = Random.Range(-spreadAngle, spreadAngle);

        // Xoay thêm 1 góc lệch nhỏ quanh trục Z
        Quaternion spreadRotation = firePos.rotation * Quaternion.Euler(0, 0, randomAngle);

        // Tạo viên đạn
        Instantiate(bullet, firePos.position, spreadRotation);
    }
}

using UnityEngine;

// Lớp Shotgun kế thừa tất cả các thuộc tính và hàm từ lớp Gun
public class Shotgun : Gun
{
    [Header("Shotgun Specifics")]
    [SerializeField] private int bulletsPerShot = 8; // Số lượng viên đạn (mảnh) bắn ra trong 1 lần
    [SerializeField] private float spreadAngle = 25f; // Độ tỏa tối đa của chùm đạn (tính bằng độ)

    // Ghi đè (override) hàm Shoot() của lớp Gun để có hành vi riêng
    protected override void Shoot()
    {
        // Kiểm tra xem có thể bắn không (dựa trên thời gian và input)
        if (shoot.action.ReadValue<float>() > 0 && Time.time >= nextShot)
        {
            // Bắn ra một chùm đạn với số lượng đã định
            for (int i = 0; i < bulletsPerShot; i++)
            {
                FireWithSpread();
            }

            // Đặt lại thời gian hồi chiêu sau khi bắn
            nextShot = Time.time + shotDelay;
        }
    }

    private void FireWithSpread()
    {
        // 1. Tính toán một góc lệch ngẫu nhiên trong khoảng -spread/2 đến +spread/2
        float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);

        // 2. Tạo một Quaternion xoay dựa trên góc lệch đó
        // Quaternion.Euler(0, 0, randomAngle) tạo ra một độ xoay quanh trục Z
        Quaternion spreadRotation = firePos.rotation * Quaternion.Euler(0, 0, randomAngle);

        // 3. Tạo ra viên đạn tại vị trí và hướng đã được điều chỉnh độ tỏa
        Instantiate(bullet, firePos.position, spreadRotation);
    }
}
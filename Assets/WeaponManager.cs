using UnityEngine;
using UnityEngine.InputSystem; // Cần dùng Input System
using System.Collections.Generic; // Cần dùng List

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private List<GameObject> weapons; // Danh sách các vũ khí
    [SerializeField] private InputActionReference switchAction; // Nút để chuyển vũ khí (E)

    private int currentWeaponIndex = 0;

    private void OnEnable()
    {
        switchAction.action.Enable();
        // Đăng ký sự kiện 'performed' (khi nút được nhấn)
        switchAction.action.performed += _ => SwitchWeapon();
    }



    private void OnDisable()
    {
        // Hủy đăng ký sự kiện để tránh lỗi
        switchAction.action.performed -= _ => SwitchWeapon();
        switchAction.action.Disable();
    }

    // === HÀM START ĐÃ ĐƯỢC SỬA LỖI ===
    void Start()
    {
        // 1. TẮT TẤT CẢ các súng trước.
        // Điều này đảm bảo mọi súng đều ở trạng thái "tắt"
        // và sẵn sàng nhận lệnh OnEnable() một cách chính xác.
        foreach (GameObject weapon in weapons)
        {
            if (weapon != null)
            {
                weapon.SetActive(false);
            }
        }

        // 2. Bây giờ, chỉ BẬT súng đầu tiên (index 0)
        // (Kiểm tra an toàn nếu danh sách không rỗng)
        if (weapons.Count > 0 && weapons[currentWeaponIndex] != null)
        {
            // Vì súng này 100% đang tắt, lệnh SetActive(true)
            // sẽ kích hoạt OnEnable() của súng.
            weapons[currentWeaponIndex].SetActive(true);
        }
    }

    private void SwitchWeapon()
    {
        // === SỬA LỖI: Thêm kiểm tra null (Phần này đã đúng) ===
        // 1. Lấy vũ khí hiện tại
        GameObject currentWeapon = weapons[currentWeaponIndex];

        // 2. Kiểm tra xem nó có còn tồn tại không TRƯỚC KHI sử dụng
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }
        // === KẾT THÚC SỬA LỖI ===

        // 3. Chuyển sang index của vũ khí tiếp theo
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;

        // === SỬA LỖI: Thêm kiểm tra null (Phần này đã đúng) ===
        // 4. Lấy vũ khí mới
        GameObject newWeapon = weapons[currentWeaponIndex];

        // 5. Kiểm tra xem nó có tồn tại không TRƯỚC KHI kích hoạt
        if (newWeapon != null)
        {
            newWeapon.SetActive(true);
        }
        // === KẾT THÚC SỬA LỖI ===
    }
}
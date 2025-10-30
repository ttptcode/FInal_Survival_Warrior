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

    void Start()
    {
        // Lúc bắt đầu, chỉ bật vũ khí đầu tiên và tắt hết các vũ khí còn lại
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == currentWeaponIndex);
        }
    }

    private void SwitchWeapon()
    {
        // 1. Tắt vũ khí hiện tại
        weapons[currentWeaponIndex].SetActive(false);

        // 2. Chuyển sang index của vũ khí tiếp theo
        // Dùng toán tử modulo (%) để quay vòng lại từ đầu khi đến cuối danh sách
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;

        // 3. Bật vũ khí mới
        weapons[currentWeaponIndex].SetActive(true);

        Debug.Log("Switched to: " + weapons[currentWeaponIndex].name);
    }
}
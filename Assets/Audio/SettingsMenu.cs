using UnityEngine;
using UnityEngine.UI; // Cần dùng cho Slider

// Gắn script này vào GameObject "SettingsPanel"
public class SettingsMenu : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // Hàm này được gọi khi Panel được bật (SetActive(true))
    private void Start()
    {
        // 1. Gán giá trị đã lưu cho Slider khi mở
        if (AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        }

        // 2. Thêm "Listeners" (sự kiện) cho Slider
        // Khi kéo slider, nó sẽ tự động gọi hàm tương ứng
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    // Hàm được gọi bởi musicSlider
    public void OnMusicSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    // Hàm được gọi bởi sfxSlider
    public void OnSFXSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }

    // (Tùy chọn) Hủy đăng ký listener khi tắt
    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Cinemachine; // Cần dùng cho Image

public class PlayerHitFeedback : MonoBehaviour
{
    // Dùng Singleton để PlayerStats dễ dàng gọi
    public static PlayerHitFeedback Instance;

    [Header("Screen Flash")]
    [SerializeField] private Image redFlashPanel; // Kéo Panel đỏ (che toàn màn hình) vào đây
    [SerializeField] private float flashDuration = 0.25f; // Thời gian chớp đỏ
    [SerializeField] [Range(0, 1)] private float maxAlpha = 0.5f; // Độ mờ tối đa (0.5 = 50%)

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.15f; // Thời gian rung
    [SerializeField] private float shakeMagnitude = 0.1f; // Cường độ rung

    public CinemachineCamera mainCamera;
    private Coroutine flashCoroutine;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Ẩn panel đi khi bắt đầu
        if (redFlashPanel != null)
        {
            redFlashPanel.color = new Color(1f, 0f, 0f, 0f);
            redFlashPanel.gameObject.SetActive(false);
        }
        
    }

    // PlayerStats sẽ gọi hàm này
    public void PlayHitEffect()
    {
        // Chớp đỏ
        if (redFlashPanel != null)
        {
            // Dừng coroutine cũ nếu đang chạy để tránh lỗi
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            flashCoroutine = StartCoroutine(ScreenFlash());
        }

        // Rung màn hình
        if (mainCamera != null)
        {
            if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(ScreenShake());
        }
    }

    private IEnumerator ScreenFlash()
    {
        redFlashPanel.gameObject.SetActive(true);
        float elapsedTime = 0f;
        
        // Fade In (Mờ dần -> Đỏ)
        while (elapsedTime < flashDuration / 2)
        {
            float alpha = Mathf.Lerp(0f, maxAlpha, elapsedTime / (flashDuration / 2));
            redFlashPanel.color = new Color(1f, 0f, 0f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f; // Reset thời gian

        // Fade Out (Đỏ -> Mờ dần)
        while (elapsedTime < flashDuration / 2)
        {
            float alpha = Mathf.Lerp(maxAlpha, 0f, elapsedTime / (flashDuration / 2));
            redFlashPanel.color = new Color(1f, 0f, 0f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        redFlashPanel.color = new Color(1f, 0f, 0f, 0f); // Đảm bảo alpha về 0
        redFlashPanel.gameObject.SetActive(false);
    }

    private IEnumerator ScreenShake()
    {
        Vector3 originalPos = mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Tạo một vị trí rung ngẫu nhiên
            float xOffset = Random.Range(-0.5f, 0.5f) * shakeMagnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * shakeMagnitude;

            // Áp dụng vị trí rung (giữ nguyên trục Z)
            mainCamera.transform.position = new Vector3(originalPos.x + xOffset, originalPos.y + yOffset, originalPos.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đặt camera về vị trí gốc sau khi rung xong
        mainCamera.transform.position = originalPos;
    }
}
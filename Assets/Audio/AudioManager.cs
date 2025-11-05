using UnityEngine;

// Đảm bảo GameObject này luôn có 2 AudioSource
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [Tooltip("AudioSource dành cho nhạc nền (BGM) - Sẽ được set 'Loop'")]
    [SerializeField] private AudioSource musicSource;

    [Tooltip("AudioSource dành cho hiệu ứng (SFX) - Sẽ dùng 'PlayOneShot'")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music (BGM)")]
    public AudioClip mainMenuMusic;
    public AudioClip inGameMusic;

    [Header("Sound Effects (SFX)")]
    public AudioClip gunShootSound;
    public AudioClip dashSound;
    public AudioClip coinCollectSound;
    public AudioClip playerHitSound;
    public AudioClip enemyDieSound;
    public AudioClip buyItemSound;
    public AudioClip errorSound; // Dùng cho "Không đủ tiền"
    public AudioClip shotGunSound; // Dùng cho "Nâng cấp thành công"

    private void Awake()
    {
        // --- Thiết lập Singleton ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không phá hủy khi chuyển scene
        }
        else
        {
            // Nếu đã có 1 bản, hủy bản mới này
            Destroy(gameObject);
        }
    }

    // --- Các hàm BGM (Nhạc nền) ---

    // Gọi hàm này từ GameUiManager khi vào Main Menu
    public void PlayMainMenuMusic()
    {
        // Chỉ phát nếu clip chưa phải là nhạc menu
        if (musicSource.clip == mainMenuMusic) return;

        musicSource.clip = mainMenuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Gọi hàm này từ GameUiManager khi nhấn "Start Game"
    public void PlayInGameMusic()
    {
        if (musicSource.clip == inGameMusic) return;

        musicSource.clip = inGameMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // --- Các hàm SFX (Hiệu ứng) ---

    // Một hàm chung để phát 1 tiếng động
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        // PlayOneShot cho phép nhiều SFX phát chồng lên nhau
        // mà không cắt ngang nhạc nền
        sfxSource.PlayOneShot(clip);
    }

    // --- Các hàm SFX cụ thể (Để các script khác gọi) ---

    // Gọi từ Gun.cs hoặc Shotgun.cs
    public void PlayGunShootSound()
    {
        PlaySFX(gunShootSound);
    }

    // Gọi từ PlayerMovement.cs (trong Coroutine Dash)
    public void PlayDashSound()
    {
        PlaySFX(dashSound);
    }

    // Gọi từ Coin.cs (trong OnTriggerEnter2D)
    public void PlayCoinCollectSound()
    {
        PlaySFX(coinCollectSound);
    }

    // Gọi từ PlayerStats.cs (trong TakeDamage)
    public void PlayPlayerHitSound()
    {
        PlaySFX(playerHitSound);
    }

    // Gọi từ Enemy.cs (trong Die)
    public void PlayEnemyDieSound()
    {
        PlaySFX(enemyDieSound);
    }

    // Gọi từ ShopManager.cs (trong BuyItem, nếu thành công)
    public void PlayBuyItemSound()
    {
        PlaySFX(buyItemSound);
    }

    // Gọi từ ShopManager.cs (nếu không đủ tiền)
    public void PlayErrorSound()
    {
        PlaySFX(errorSound);
    }
    public void PlayShotGunSound()
    {
        PlaySFX(shotGunSound);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
        // Lưu cài đặt
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    // Hàm này được gọi bởi Slider
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
        // Lưu cài đặt
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    // Hàm này được SettingsMenu gọi khi Start
    public float GetMusicVolume()
    {
        // Lấy giá trị đã lưu, nếu không có thì mặc định là 1 (tối đa)
        return PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    // Hàm này được gọi ở Awake
    private void LoadVolumeSettings()
    {
        // Áp dụng âm lượng đã lưu khi khởi động
        SetMusicVolume(GetMusicVolume());
        SetSFXVolume(GetSFXVolume());
    }
}
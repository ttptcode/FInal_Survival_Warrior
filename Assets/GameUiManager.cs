using UnityEngine;
using UnityEngine.SceneManagement; // Cần để tải lại scene hoặc về menu
using UnityEngine.InputSystem; // Cần cho Input System mới
using System.Collections;
using UnityEngine.UI; // <-- THÊM MỚI: Cần cho Button

// Gắn script này lên GameObject "GameUi"
public class GameUiManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static GameUiManager Instance;
    private static bool isLoadingFromGame = false;


    [Header("Panel References (Kéo từ Hierarchy)")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stopGamePanel; // Panel "Tạm dừng"
    [SerializeField] private GameObject MoneyDisplay; // Panel "Tạm dừng"
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject howToPlayPanel; // <-- THÊM MỚI



    // === THÊM MỚI: Tham chiếu đến nút Continue ===
    [SerializeField] private Button continueButton;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference pauseAction;

    private bool isPaused = false;
    private bool isGameOver = false;

    private void Awake()
    {
        // Thiết lập Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Kích hoạt input

        pauseAction.action.Enable();
        pauseAction.action.performed += _ => TogglePauseGame();
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        // === THAY ĐỔI: Kiểm tra cờ (flag) trước khi hiển thị Menu ===
        if (isLoadingFromGame)
        {
            // Nếu chúng ta vừa tải lại từ nút "Start Game"
            // -> không hiển thị menu, chạy game luôn

            if (MoneyDisplay != null) MoneyDisplay.SetActive(true);
            HideAllPanels();
            Time.timeScale = 1f;
            isPaused = false;
            isGameOver = false;
            isLoadingFromGame = false; // Reset cờ lại
        }
        else
        {
            // Nếu đây là lần đầu tiên mở game
            // -> hiển thị Main Menu
            ShowMainMenuPanel();
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký input
        pauseAction.action.Disable();
        pauseAction.action.performed -= _ => TogglePauseGame();
    }

    // --- Chức năng chính (gọi từ Input) ---

    private void TogglePauseGame()
    {
        // Không thể Pause khi đang ở Main Menu hoặc Game Over
        if (this == null)
        {
            return;
        }
        if (mainMenuPanel.activeSelf || isGameOver || (settingsPanel != null && settingsPanel.activeSelf))
        {
            return;
        }

        isPaused = !isPaused;

        if (isPaused)
        {
            ShowPausePanel();
        }
        else
        {
            ResumeGame();
        }
    }

    // --- Các hàm Tắt/Mở Panel ---

    private void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        stopGamePanel.SetActive(false);
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        // settingsPanel.SetActive(false);
    }

    public void ShowMainMenuPanel()
    {
        HideAllPanels();
        MoneyDisplay.SetActive(false);
        mainMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Dừng game khi ở Main Menu
        isPaused = false;
        isGameOver = false;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMainMenuMusic();
        }

        // === THÊM MỚI: Ẩn nút Continue khi mới vào game ===
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    // PlayerStats sẽ gọi hàm này khi chết
    public void ShowGameOverPanel()
    {
        HideAllPanels();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Dừng game
        isGameOver = true;
        isPaused = false; // <-- THÊM MỚI: Game over thì không còn là pause
    }

    public void ShowPausePanel()
    {
        HideAllPanels();
        if (MoneyDisplay != null) MoneyDisplay.SetActive(false);
        stopGamePanel.SetActive(true);
        Time.timeScale = 0f; // Dừng game
        isPaused = true;
    }

    // --- Các hàm cho Nút (Buttons) ---

    // Gắn vào nút "StarGame"
    public void StartGame()
    {
        // === THAY ĐỔI: Đặt cờ (flag) trước khi tải lại ===
        HideAllPanels();
        Time.timeScale = 1f;

        isLoadingFromGame = true; // Báo cho lần tải sau biết là "đang chơi mới"
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayInGameMusic();
        }

        // Tải lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Gắn vào nút "Resume" (trong StopGame) VÀ NÚT "CONTINUE" (trong MainMenu)
    public void ResumeGame()
    {
        HideAllPanels();
        if (MoneyDisplay != null) MoneyDisplay.SetActive(true);
        Time.timeScale = 1f; // Tiếp tục chạy game
        isPaused = false;
    }

    // Gắn vào nút "MainMenu_Button" (trong GameOver) và "MainMenu" (trong StopGame)
    public void GoToMainMenu()
    {
        // === THAY ĐỔI: Logic hiển thị nút Continue ===
        HideAllPanels();
        mainMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Vẫn dừng game
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMainMenuMusic();
        }
        // Chỉ hiển thị nút "Continue" nếu game đang ở trạng thái Pause
        // (Nếu game over, isPaused sẽ là false)
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(isPaused);
        }

        isGameOver = false; // Reset cờ này vì đã về menu
    }

    // Gắn vào nút "Settings" (nếu có)
    public void OpenSettings()
    {
        // === THAY ĐỔI: Logic mở Settings ===
        // Mở Panel Cài đặt (nó sẽ hiện đè lên MainMenu hoặc PauseMenu)
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Chưa gán Settings Panel!");
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    // Gắn vào nút "HowToPlay" (nếu có)
    public void OpenHowToPlay()
    {
        // Mở panel Hướng dẫn (hiện đè lên panel hiện tại)
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Chưa gán HowToPlay Panel!");
        }
    }

    // === THÊM MỚI ===
    // Gắn vào nút "Back" trong HowToPlayPanel
    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }


    // Gắn vào nút "Exit"
    public void ExitGame()
    {
        Debug.Log("Thoát Game!");
        Application.Quit();

#if UNITY_EDITOR
        // Dòng này để thoát game khi chạy trong Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
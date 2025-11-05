using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; // Cần dùng thư viện này cho Coroutine

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    [Header("Movement")]
    public PlayerStats playerStats; // <-- THÊM MỚI
    public InputActionReference move;

    [Header("Dash")]
    public InputActionReference dashAction; // Tham chiếu đến Action 'Dash'
    public float dashSpeed = 20f;         // Tốc độ khi dash
    public float dashDuration = 0.25f;    // Thời gian dash (như bạn yêu cầu)
    public float dashCooldown = 1f;       // Thời gian hồi chiêu

    private bool isDashing = false;
    private bool canDash = true;

    // Kích hoạt các input action
    private void Start()
    {
        // Lấy script PlayerStats (vì nó cùng nằm trên Player)
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerMovement không tìm thấy PlayerStats! Hãy chắc chắn nó được gắn trên cùng một GameObject.");
        }
    }
    private void OnEnable()
    {
        move.action.Enable();
        dashAction.action.Enable();

        // Đăng ký hàm TryDash() vào sự kiện 'performed' (khi nút được nhấn)
        dashAction.action.performed += _ => TryDash();
    }

    // Hủy đăng ký khi tắt
    private void OnDisable()
    {
        move.action.Disable();
        dashAction.action.Disable();
        dashAction.action.performed -= _ => TryDash();
    }

    void Update()
    {
        // Nếu đang dash, Coroutine sẽ xử lý chuyển động, hàm Update không làm gì cả
        if (isDashing)
        {
            return;
        }

        // Xử lý di chuyển bình thường
        Vector2 input = move.action.ReadValue<Vector2>();
        rb.linearVelocity = input.normalized * playerStats.GetTotalMoveSpeedMultiplier();

        // Xử lý lật sprite
        if (input.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (input.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // Chỉ chạy animation "isRunning" khi đang di chuyển VÀ không dash
        bool isRunning = move.action.ReadValue<Vector2>().normalized != Vector2.zero && !isDashing;
        animator.SetBool("isRunning", isRunning);
    }

    // Hàm này được gọi khi nhấn nút Dash
    private void TryDash()
    {
        // Chỉ dash nếu có thể (không đang hồi chiêu) và không đang dash
        if (this == null)
        {
            return;
        }
        if (canDash && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    // Coroutine xử lý logic Dash
    private IEnumerator DashCoroutine()
    {
        // --- Bắt đầu Dash ---
        canDash = false;    // Bắt đầu hồi chiêu
        isDashing = true;
        animator.SetBool("isDashing", true); // YÊU CẦU 1: Set bool 'isDashing' thành true

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDashSound();
        }

        // --- Logic di chuyển (Bonus) ---
        float originalGravity = rb.gravityScale; // Lưu lại trọng lực
        rb.gravityScale = 0f; // Tắt trọng lực khi dash

        // Lấy hướng di chuyển, nếu đứng yên thì dash theo hướng đang nhìn
        Vector2 dashDir = move.action.ReadValue<Vector2>().normalized;
        if (dashDir == Vector2.zero)
        {
            dashDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        }

        rb.linearVelocity = dashDir * dashSpeed; // Áp dụng tốc độ dash
        // --- Kết thúc Logic di chuyển ---

        // YÊU CẦU 2: Chờ 0.25 giây
        yield return new WaitForSeconds(dashDuration);

        // --- Kết thúc Dash ---
        isDashing = false;
        animator.SetBool("isDashing", false); // YÊU CẦU 3: Set bool 'isDashing' thành false
        rb.gravityScale = originalGravity; // Khôi phục trọng lực
        // rb.linearVelocity = Vector2.zero; // Dừng lại (tùy chọn) 
        // Hàm Update() sẽ tự động lấy lại quyền kiểm soát ở frame tiếp theo

        // --- Hồi chiêu (Cooldown) ---
        yield return new WaitForSeconds(dashCooldown); // Chờ hết thời gian hồi chiêu
        canDash = true; // Cho phép dash trở lại
    }
}
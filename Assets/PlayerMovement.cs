using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D rb;
    public InputActionReference move;
    public float moveSpeed;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    
    // [Header("Gun System")]
    // public PlayerGunController gunController;
    void Start()
    {
        // // Tự động tìm gun controller nếu chưa được gán
        // if (gunController == null)
        // {
        //     gunController = GetComponent<PlayerGunController>();
        //     if (gunController == null)
        //     {
        //         gunController = gameObject.AddComponent<PlayerGunController>();
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = move.action.ReadValue<Vector2>();
        rb.linearVelocity = input.normalized * moveSpeed;
        if (input.x < 0)
        {
            spriteRenderer.flipX = true;

        } else if (input.x >0) spriteRenderer.flipX = false;
        UpdateAnimation();
    }
    private void UpdateAnimation()
    {
        animator.SetBool("isRunning", move.action.ReadValue<Vector2>().normalized != Vector2.zero);
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    public PlayerMovement player2; // Tham chiếu đến player script
    public NavMeshAgent agent;
    public Image healthBar;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float maxHealth = 100f;
    public float currentHealth;

    private bool facingRight = true;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
    }


    void Awake()
    {
        // Cho phép NavMesh hoạt động trên mặt phẳng 2D
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        // Cập nhật điểm đến là vị trí player
        agent.SetDestination(player.position);


        // Lật hướng theo hướng di chuyển
        Flip();
    }

    private void Flip()
    {
        if (agent.velocity.x > 0 && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else if (agent.velocity.x < 0 && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // Gọi khi enemy tấn công trúng player
    public void KillPlayer()
    {
        if (player2 != null)
        {

            // Hủy player
            Destroy(player2.gameObject);
        }
    }
    public virtual void TakeDamage(float damage)
    {
        // Nếu đã chết rồi thì không nhận thêm sát thương nữa
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHpBar();

        if (currentHealth <= 0)
        {
            // Đánh dấu là đã chết và gọi Die()
            isDead = true;
            Die();
        }
    }
    protected virtual void Die()
    {
        // Trigger animation chết (nếu có)
        animator.SetTrigger("die");

        // Vô hiệu hoá Collider để không bị bắn trúng nữa
        GetComponent<Collider2D>().enabled = false;

        // Dừng agent lại MỘT CÁCH AN TOÀN
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero; // Dừng hẳn chuyển động vật lý
        }

        // Hủy game object ngay lập tức
        Destroy(gameObject);
    }
    protected void UpdateHpBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;


        }
    }
}

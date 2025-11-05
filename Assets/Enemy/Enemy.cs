using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    public PlayerMovement player2;
    public NavMeshAgent agent;
    public Image healthBar;

    // THAY ĐỔI: Chuyển sang protected để lớp con có thể truy cập
    protected PlayerStats playerStats;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float maxHealth = 100f;
    public float currentHealth;
    public float doDamage = 20f;

    // THAY ĐỔI: Chuyển sang protected
    protected bool facingRight = true;
    protected bool isDead = false;

    [Header("Loot")]
    [Tooltip("Kéo Prefab Coin của bạn vào đây")]
    [SerializeField] private GameObject coinPrefab;

    public void Setup(float bonusHP)
    {
        maxHealth += bonusHP;
        currentHealth = maxHealth;
    }

    protected virtual void Start() // THAY ĐỔI: Thêm 'virtual'
    {
        // Tự động tìm PlayerStats nếu chưa gán
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }

        if (playerStats == null)
        {
            // Dự phòng tìm bằng Instance (Singleton)
            playerStats = PlayerStats.Instance;
        }
    }

    void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    // THAY ĐỔI: Thêm 'protected virtual'
    protected virtual void Update()
    {
        // Kiểm tra Player đã chết chưa
        if (isDead || player == null || (playerStats != null && playerStats.isDead))
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
            if (playerStats != null && playerStats.isDead)
            {
                //animator.SetBool("isPlayerDead", true);
            }
            return;
        }

        // Logic di chuyển cơ bản
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false; // Kẻ thù 'Enemy' cơ bản luôn di chuyển
            agent.SetDestination(player.position);
        }

        bool isMoving = agent.velocity.magnitude > 0.1f;
        //animator.SetBool("isMoving", isMoving);

        Flip();
    }

    // THAY ĐỔI: Thêm 'protected virtual'
    protected virtual void Flip()
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

    public void AttackPlayer()
    {
        if (playerStats != null)
        {
            playerStats.TakeDamage(doDamage);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        // ... (Code TakeDamage giữ nguyên) ...
        if (isDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHpBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // ... (Code Die giữ nguyên) ...
        if (isDead) return;
        isDead = true;
        animator.SetTrigger("die");
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        GetComponent<Collider2D>().enabled = false;
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 2f);
    }

    protected void UpdateHpBar()
    {
        // ... (Code UpdateHpBar giữ nguyên) ...
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
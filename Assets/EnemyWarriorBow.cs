using UnityEngine;

public class EnemyWarriorBow : Enemy
{
    [Header("Warrior Bow Settings")]
    [SerializeField] private float shootingRange = 10f; // Tầm bắn cung tối đa

    [Header("Ranged Attack")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float arrowDamage = 10f; // Sát thương của mũi tên

    private float timeUntilNextShot = 0f;

    // Ghi đè hàm Update() của lớp Enemy
    protected override void Update()
    {
        // 1. Kiểm tra các điều kiện chết (giống như lớp cha)
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

        // 2. Tính toán khoảng cách
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //// 3. Cập nhật cooldown
        //if (timeUntilNextShot > 0)
        //{
        //    timeUntilNextShot -= Time.deltaTime;
        //}

        // 4. Logic Quyết định Hành vi

        //// --- A. Nếu Player ở quá gần -> Chém (Melee) ---
        //if (distanceToPlayer <= meleeRange)
        //{
        //    // Dừng di chuyển
        //    if (agent.isOnNavMesh)
        //    {
        //        agent.isStopped = true;
        //        agent.SetDestination(transform.position);
        //    }
        //    animator.SetBool("isMoving", false);

        //    // Kích hoạt bool "isTouchPlayer" để Animator chạy animation chém
        //    // (Giống script EnemyAttackTrigger của bạn)
        //    animator.SetBool("isTouchPlayer", true);

        //    // Quay mặt về phía Player
        //    FacePlayer();
        //}

        // --- B. Nếu Player ở tầm bắn cung -> Bắn (Ranged) ---
        if (distanceToPlayer <= shootingRange)
        {
            // Dừng di chuyển
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.SetDestination(transform.position);
            }
            animator.SetBool("isTouchPlayer", false); 
            animator.SetBool("shoot", true); 

            // Quay mặt về phía Player
            FacePlayer();

            // Nếu đã hồi chiêu, bắn tên
            //if (timeUntilNextShot <= 0)
            //{
            //    ShootArrow();
            //    timeUntilNextShot = shootCooldown;
            //}
        }

        // --- C. Nếu Player ở quá xa -> Đuổi theo (Chase) ---
        else
        {
            // Tiếp tục di chuyển
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            animator.SetBool("isTouchPlayer", false);
            animator.SetBool("shoot", false);

            // Lật sprite (Flip)
            Flip(); // Gọi hàm Flip() của lớp cha
        }
    }

    private void ShootArrow()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null) return;

        // Kích hoạt animation bắn cung
        animator.SetTrigger("shoot"); // (Bạn cần tạo Trigger "shoot" trong Animator)

        // Tạo mũi tên
        GameObject arrowGO = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        // Lấy script của mũi tên và "ra lệnh" cho nó
        Arrow arrowScript = arrowGO.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Setup(arrowDamage, (player.position - arrowSpawnPoint.position).normalized);
        }
    }

    // Hàm riêng để quay mặt (không lật sprite) khi đứng yên
    private void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;

        // Nếu Player ở bên phải VÀ Enemy đang nhìn bên trái
        if (direction.x > 0 && !facingRight)
        {
            // Lật sang phải
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        // Nếu Player ở bên trái VÀ Enemy đang nhìn bên phải
        else if (direction.x < 0 && facingRight)
        {
            // Lật sang trái
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
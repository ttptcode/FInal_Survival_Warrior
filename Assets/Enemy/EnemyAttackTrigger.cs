using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Lấy animator từ object cha (Enemy)
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player vào vùng tấn công!");
            if (animator != null)
                animator.SetBool("isTouchPlayer", true);
            else
                Debug.LogWarning("⚠️ Animator chưa được gán trong EnemyAttackTrigger!");
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player ra khỏi vùng tấn công!");
            if (animator != null)
            {
                animator.SetBool("isTouchPlayer", false);

            }
        }
    }
}

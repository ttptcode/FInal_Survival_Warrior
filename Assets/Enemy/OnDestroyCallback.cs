using UnityEngine;
using System; // Cần thiết cho 'Action'

public class OnDestroyCallback : MonoBehaviour
{
    // 'Action' là một 'delegate' (sự kiện) mà các script khác có thể đăng ký
    public Action OnDestroyed;

    // Hàm này được Unity tự động gọi khi GameObject bị hủy
    private void OnDestroy()
    {
        // 'OnDestroyed?' kiểm tra xem có script nào (như EnemySpawner) đang lắng nghe không
        // Nếu có, nó sẽ 'Invoke' (gọi) sự kiện đó
        OnDestroyed?.Invoke();
    }
}

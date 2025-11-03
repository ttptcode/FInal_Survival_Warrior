using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] protected Camera mainCam;
    [SerializeField] protected InputActionReference aimAction;
    [SerializeField] protected Transform firePos;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float shotDelay = 0.15f;
    [SerializeField] protected InputActionReference shoot;
    protected float nextShot; // Cho phép class con dùng được

    protected virtual void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    protected virtual void OnEnable()
    {
        aimAction.action.Enable();
    }

    protected virtual void OnDisable()
    {
        aimAction.action.Disable();
    }

    protected virtual void Update()
    {
        RotateToMouse();
        Shoot();
    }

    // 👇 Cho phép class con (SubmachineGun) gọi được
    protected void RotateToMouse()
    {
        Vector2 mouseScreenPos = aimAction.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void Shoot()
    {
        if (shoot.action.ReadValue<float>() > 0 && Time.time >= nextShot)
        {
            Instantiate(bullet, firePos.position, firePos.rotation);
            nextShot = Time.time + shotDelay;
        }
    }
}

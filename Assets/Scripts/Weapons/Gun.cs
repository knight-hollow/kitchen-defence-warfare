using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    [Header("Refs")]
    public Transform muzzle;
    public Bullet bulletPrefab;

    [Header("Weapon Stats")]
    public int damage = 25;
    public float fireRate = 5f;          // 发/秒
    public float bulletSpeed = 20f;
    public float splashRadius = 0f;      // 0=无AOE，>0=范围伤害

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private bool isHeld = false;
    private bool triggerHeld = false;

    private float nextFireTime = 0f;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab == null)
        {
            Debug.LogError("[Gun] 缺少 XRGrabInteractable");
            return;
        }

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);

        // 扳机按下/松开（XRIT的 Activate）
        grab.activated.AddListener(OnActivated);
        grab.deactivated.AddListener(OnDeactivated);
    }

    private void OnDestroy()
    {
        if (grab == null) return;
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
        grab.activated.RemoveListener(OnActivated);
        grab.deactivated.RemoveListener(OnDeactivated);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        triggerHeld = false;
    }

    private void OnActivated(ActivateEventArgs args)
    {
        if (!isHeld) return;
        triggerHeld = true;
    }

    private void OnDeactivated(DeactivateEventArgs args)
    {
        triggerHeld = false;
    }

    private void Update()
    {
        if (!isHeld || !triggerHeld) return;

        Debug.Log("TriggerHeld TRUE");   // ✅ 看看有没有进到这里

        if (Time.time < nextFireTime) return;

        Debug.Log("FIRE!");              // ✅ 看看有没有触发发射

        FireOnce();
        float interval = (fireRate <= 0.01f) ? 0.1f : (1f / fireRate);
        nextFireTime = Time.time + interval;
    }


    private void FireOnce()
    {
        if (muzzle == null || bulletPrefab == null) return;

        Bullet b = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        b.Init(damage, bulletSpeed, splashRadius);
    }
}

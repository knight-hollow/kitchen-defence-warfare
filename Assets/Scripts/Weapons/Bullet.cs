using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;

    private int damage;
    private float speed;
    private float splashRadius;

    private Rigidbody rb;

    private bool inited = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(int dmg, float spd, float splash)
    {
        damage = dmg;
        speed = spd;
        splashRadius = splash;

        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }

        inited = true;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!inited) return;

        // 命中怪物
        MonsterHealth mh = collision.collider.GetComponentInParent<MonsterHealth>();
        if (mh != null)
        {
            if (splashRadius > 0.01f)
            {
                DoSplashDamage(transform.position);
            }
            else
            {
                mh.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void DoSplashDamage(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(center, splashRadius);
        foreach (var h in hits)
        {
            MonsterHealth mh = h.GetComponentInParent<MonsterHealth>();
            if (mh != null)
            {
                mh.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (splashRadius > 0.01f)
        {
            Gizmos.DrawWireSphere(transform.position, splashRadius);
        }
    }
}

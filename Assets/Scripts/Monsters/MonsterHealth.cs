using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int hp;

    [HideInInspector]
    public WaveManager waveManager;

    private bool dead = false;

    private void Awake()
    {
        hp = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        if (dead) return;

        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
    }

    private void Die()
    {
        if (dead) return;
        dead = true;

        if (waveManager != null)
            waveManager.OnMonsterRemoved();

        Destroy(gameObject);
    }
}

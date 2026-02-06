using UnityEngine;

public class FrontDeskTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public WaveManager waveManager;

    private void OnTriggerEnter(Collider other)
    {
        MonsterHealth mh = other.GetComponentInParent<MonsterHealth>();
        if (mh == null) return;

        // 扣血
        if (gameManager != null)
            gameManager.TakeDamage(1);

        // 算作移除怪物（否则波次会卡）
        if (waveManager != null)
            waveManager.OnMonsterRemoved();

        Destroy(mh.gameObject);
    }
}

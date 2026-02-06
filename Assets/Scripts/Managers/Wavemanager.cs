using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Config")]
    public int[] waveCounts = new int[] { 15, 20, 30 };
    public float spawnInterval = 0.6f;

    [Header("Monster Prefabs (Random)")]
    [Tooltip("把4种怪物Prefab都拖到这里。每次刷怪会随机选一种生成。")]
    public GameObject[] monsterPrefabs;

    [Tooltip("可选：权重数组，长度需与 monsterPrefabs 相同。比如 [50,30,15,5] 表示A更常见。留空则均匀随机。")]
    public int[] spawnWeights;

    [Header("Target")]
    public Transform monsterTarget;

    [Header("Spawn Points")]
    public Transform spawnPointsParent;

    [Header("UI")]
    public UIManager uiManager;

    [Header("Game")]
    public GameManager gameManager;

    private Transform[] spawnPoints;

    private int currentWaveIndex = -1;
    private int spawnedThisWave = 0;
    private int alive = 0;
    private bool spawning = false;

    private void Awake()
    {
        CacheSpawnPoints();
    }

    private void CacheSpawnPoints()
    {
        if (spawnPointsParent == null)
        {
            spawnPoints = new Transform[0];
            return;
        }

        int childCount = spawnPointsParent.childCount;
        spawnPoints = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
            spawnPoints[i] = spawnPointsParent.GetChild(i);
    }

    public void StartWaves()
    {
        if (monsterTarget == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[WaveManager] 缺少 monsterTarget 或 spawnPoints。");
            return;
        }

        if (monsterPrefabs == null || monsterPrefabs.Length == 0)
        {
            Debug.LogError("[WaveManager] monsterPrefabs 为空！请拖入你的4种怪物Prefab。");
            return;
        }

        currentWaveIndex = -1;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waveCounts.Length)
        {
            if (gameManager != null)
                gameManager.EndGame(true); // 胜利
            return;
        }

        spawnedThisWave = 0;
        alive = 0;

        if (uiManager != null)
        {
            uiManager.UpdateWave(currentWaveIndex + 1, waveCounts.Length);
            uiManager.UpdateRemaining(alive);
        }

        if (!spawning)
            StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        spawning = true;

        int totalToSpawn = waveCounts[currentWaveIndex];

        while (spawnedThisWave < totalToSpawn)
        {
            // 暂停时不刷怪（Time.timeScale=0时 WaitForSeconds 也会停住，
            // 但这里更保险：如果你未来改成 Unscaled 的等待，也不会误刷）
            if (Time.timeScale > 0.0001f)
            {
                SpawnOne();
                spawnedThisWave++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        spawning = false;
        // 波次结束判定放在 OnMonsterRemoved 里：alive==0 且 spawnedThisWave==total
    }

    private void SpawnOne()
    {
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = PickRandomMonsterPrefab();
        GameObject m = Instantiate(prefab, sp.position, sp.rotation);

        // 设置目标点
        MonsterMove mv = m.GetComponent<MonsterMove>();
        if (mv != null)
            mv.target = monsterTarget;

        // 让怪物死亡时回调 WaveManager
        MonsterHealth mh = m.GetComponent<MonsterHealth>();
        if (mh != null)
            mh.waveManager = this;

        alive++;

        if (uiManager != null)
            uiManager.UpdateRemaining(alive);
    }

    private GameObject PickRandomMonsterPrefab()
    {
        // 1) 如果权重合法，用加权随机
        if (spawnWeights != null && spawnWeights.Length == monsterPrefabs.Length)
        {
            int sum = 0;
            for (int i = 0; i < spawnWeights.Length; i++)
                sum += Mathf.Max(0, spawnWeights[i]);

            if (sum > 0)
            {
                int r = Random.Range(0, sum);
                int acc = 0;
                for (int i = 0; i < spawnWeights.Length; i++)
                {
                    acc += Mathf.Max(0, spawnWeights[i]);
                    if (r < acc)
                        return monsterPrefabs[i];
                }
            }
        }

        // 2) 否则均匀随机
        return monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
    }

    /// <summary>
    /// 怪物被击杀 或 碰到前台被销毁，都必须调用
    /// </summary>
    public void OnMonsterRemoved()
    {
        alive--;
        if (alive < 0) alive = 0;

        if (uiManager != null)
            uiManager.UpdateRemaining(alive);

        int totalToSpawn = waveCounts[currentWaveIndex];
        if (!spawning && spawnedThisWave >= totalToSpawn && alive == 0)
        {
            StartNextWave();
        }
    }
}

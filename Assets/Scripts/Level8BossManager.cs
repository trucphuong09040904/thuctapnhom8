using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level8BossManager : MonoBehaviour
{
    public GameObject boss5Prefab, boss6Prefab, boss7Prefab;
    private GameObject currentBoss;
    public Transform spawnPoint;
    public Slider bossHealthBar; // Thanh máu chính

    void Start()
    {
        SpawnBoss(boss5Prefab, () => SpawnBoss(boss6Prefab, () => SpawnBoss(boss7Prefab, null)));
    }

    void SpawnBoss(GameObject bossPrefab, System.Action onBossDefeated)
    {
        currentBoss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        bossHealthBar.gameObject.SetActive(true);

        if (currentBoss.TryGetComponent(out LV8Boss5 boss5))
        {
            boss5.SetHealthBar(bossHealthBar);
            boss5.OnBossDefeated += () => { Destroy(currentBoss); onBossDefeated?.Invoke(); };
        }
        else if (currentBoss.TryGetComponent(out LV8Boss6 boss6))
        {
            boss6.SetHealthBar(bossHealthBar);
            boss6.OnBossDefeated += () => { Destroy(currentBoss); onBossDefeated?.Invoke(); };
        }
        else if (currentBoss.TryGetComponent(out LV8Boss7 boss7))
        {
            boss7.SetHealthBar(bossHealthBar);
            boss7.OnBossDefeated += () => { Destroy(currentBoss); onBossDefeated?.Invoke(); };
        }
    }
}

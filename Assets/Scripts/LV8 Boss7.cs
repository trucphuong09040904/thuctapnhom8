using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LV8Boss7 : MonoBehaviour
{
    public delegate void BossDefeatedHandler(LV8Boss7 boss);
    public static event BossDefeatedHandler OnBossDefeated;

    [Header("Health Settings")]
    public int maxHealth = 5000;
    private int currentHealth;
    public Slider healthBar;
    public int scorePerHit = 100;

    [Header("Waypoint Movement")]
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float waitTimeAtWaypoint = 2f;
    private int currentWaypointIndex = 0;

    [Header("Minion Settings")]
    public GameObject minionPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    void Update()
    {
        if (healthBar != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            healthBar.transform.position = screenPos;
        }
    }

    IEnumerator MoveToNextWaypoint()
    {
        while (true)
        {
            Transform target = waypoints[currentWaypointIndex];

            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(waitTimeAtWaypoint);

            int attackType = Random.Range(0, 2);
            if (attackType == 0)
                yield return StartCoroutine(CircularMinionAttack());
            else
                yield return StartCoroutine(VerticalMinionAttack());

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    IEnumerator CircularMinionAttack()
    {
        int minionCount = 8;
        float radius = 2f;
        Vector3 bossPosition = transform.position;

        for (int i = 0; i < minionCount; i++)
        {
            float angle = i * Mathf.PI * 2 / minionCount;
            Vector3 spawnPos = bossPosition + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            float angleDeg = angle * Mathf.Rad2Deg;

            GameObject minion = Instantiate(minionPrefab, spawnPos, Quaternion.Euler(0, 0, angleDeg));
            Minion minionScript = minion.GetComponent<Minion>();

            if (minionScript != null)
            {
                minionScript.SetTargetPosition(spawnPos);
                minionScript.SetCircularPattern(bossPosition);
            }
        }

        yield return new WaitForSeconds(10f);
    }

    IEnumerator VerticalMinionAttack()
    {
        int columns = 2;
        int rows = 3;
        float spacing = 1.5f;
        Vector3 startPosition = transform.position + new Vector3(-spacing, -spacing, 0);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 spawnPos = startPosition + new Vector3(j * spacing * 2, i * spacing, 0);
                GameObject minion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
                Minion minionScript = minion.GetComponent<Minion>();

                if (minionScript != null)
                {
                    minionScript.SetTargetPosition(spawnPos);
                    minionScript.SetVerticalPattern(currentWaypointIndex);
                }
            }
        }
        yield return new WaitForSeconds(10f);
    }

    public void SetHealthBar(Slider healthBar)
    {
        this.healthBar = healthBar;
        this.healthBar.maxValue = maxHealth;
        this.healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (GameScore.instance != null)
        {
            Debug.Log("🏆 Cộng điểm cho Player: " + scorePerHit);
            GameScore.instance.AddScore(scorePerHit);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 Boss đã bị tiêu diệt!");

        if (GameScore.instance != null)
        {
            Debug.Log("🏆 Cộng điểm khi tiêu diệt Boss!");
            GameScore.instance.AddScore(2000);
        }

        OnBossDefeated?.Invoke(this);

        Destroy(gameObject);
        Destroy(healthBar.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBulletTag"))
        {
            Debug.Log("💥 Boss bị bắn trúng! Mất 50 HP.");
            TakeDamage(50);
            Destroy(collision.gameObject);
        }
    }

}
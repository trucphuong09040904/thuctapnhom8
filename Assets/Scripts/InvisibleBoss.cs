using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InvisibleBoss : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5000;
    private int currentHealth;
    public GameObject explosionEffect;
    public Slider healthBar;
    public RectTransform healthBarUI;

    [Header("Stealth Mechanism")]
    public SpriteRenderer bossRenderer;
    public Collider2D bossCollider;
    public float visibleTime = 3f;
    public float invisibleTime = 3f;

    [Header("Attack Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    private bool isInvisible = false;

    [Header("Movement Settings")]
    public float moveInterval = 2f;
    public float minX = -3f, maxX = 3f;
    public float minY = 2f, maxY = 4f;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        StartCoroutine(InvisibilityCycle());
        StartCoroutine(FireBullets());
        StartCoroutine(RandomMove());
    }

    void Update()
    {
        if (healthBarUI != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            healthBarUI.position = screenPos;
        }
    }

    IEnumerator InvisibilityCycle()
    {
        while (currentHealth > 0)
        {
            bossRenderer.enabled = true;
            bossCollider.enabled = true;
            isInvisible = false;
            healthBarUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(visibleTime);

            bossRenderer.enabled = false;
            bossCollider.enabled = false;
            isInvisible = true;
            healthBarUI.gameObject.SetActive(false);
            yield return new WaitForSeconds(invisibleTime);
        }
    }

    IEnumerator FireBullets()
    {
        while (currentHealth > 0)
        {
            if (!isInvisible && firePoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.AddComponent<BulletLV8>(); // Gán script cho đạn của Boss
            }
            yield return new WaitForSeconds(fireRate);
        }
    }


    IEnumerator RandomMove()
    {
        while (currentHealth > 0)
        {
            float newX = Random.Range(minX, maxX);
            float newY = Random.Range(minY, maxY);
            transform.position = new Vector3(newX, newY, transform.position.z);

            yield return new WaitForSeconds(currentHealth <= maxHealth * 0.2f ? 1.5f : moveInterval);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvisible) return;

        currentHealth -= damage;
        healthBar.value = currentHealth;
        GameScore.instance.AddScore(100); // Cộng 100 điểm cho Player

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        StartCoroutine(FinalBulletStorm());
        Destroy(healthBarUI.gameObject);
        Destroy(gameObject);
    }

    IEnumerator FinalBulletStorm()
    {
        for (int i = 0; i < 10; i++)
        {
            float angle = i * 36f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(bulletPrefab, transform.position, rotation);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBulletTag"))
        {
            TakeDamage(100);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("PlayerShipTag"))
        {
            Debug.Log("🔥 Boss va chạm với Player! Gây 2 sát thương.");
            collision.GetComponent<PlayerController>()?.TakeDamage(2);
        }
    }
}



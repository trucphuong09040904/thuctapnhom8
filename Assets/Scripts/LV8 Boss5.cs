using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LV8Boss5 : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveRange = 5f;
    public float shootInterval = 2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform player;
    private HashSet<int> hitBulletIDs = new HashSet<int>();
    public int maxHealth = 5000;
    private int currentHealth;
    public Slider healthBar;
    public GameObject explosionEffect;
    public System.Action OnBossDefeated; // Sự kiện khi boss chết

    private Vector3 targetPosition;
    private Vector2 minScreenBounds, maxScreenBounds;
    private bool isEnraged = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        minScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(MoveRandomly());
        StartCoroutine(ShootAtPlayer());
    }

    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            float minY = minScreenBounds.y + (maxScreenBounds.y - minScreenBounds.y) * 0.25f;
            float maxY = maxScreenBounds.y;

            targetPosition = new Vector3(
                Random.Range(minScreenBounds.x, maxScreenBounds.x),
                Random.Range(minY, maxY),
                transform.position.z
            );

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);
            if (player != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.tag = "Bullet";
                Vector2 direction = (player.position - firePoint.position).normalized;
                bullet.GetComponent<Bullet>().SetDirection(direction, "Boss");
            }
        }
    }

    public void TakeDamage(int damage, int bulletID)
    {
        if (hitBulletIDs.Contains(bulletID)) return;
        hitBulletIDs.Add(bulletID);

        currentHealth -= damage;
        if (healthBar != null) healthBar.value = currentHealth;

        GameScore.instance.AddScore(100);

        if (currentHealth <= 2000 && !isEnraged)
        {
            EnterRageMode();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void EnterRageMode()
    {
        isEnraged = true;
        moveSpeed *= 3;
        Debug.Log("🔥 Boss đã vào trạng thái CUỒNG NỘ!");

        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.Shake(0.5f, 0.1f));
        }

        StartCoroutine(FlashRedEffect());
    }

    IEnumerator FlashRedEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Die()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        OnBossDefeated?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBulletTag"))
        {
            int bulletID = collision.gameObject.GetInstanceID();
            TakeDamage(100, bulletID);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("PlayerShipTag"))
        {
            Debug.Log("🔥 Boss va chạm với Player! Gây 2 sát thương.");
            collision.GetComponent<PlayerController>()?.TakeDamage(2);
        }
    }
}

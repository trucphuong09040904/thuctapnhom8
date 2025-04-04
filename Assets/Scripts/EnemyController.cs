using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject ExplosionGO; // Prefab vụ nổ
    public float baseSpeed = 2f; // Tốc độ cơ bản
    public float speedMultiplier = 1f; // Hệ số nhân tốc độ
    private float speed; // Tốc độ thực tế của enemy

    private GameScore gameScore;
    private bool isDestroyed = false;

    void Start()
    {
        gameScore = FindObjectOfType<GameScore>();
        if (gameScore == null)
        {
            Debug.LogError("EnemyController: Không tìm thấy GameScore!");
        }

        // Tính toán tốc độ thực tế
        speed = baseSpeed * speedMultiplier;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isDestroyed) return;

        if (col.CompareTag("PlayerShipTag") || col.CompareTag("PlayerBulletTag"))
        {
            isDestroyed = true;
            PlayExplosion();

            if (gameScore != null)
            {
                gameScore.Score += 100;
                Debug.Log("Enemy bị bắn! Điểm hiện tại: " + gameScore.Score);
            }

            if (col.CompareTag("PlayerBulletTag"))
            {
                Destroy(col.gameObject);
            }

            Destroy(gameObject);
        }
    }

    void PlayExplosion()
    {
        Instantiate(ExplosionGO, transform.position, Quaternion.identity);
    }

    // Hàm này cho phép thay đổi tốc độ enemy từ bên ngoài
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        speed = baseSpeed * speedMultiplier; // Cập nhật tốc độ mới
    }
}

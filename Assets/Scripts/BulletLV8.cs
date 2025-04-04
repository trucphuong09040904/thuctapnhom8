using UnityEngine;
using System.Collections;

public class BulletLV8 : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 targetPosition;
    private Rigidbody2D rb;

    void Start()
    {
        Transform boss = GameObject.FindGameObjectWithTag("BossM8")?.transform; // Lấy vị trí Boss
        rb = GetComponent<Rigidbody2D>();

        if (boss != null)
        {
            transform.position = boss.position; // Đạn bắt đầu từ vị trí của Boss
        }

        Transform player = GameObject.FindGameObjectWithTag("PlayerShipTag")?.transform;

        if (player != null)
        {
            targetPosition = player.position; // Hướng về phía Player
        }
        else
        {
            targetPosition = transform.position + Vector3.down * 10f; // Nếu không có Player, bắn xuống
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;

        Destroy(gameObject, 5f); // Hủy đạn sau 5 giây
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerShipTag"))
        {
            PlayerController player = col.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(2); // Gây sát thương 2 HP
                player.StartCoroutine(player.InvertControls(2f)); // Đảo ngược điều khiển trong 2 giây
            }
            Destroy(gameObject);
        }
    }
}

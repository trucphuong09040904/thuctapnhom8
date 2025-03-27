using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed = 5f;
    private int damage;

    public void SetTarget(Vector3 playerPosition, int bulletDamage)
    {
        // Tính hướng từ viên đạn đến Player
        direction = (playerPosition - transform.position).normalized;
        damage = bulletDamage;
    }

    void Update()
    {
        // Đạn di chuyển theo hướng đã tính sẵn
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerShipTag"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

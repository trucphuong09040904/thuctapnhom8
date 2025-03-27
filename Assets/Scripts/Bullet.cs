using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; // Tốc độ bay của đạn
    public float lifeTime = 5f; // Thời gian tự hủy nếu không va chạm

    private Vector2 moveDirection;
    private string shooterTag; // Lưu tag của người bắn

    void Start()
    {
        Destroy(gameObject, lifeTime); // Hủy đạn sau một thời gian
    }

    public void SetDirection(Vector2 direction, string shooter)
    {
        moveDirection = direction.normalized;
        shooterTag = shooter; // Gán tag của người bắn
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu đạn của Boss bắn trúng Player
        if (shooterTag == "Boss" && collision.CompareTag("PlayerShipTag"))
        {
            Debug.Log("🔥 Player bị trúng đạn Boss! Mất 2 HP.");
            collision.GetComponent<PlayerController>()?.TakeDamage(2);
            Destroy(gameObject);
        }


    }
}

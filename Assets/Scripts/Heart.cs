using UnityEngine;

public class Heart : MonoBehaviour
{
    public int healAmount = 2; // Số máu hồi lại khi bắn trúng
    public float fallSpeed = 2f; // Tốc độ rơi xuống
    public float spawnRangeX = 8f; // Phạm vi sinh ngẫu nhiên theo trục X

    private void Start()
    {
        // Đặt vị trí ngẫu nhiên khi sinh ra
        transform.position = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Camera.main.orthographicSize + 1, 0);
    }

    private void Update()
    {
        // Di chuyển trái tim xuống dưới
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Hủy nếu rơi khỏi màn hình
        if (transform.position.y < -Camera.main.orthographicSize - 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBulletTag")) // Nếu bị đạn bắn trúng
        {
            PlayerController.Instance.Heal(healAmount); // Hồi máu cho Player
            Destroy(collision.gameObject); // Hủy viên đạn
            Destroy(gameObject); // Hủy trái tim
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float speed = 2.0f; // Tốc độ rơi xuống
    private bool isCollected = false; // Ngăn chặn cộng điểm 2 lần

    void Update()
    {
        // Đồng xu di chuyển xuống dưới
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Xóa đồng xu nếu rơi ra khỏi màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Nếu coin đã bị thu thập, không xử lý va chạm nữa
        if (isCollected) return;

        Debug.Log("Va chạm với: " + col.gameObject.name); // Kiểm tra đối tượng va chạm

        if (col.CompareTag("PlayerBulletTag")) // Nếu đạn bắn trúng
        {
            Debug.Log("Đạn bắn trúng đồng xu!");

            isCollected = true; // Đánh dấu đã nhặt để ngăn chặn cộng điểm 2 lần

            // Cộng điểm
            GameScore gameScore = FindObjectOfType<GameScore>();
            if (gameScore != null)
            {
                gameScore.Score += 50;
                Debug.Log("Cộng 50 điểm! Tổng điểm: " + gameScore.Score);
            }

            // Xóa đạn và đồng xu
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}

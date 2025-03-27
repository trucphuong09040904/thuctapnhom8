using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 8f;
    

    void Update()
    {
        // Di chuyển viên đạn
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Nếu viên đạn đi ra khỏi màn hình, hủy nó
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

}

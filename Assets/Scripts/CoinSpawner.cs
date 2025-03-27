using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject CoinPrefab; // Prefab của đồng xu
    public float spawnRate = 3f;  // Thời gian xuất hiện coin

    void Start()
    {
        // Bắt đầu sinh coin liên tục
        InvokeRepeating("SpawnCoin", 1f, spawnRate);
    }

    void SpawnCoin()
    {
        // Lấy giới hạn ngang thực tế của màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Random vị trí ngang (X), đảm bảo trong khoảng màn hình
        float randomX = Random.Range(min.x + 1f, max.x - 1f); // Tránh spawn sát mép
        float spawnY = max.y + 1f; // Coin xuất hiện ngay phía trên màn hình

        Vector2 spawnPosition = new Vector2(randomX, spawnY);

        Instantiate(CoinPrefab, spawnPosition, Quaternion.identity);
    }
}

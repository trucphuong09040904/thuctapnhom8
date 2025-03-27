using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab trái tim
    public float spawnInterval = 5f; // Thời gian giữa mỗi lần spawn
    public float spawnRangeX = 8f; // Phạm vi xuất hiện theo trục X

    private void Start()
    {
        InvokeRepeating("SpawnHeart", 2f, spawnInterval); // Bắt đầu sinh trái tim liên tục
    }

    void SpawnHeart()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Camera.main.orthographicSize + 1, 0);
        Instantiate(heartPrefab, spawnPosition, Quaternion.identity);
    }
}

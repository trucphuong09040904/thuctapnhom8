using UnityEngine;
using UnityEngine.UI; // Để hiển thị thanh máu

public class BossController : MonoBehaviour
{
    public Transform bossMain;
    public Transform bossLeft;
    public Transform bossRight;
    public float moveSpeed = 2f;
    public float moveRange = 3f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPointMain;
    public Transform bulletSpawnPointLeft;
    public Transform bulletSpawnPointRight;
    public float shootInterval = 2f;
    public int bulletDamage = 2;
    public int maxHealth = 5000; // Máu của Boss chính
    private int currentHealth;

    public Slider bossHealthBar; // Thanh máu của Boss chính

    private float minX, maxX, minY, maxY;
    private Vector3 targetPosition;
    private Transform player;

    void Start()
    {
        float screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        minX = -screenHalfWidth + 2f;
        maxX = screenHalfWidth - 2f;
        minY = -2f;
        maxY = 4f;

        player = FindObjectOfType<PlayerController>()?.transform;

        currentHealth = maxHealth;
        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = maxHealth;
            bossHealthBar.value = currentHealth;
        }

        InvokeRepeating("Shoot", 1f, shootInterval);
        ChooseNewTargetPosition();
    }

    void Update()
    {
        MoveBosses();
        if (bossHealthBar != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(bossMain.position + new Vector3(0, 1.5f, 0));
            bossHealthBar.transform.position = screenPosition;
        }
    }

    void MoveBosses()
    {
        bossMain.position = Vector3.MoveTowards(bossMain.position, targetPosition, moveSpeed * Time.deltaTime);
        bossLeft.position = new Vector3(bossMain.position.x - 2f, bossMain.position.y, bossLeft.position.z);
        bossRight.position = new Vector3(bossMain.position.x + 2f, bossMain.position.y, bossRight.position.z);

        if (Vector3.Distance(bossMain.position, targetPosition) < 0.1f)
        {
            ChooseNewTargetPosition();
        }
    }

    void ChooseNewTargetPosition()
    {
        targetPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), bossMain.position.z);
    }

    void Shoot()
    {
        if (player == null) return;

        CreateBullet(bulletSpawnPointMain);
        CreateBullet(bulletSpawnPointLeft);
        CreateBullet(bulletSpawnPointRight);
    }

    void CreateBullet(Transform spawnPoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        BossBullet bossBullet = bullet.GetComponent<BossBullet>();

        if (bossBullet != null)
        {
            bossBullet.SetTarget(player.position, bulletDamage);
        }
    }

    // Xử lý khi bị bắn
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (bossHealthBar != null)
        {
            bossHealthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(bossMain.gameObject);
        Destroy(bossLeft.gameObject);
        Destroy(bossRight.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerShipTag"))
        {
            other.GetComponent<PlayerController>().TakeDamage(2);
        }

        if (other.CompareTag("PlayerBulletTag"))
        {
            TakeDamage(100);
            FindObjectOfType<GameScore>().AddScore(100);
            Destroy(other.gameObject);
        }
    }
}

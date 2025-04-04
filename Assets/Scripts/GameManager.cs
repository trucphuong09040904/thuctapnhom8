using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public GameObject gameOver;  // UI Game Over
    public GameObject winnerTextGO; // UI Winner Text
    public Image ImageHP;  // Thanh máu
    public GameObject scoreUITextGO; // UI điểm số

    public RectTransform countdownPanel; // Thanh ngang mở rộng
    public Image countdownImage;
    public Sprite[] countdownSprites; // 3 ảnh đếm ngược

    public GameObject fireworksPrefab; // Thêm biến prefab pháo bông

    private bool isGameOver = false;
    private const int WINNING_SCORE = 10000; // Điểm thắng
    private const string WINNING_SCENE = "PlayScene 7"; // Tên của màn 7

    void Start()
    {
        // Ẩn Winner Text và Game Over lúc đầu
        if (winnerTextGO != null)
        {
            winnerTextGO.SetActive(false);
        }
        else
        {
            Debug.LogError("⚠ Lỗi: winnerTextGO chưa được gán trong GameManager!");
        }

        if (gameOver != null)
        {
            gameOver.SetActive(false);
        }
        else
        {
            Debug.LogError("⚠ Lỗi: gameOver chưa được gán trong GameManager!");
        }

        StartCoroutine(ExpandPanelAndCountdown());
    }

    IEnumerator ExpandPanelAndCountdown()
    {
        Time.timeScale = 0f; // Dừng game khi đếm ngược

        float duration = 0.5f;
        float targetHeight = 300f; // Độ cao của thanh ngang sau khi mở rộng
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float newHeight = Mathf.Lerp(0, targetHeight, elapsedTime / duration);
            countdownPanel.sizeDelta = new Vector2(countdownPanel.sizeDelta.x, newHeight);
            yield return null;
        }

        // Đếm ngược 3, 2, 1
        if (countdownImage != null && countdownSprites.Length > 0)
        {
            for (int i = 0; i < countdownSprites.Length; i++)
            {
                countdownImage.sprite = countdownSprites[i];
                yield return new WaitForSecondsRealtime(1f);
            }
            countdownImage.gameObject.SetActive(false);
            countdownPanel.gameObject.SetActive(false); // Ẩn panel sau khi đếm xong
        }

        Time.timeScale = 1f; // Bắt đầu game sau khi đếm ngược xong
    }

    public void UpdateHP(float currentHP, float maxHP)
    {
        if (ImageHP != null)
        {
            ImageHP.fillAmount = currentHP / maxHP;
        }
        else
        {
            Debug.LogError("⚠ Lỗi: ImageHP chưa được gán trong GameManager!");
        }

        // Kiểm tra nếu máu hết thì dừng game
        if (currentHP <= 0)
        {
            Over();
        }
    }

    // Hàm kiểm tra điểm số và hiển thị Winner nếu đạt 500 điểm ở màn 7
    public void CheckScoreForWin()
    {
        if (isGameOver) return; // Tránh gọi nhiều lần

        string currentScene = SceneManager.GetActiveScene().name;
        int playerScore = GameScore.instance != null ? GameScore.instance.Score : 0;

        if (currentScene == WINNING_SCENE && playerScore >= WINNING_SCORE)
        {
            isGameOver = true;
            Time.timeScale = 0f; // Dừng game
            if (winnerTextGO != null)
            {
                winnerTextGO.SetActive(true); // Hiện "Winner"
            }

            // Hiển thị pháo bông ngay khi "Winner" được hiển thị
            if (fireworksPrefab != null)
            {
                Instantiate(fireworksPrefab, Vector3.zero, Quaternion.identity); // Hiện pháo bông tại vị trí (0, 0, 0)
            }

            StartCoroutine(ShowGameOverAfterDelay(2f)); // Sau 2 giây hiện "Game Over"
        }
    }

    public void Over()
    {
        if (isGameOver) return; // Tránh gọi nhiều lần
        isGameOver = true;

        Time.timeScale = 0f; // Dừng toàn bộ game
        int playerScore = GameScore.instance != null ? GameScore.instance.Score : 0;
        string currentScene = SceneManager.GetActiveScene().name;

        // Nếu ở màn 7 và đạt 500 điểm, hiển thị Winner trước
        if (currentScene == WINNING_SCENE && playerScore >= WINNING_SCORE)
        {
            if (winnerTextGO != null)
            {
                winnerTextGO.SetActive(true); // Hiện "Winner"
            }

            // Hiển thị pháo bông
            if (fireworksPrefab != null)
            {
                Instantiate(fireworksPrefab, Vector3.zero, Quaternion.identity); // Hiện pháo bông tại vị trí (0, 0, 0)
            }

            StartCoroutine(ShowGameOverAfterDelay(2f)); // Sau 2 giây hiện "Game Over"
        }
        else
        {
            ShowGameOver(); // Nếu không đạt 500 điểm hoặc không ở màn 7, hiện "Game Over" ngay
        }
    }

    private IEnumerator ShowGameOverAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Sử dụng thời gian thực
        ShowGameOver();
    }

    void ShowGameOver()
    {
        if (gameOver != null)
        {
            gameOver.SetActive(true);
            Debug.Log("Hiển thị Game Over UI");
        }
        else
        {
            Debug.LogError("⚠ Lỗi: gameOver UI chưa được gán trong GameManager!");
        }
    }

    public void restart()
    {
        Time.timeScale = 1f; // Reset thời gian trước khi restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Kiểm tra nếu scoreUITextGO tồn tại
        if (scoreUITextGO != null)
        {
            scoreUITextGO.GetComponent<GameScore>().Score = 0;
        }
        else
        {
            Debug.LogError("⚠ Lỗi: scoreUITextGO chưa được gán trong GameManager!");
        }
    }

    public void quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Thêm thư viện TMP

public class GameScore : MonoBehaviour
{
    public static GameScore instance;
    public GameObject statsPanel;
    public GameObject VictoryPanel;
    TMP_Text scoreTextUI; 
    int score;

    public int targetScore1 = 1500;
    public int targetScore2 = 2000;
    public int targetScore3 = 3000;
    public int targetScore4 = 4000;
    public int targetScore5 = 5000;
    public int targetScore6 = 6000;
    public int targetScore7 = 7000;
    public int targetScore8 = 100;

    public string scene1 = "PlayScene";
    public string scene2 = "PlayScene 1";
    public string scene3 = "PlayScene 2";
    public string scene4 = "PlayScene 3";
    public string scene5 = "PlayScene 4";
    public string scene6 = "PlayScene 5";
    public string scene7 = "PlayScene 6";
    public string scene8 = "PlayScene 7";

    private GameManager gameManager; // Tham chiếu đến GameManager

    public int Score
    {
        get { return this.score; }
        set
        {
            this.score = value;
            UpdateScoreTextUI();
            CheckAndLoadNextScene();
            // Gọi CheckScoreForWin để kiểm tra điểm số
            if (gameManager != null)
            {
                gameManager.CheckScoreForWin();
            }
        }
    }

    void Awake()
    {
        // Đảm bảo chỉ có 1 instance của GameScore
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        scoreTextUI = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        gameManager = FindObjectOfType<GameManager>(); // Tìm GameManager

        if (scoreTextUI == null)
        {
            Debug.LogError("GameScore: Không tìm thấy TMP Text UI! Hãy đảm bảo có TextMeshPro trong Canvas.");
        }
        else
        {
            UpdateScoreTextUI();
        }

        if (gameManager == null)
        {
            Debug.LogError("GameScore: Không tìm thấy GameManager trong scene!");
        }
    }

    void UpdateScoreTextUI()
    {
        if (scoreTextUI != null)
        {
            scoreTextUI.text = string.Format("{0:000000}", score);
        }
    }

    void CheckAndLoadNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == scene1 && score >= targetScore1)
        {
            LoadNextScene(scene2);
        }
        else if (currentScene == scene2 && score >= targetScore2)
        {
            LoadNextScene(scene3);
        }
        else if (currentScene == scene3 && score >= targetScore3)
        {
            LoadNextScene(scene4);
        }
        else if (currentScene == scene4 && score >= targetScore4)
        {
            LoadNextScene(scene5);
        }
        else if (currentScene == scene5 && score >= targetScore5)
        {
            LoadNextScene(scene6);
        }
        else if (currentScene == scene6 && score >= targetScore6)
        {
            LoadNextScene(scene7);
        }
        else if (currentScene == scene7 && score >= targetScore7)
        {
            LoadNextScene(scene8);
        }
        else if (currentScene == scene8 && score >= targetScore8)
        {
            LevelTimer timer = FindObjectOfType<LevelTimer>();
            if (timer != null)
            {
                timer.SaveTime();
                float completedTime = Time.time - timer.startTime; // Tính thời gian đã hoàn thành
                Debug.Log($"✅ Thời gian đã được lưu cho {timer.levelKey}: {completedTime:F2} giây");
            }
            else
            {
                Debug.LogWarning("⚠️ Không tìm thấy LevelTimer trong scene hiện tại!");
            }

            ShowStatsPanel();
        }
    }

    void LoadNextScene(string sceneName)
    {
        LevelTimer timer = FindObjectOfType<LevelTimer>();
        if (timer != null)
        {
            timer.SaveTime();
            float completedTime = Time.time - timer.startTime; // Tính thời gian đã hoàn thành
            Debug.Log($"✅ Thời gian đã được lưu cho {timer.levelKey}: {completedTime:F2} giây");
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy LevelTimer trong scene hiện tại!");
        }

        Debug.Log("🔁 Đang chuyển đến scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // Thêm hàm này để cập nhật điểm số khi Boss bị bắn
    public void AddScore(int amount)
    {
        Score += amount;
    }
    
    void ShowStatsPanel()
    {
        FindObjectOfType<LevelTimer>()?.SaveTime();

        if (statsPanel != null)
        {
            statsPanel.SetActive(true);
            Animator animator = statsPanel.GetComponent<Animator>();
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PanelSlideUp"))
                    {
                        animator.Play("PanelSlideUp");
                    }
            Debug.Log("📊 Hiển thị bảng thống kê thời gian hoàn thành!");
        }
        else
        {
            Debug.LogWarning("❌ Không gán StatsPanel trong Inspector!");
        }

        if (VictoryPanel != null)
        {
            VictoryPanel.SetActive(true);
            Animator animator = VictoryPanel.GetComponent<Animator>();
            if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("PanelSlideInFromLeft"))
            {
                animator.Play("PanelSlideInFromLeft");
            }

            Debug.Log("➡️ Hiển thị sideStatsPanel với animation trượt từ trái sang phải!");
        }
        else
        {
            Debug.LogWarning("❌ sideStatsPanel chưa được gán trong Inspector!");
        }

    }


}
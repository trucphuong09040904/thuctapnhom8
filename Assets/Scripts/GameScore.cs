using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Thêm thư viện TMP

public class GameScore : MonoBehaviour
{
    public static GameScore instance; // 🔹 Biến instance để gọi từ bất kỳ đâu

    TMP_Text scoreTextUI; // Sử dụng TMP_Text thay vì Text
    int score;
    float startTime; // Lưu thời gian bắt đầu màn chơi

    public int targetScore1 = 1000;
    public int targetScore2 = 2000;
    public int targetScore3 = 3000;
    public int targetScore4 = 4000;
    public int targetScore5 = 5000;
    public int targetScore6 = 6000;
    public int targetScore7 = 7000;
    public int targetScoreFinal = 50000; // Mốc điểm chiến thắng màn 8

    public string scene1 = "PlayScene";
    public string scene2 = "PlayScene 1";
    public string scene3 = "PlayScene 2";
    public string scene4 = "PlayScene 3";
    public string scene5 = "PlayScene 4";
    public string scene6 = "PlayScene 5";
    public string scene7 = "PlayScene 6";
    public string finalScene = "PlayScene 7"; // Màn cuối cùng

    public int Score
    {
        get { return this.score; }
        set
        {
            this.score = value;
            UpdateScoreTextUI();
            CheckAndLoadNextScene();
        }
    }

    void Awake()
    {
        // 🔹 Đảm bảo chỉ có 1 instance của GameScore
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

        if (scoreTextUI == null)
        {
            Debug.LogError("GameScore: Không tìm thấy TMP Text UI! Hãy đảm bảo có TextMeshPro trong Canvas.");
        }
        else
        {
            UpdateScoreTextUI();
        }

        startTime = Time.time; // Lưu thời gian bắt đầu màn chơi
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
        else if (currentScene == finalScene && score >= targetScoreFinal)
        {
            HandleFinalWin(); // Xử lý khi người chơi đạt 50,000 điểm
        }
    }

    void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    void HandleFinalWin()
    {
        float completionTime = Time.time - startTime; // Tính thời gian hoàn thành màn chơi
        ScoreManager.SaveScore(completionTime); // Lưu thời gian hoàn thành vào bảng thành tích

        Debug.Log("🎉 Bạn đã chiến thắng! Thành tích của bạn đã được lưu.");
        SceneManager.LoadScene("LeaderboardScene"); // Chuyển đến bảng thành tích
    }
}

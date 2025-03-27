using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    public TMP_Text leaderboardText; // Kéo thả TextMeshPro UI vào đây trong Unity

    void Start()
    {
        List<float> scores = ScoreManager.LoadScores();
        string displayText = "🏆 Bảng Xếp Hạng 🏆\n";

        if (scores.Count == 0)
        {
            displayText += "Chưa có thành tích nào!";
        }
        else
        {
            for (int i = 0; i < scores.Count; i++)
            {
                displayText += $"{i + 1}. {scores[i]:0.00} giây\n";
            }
        }

        leaderboardText.text = displayText;
    }
}

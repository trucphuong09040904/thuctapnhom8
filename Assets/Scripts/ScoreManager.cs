using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    private const string ScoreKey = "TopScores";

    public static void SaveScore(float completionTime)
    {
        List<float> topScores = LoadScores();

        topScores.Add(completionTime);
        topScores.Sort(); // Sắp xếp theo thời gian từ nhỏ đến lớn

        if (topScores.Count > 5)
        {
            topScores.RemoveAt(5); // Chỉ giữ 5 thành tích nhanh nhất
        }

        for (int i = 0; i < topScores.Count; i++)
        {
            PlayerPrefs.SetFloat(ScoreKey + i, topScores[i]);
        }

        PlayerPrefs.Save();
    }

    public static List<float> LoadScores()
    {
        List<float> scores = new List<float>();

        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey(ScoreKey + i))
            {
                scores.Add(PlayerPrefs.GetFloat(ScoreKey + i));
            }
        }

        return scores;
    }
}

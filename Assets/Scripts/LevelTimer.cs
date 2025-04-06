using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float countdown = 3f;
    public float startTime;
    private bool levelStarted = false;
    public string levelKey;

    void Start()
    {
        startTime = Time.time;
        Invoke(nameof(StartLevel), countdown); // Chờ 3s rồi bắt đầu tính
    }

    void StartLevel()
    {
        levelStarted = true;
        startTime = Time.time;
    }

    public float GetElapsedTime()
    {
        if (!levelStarted) return 0f;
        return Time.time - startTime;
    }

    public void SaveTime()
    {
        float time = GetElapsedTime();
        PlayerPrefs.SetFloat(levelKey, time);
        PlayerPrefs.Save();
    }
}

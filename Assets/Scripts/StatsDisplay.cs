using System.Collections;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    public TMP_Text[] levelTexts; // Gán 8 text (Màn 1 đến 8)
    public TMP_Text totalTimeText;

    public float showDelay = 0.5f;
    public float moveDuration = 0.4f;
    public Vector3 moveOffset = new Vector3(0f, -50f, 0f); // Dịch từ dưới lên

    private int numberOfLevels = 8;
    private float totalTime = 0f;

    void Start()
    {
        StartCoroutine(ShowStatsOneByOne());
    }

    IEnumerator ShowStatsOneByOne()
    {
        totalTime = 0f;

        for (int i = 0; i < numberOfLevels; i++)
        {
            string key = "Màn " + (i + 1);

            if (PlayerPrefs.HasKey(key))
            {
                float time = PlayerPrefs.GetFloat(key);
                totalTime += time;
                levelTexts[i].text = $"Màn {i + 1}: {FormatTime(time)}";
            }
            else
            {
                levelTexts[i].text = $"Màn {i + 1}: Chưa có dữ liệu";
            }

            yield return StartCoroutine(AnimateText(levelTexts[i]));
            yield return new WaitForSeconds(showDelay);
        }

        totalTimeText.text = $"Total time: {FormatTime(totalTime)}";
        yield return StartCoroutine(AnimateText(totalTimeText));
    }

    IEnumerator AnimateText(TMP_Text textObject)
    {
        RectTransform rect = textObject.GetComponent<RectTransform>();
        Vector3 originalPos = rect.anchoredPosition;
        Vector3 startPos = originalPos + moveOffset;

        textObject.gameObject.SetActive(true);
        rect.anchoredPosition = startPos;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            rect.anchoredPosition = Vector3.Lerp(startPos, originalPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time - Mathf.Floor(time)) * 100);
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}

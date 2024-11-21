using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Challenger : MonoBehaviour
{
    public TextMeshProUGUI month;
    public TextMeshProUGUI progressText;
    public Image progress;
    public Image cup;

    public void LoadData(DateTime date, DateTime releaseDate, Sprite cup, DataManager dataManager)
    {
        TimeSpan timeSpan = date - releaseDate;
        int startIndex = timeSpan.Days + 1000 + 1;

        int dayInMonth = DateTime.DaysInMonth(date.Year, date.Month);

        int totalCompleted = 0;
        for (int i = 0; i < dayInMonth; i++)
        {
            LevelDataStorage levelDataStorage = dataManager.GetLevelStorage(startIndex);
            if (levelDataStorage.isCompleted) totalCompleted++;
            startIndex++;
        }

        progress.fillAmount = (float)totalCompleted / dayInMonth;
        string monthInEnglish = date.ToString("MMMM", new System.Globalization.CultureInfo("en-US"));
        month.text = monthInEnglish;
        progressText.text = totalCompleted + "/" + dayInMonth;
        this.cup.sprite = cup;
        gameObject.SetActive(true);
    }
}

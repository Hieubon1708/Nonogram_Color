using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyDay : MonoBehaviour
{
    public Image bg;
    public Image bgCircle;
    public Image progress;
    public Image image;
    public TextMeshProUGUI num;
    public int index;
    public int level;
    public DateTime date;

    public void CanSelcect()
    {
        Color color = num.color;
        color.a = 1f;
        num.color = color;
    }

    public void SetDefaultField()
    {
        BgHide();
        if (image.gameObject.activeSelf)
        {
            image.gameObject.SetActive(false);
            BgHide();
        }
    }

    public void LoadData(Sprite sprite, int level, DateTime date, ref int totalCompleted)
    {
        this.level = level;
        this.date = date;
        LevelDataStorage levelDataStorage = GameController.instance.dataManager.GetLevelStorage(level);
        LevelConfig levelConfig = GameController.instance.dataManager.GetLevel(level);
        if (levelDataStorage.isCompleted)
        {
            image.sprite = sprite;
            image.gameObject.SetActive(true);
            totalCompleted++;
        }
        if (levelDataStorage.totalSelect > 0)
        {
            Color color = progress.color;
            color.a = 1f;
            progress.color = color;
            progress.fillAmount = 1 - (float)levelDataStorage.totalSelect / levelConfig.totalToWin;
        }
    }

    public void ResetProgress()
    {
        Color color = progress.color;
        color.a = 0f;
        progress.color = color;
        progress.fillAmount = 1;
    }

    public void CheckProgress()
    {
        if (GameController.instance.levelDataStorage.totalSelect > 0)
        {
            Color color = progress.color;
            color.a = 1f;
            progress.color = color;
            progress.fillAmount = 1 - (float)GameController.instance.levelDataStorage.totalSelect / GameController.instance.levelConfig.totalToWin;
        }
    }

    public void CanNotSelcect()
    {
        Color color = num.color;
        color.a = 0.5f;
        num.color = color;
    }

    public void OnClick(float time)
    {
        if (num.text == "" || num.color.a == 0.5f) return;
        UIController.instance.daily.HideBgAll(time);
        UIController.instance.daily.SelectDay(this, level);
        BgShow(time);
    }

    public void BgShow(float time)
    {
        DoKill();
        if (!image.gameObject.activeSelf) bgCircle.DOFade(1f, time);
        else bg.DOFade(1f, time);
    }

    public void BgHide(float time)
    {
        DoKill();
        if (!image.gameObject.activeSelf) bgCircle.DOFade(0f, time);
        else bg.DOFade(0f, time);
    }

    public void BgHide()
    {
        if (!image.gameObject.activeSelf)
        {
            Color c2 = bgCircle.color;
            c2.a = 0;
            bgCircle.color = c2;
        }
        else
        {
            Color c1 = bg.color;
            c1.a = 0;
            bg.color = c1;
        }
    }

    public void BgShow(bool isMainPage)
    {
        if (!image.gameObject.activeSelf)
        {
            Color c2 = bgCircle.color;
            c2.a = 1;
            bgCircle.color = c2;
        }
        else
        {
            Color c1 = bg.color;
            c1.a = 1;
            bg.color = c1;
        }
        if (isMainPage) UIController.instance.daily.SelectDay(this, level);
    }

    void DoKill()
    {
        bg.DOKill();
        bgCircle.DOKill();
    }

    public void OnDestroy()
    {
        DoKill();
    }
}

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyDay : MonoBehaviour
{
    public Image bg;
    public Image bgCircle;
    public Image progress;
    public TextMeshProUGUI num;
    public int index;
    public int level;

    public void CanSelcect()
    {
        Color color = num.color;
        color.a = 1f;
        num.color = color;
    }

    public void CanNotSelcect()
    {
        Color color = num.color;
        color.a = 0.5f;
        num.color = color;
    }

    public void OnClick(float time)
    {
        if(num.text == "" || num.color.a == 0.5f) return;
        UIController.instance.daily.HideBgAll(UIController.instance.daily.day[1]);
        BgShow(time);
    }

    public void BgShow(float time)
    {
        DoKill();
        bgCircle.DOFade(1f, time);
    }

    public void BgHide(float time)
    {
        DoKill();
        bgCircle.DOFade(0f, time);
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

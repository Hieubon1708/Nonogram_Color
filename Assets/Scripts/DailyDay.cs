using TMPro;
using UnityEngine;

public class DailyDay : MonoBehaviour
{
    public Animation bgAni;
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

    public void OnClick()
    {
        if(num.text == "" || num.color == Color.gray) return;
        UIController.instance.daily.SelectDay(index, level);
        BgShowAni();
    }

    public void BgShowAni()
    {
        bgAni.Play("DailyBgShowAni");
    }

    public void BgHideAni()
    {
        bgAni.Play("DailyBgHideAni");
    }
}

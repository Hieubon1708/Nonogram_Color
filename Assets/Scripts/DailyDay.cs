using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyDay : MonoBehaviour
{
    public Animation ani;
    public Image progress;
    public Image image;
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
        BgShow();
    }

    public void BgShow()
    {
        /*if (isCompleted)
        {
            ani.Play("DailyDayShowBgCompleted");
        }
        else
        {
            ani.Play("DailyDayShowBg");
        }*/
    }

    public void BgHide()
    {
       /* if (isCompleted)
        {
            ani.Play("DailyDayHideBgCompleted");
        }
        else
        {
            ani.Play("DailyDayHide.Bg");
        }*/
    }
}

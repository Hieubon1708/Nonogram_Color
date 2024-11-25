using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Daily : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject dayPre;
    public int level;
    public RectTransform[] container;
    public DailyDay[][] day = new DailyDay[3][];
    public RectTransform[] pages;
    public RectTransform pageParent;
    public RectTransform canvas;
    public DateTime dateSelect;
    public DateTime releaseDate = new DateTime(2024, 10, 1);
    public GameObject intro;
    public bool isCanBack;
    public bool isCanNext;
    public TextMeshProUGUI[] date;
    public TextMeshProUGUI[] totalCompleted;
    public DailyDay dailyDay;
    public GameObject gamePlay;
    public GameObject daily;
    public GameObject labelDaily;
    public GameObject back;
    public GameObject backOrigin;
    public GameObject home;
    public GameObject panelQuestion;
    public CanvasGroup fontWin;
    public CanvasGroup tempFontWin;
    public TextMeshProUGUI label;
    public Image arrowLeft;
    public Image arrowRight;
    public GameObject continueButton;
    public DailyControlView dailyControlView;

    public void Generate()
    {

        for (int i = 0; i < container.Length; i++)
        {
            DailyDay[] dailyDay = new DailyDay[40];
            for (int j = 0; j < dailyDay.Length; j++)
            {
                dailyDay[j] = Instantiate(dayPre, container[i]).GetComponent<DailyDay>();
                dailyDay[j].name = i + " " + j;
            }
            day[i] = dailyDay;
        }

        //LoadPage(DateTime.Now);
    }

    public void LoadData()
    {
        LoadPage(DateTime.Now);
        arrowLeft.color = Vector4.one;
        arrowRight.color = new Vector4(arrowRight.color.r, arrowRight.color.b, arrowRight.color.g, 0);
        dailyControlView.LoadData();
    }

    int GetIndex(DateTime currentTime)
    {
        TimeSpan timeSpan = currentTime - releaseDate;
        return timeSpan.Days;
    }

    public bool IsCanBack()
    {
        DateTime date = dateSelect;
        bool result = date.Month == releaseDate.Month && date.Year == releaseDate.Year;
        if (!result)
        {
            if (arrowLeft.color.a != 0) arrowLeft.DOFade(0f, 0.25f);
        }
        else
        {
            if (arrowLeft.color.a != 1) arrowLeft.DOFade(1f, 0.25f);
        }
        return result;
    }

    public bool IsCanNext()
    {
        DateTime date = dateSelect;
        bool result = date.Month == DateTime.Now.Month && date.Year == DateTime.Now.Year;
        if (!result)
        {
            if (arrowRight.color.a != 0) arrowRight.DOFade(0f, 0.25f);
        }
        else
        {
            if (arrowRight.color.a != 1) arrowRight.DOFade(1f, 0.25f);
        }
        return result;
    }

    public void LoadPage(DateTime currentViewPage)
    {
        int indexStart = -1;
        for (int i = 0; i < day.Length; i++)
        {
            int totalCompleted = 0;
            if (i == 0) currentViewPage = currentViewPage.AddMonths(-1);
            else currentViewPage = currentViewPage.AddMonths(1);
            DateTime firstDayOfMonth = new DateTime(currentViewPage.Year, currentViewPage.Month, 1);
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
            if (indexStart == -1)
            {
                DateTime date = firstDayOfMonth;
                if (date.Date < releaseDate) date = releaseDate;
                indexStart = GetIndex(date);
            }

            if (i == 1) dateSelect = currentViewPage;
            //Debug.LogWarning(currentViewPage);

            string monthInEnglish = currentViewPage.ToString("MMMM", new System.Globalization.CultureInfo("en-US"));
            date[i].text = monthInEnglish + " " + currentViewPage.Year;

            int startDayOfWeek = GetDayOfWeek((int)dayOfWeek);
            int d = 1;
            int dayInMonth = DateTime.DaysInMonth(currentViewPage.Year, currentViewPage.Month);
            // Debug.LogWarning("month " + currentViewPage.Month + " have " + dayInMonth);

            for (int j = 0; j < day[i].Length; j++)
            {
                day[i][j].ResetDay();
                if (j >= startDayOfWeek && d <= dayInMonth)
                {
                    DateTime date = new DateTime(currentViewPage.Year, currentViewPage.Month, d);
                    if (date.Date > DateTime.Now.Date)
                    {
                        day[i][j].CanNotSelcect();
                    }
                    else
                    {
                        day[i][j].CanSelcect();
                        if (date.Date >= releaseDate)
                        {
                            day[i][j].LoadData(sprites[indexStart], indexStart + 1 + 1000, date.Date, ref totalCompleted);
                            indexStart++;
                        }
                        if (d == dayInMonth || date.Date == DateTime.Now.Date)
                        {
                            //Debug.Log("month " + currentViewPage.Month + " day  " + d + " j " + j);
                            int index = j;
                            bool isHave = false;
                            while (index >= startDayOfWeek)
                            {
                                if (!day[i][index].image.gameObject.activeSelf)
                                {
                                    isHave = true;
                                    a[i] = day[i][index].gameObject;
                                    day[i][index].BgShow(i == 1);
                                    break;
                                }
                                index--;
                            }
                            if (!isHave)
                            {
                                day[i][startDayOfWeek].BgShow(i == 1);
                            }
                        }
                    }
                    day[i][j].num.text = d.ToString();
                    d++;

                }
                else
                {
                    day[i][j].num.text = "";
                }
            }
            this.totalCompleted[i].text = totalCompleted + "/" + dayInMonth;
        }
    }
    public GameObject[] a = new GameObject[10];

    int GetDayOfWeek(int dayOfWeek)
    {
        if (dayOfWeek == 0) return 6;
        else return dayOfWeek - 1;
    }

    public void HideBgAll(float time)
    {
        for (int i = 0; i < day[1].Length; i++)
        {
            day[1][i].BgHide(time);
        }
    }

    public void ShowQuestion()
    {
        GameController.instance.levelDataStorage = GameController.instance.dataManager.GetLevelStorage(level);
        if (dailyDay.image.gameObject.activeSelf)
        {
            continueButton.SetActive(false);
            panelQuestion.SetActive(true);
            return;
        }
        else continueButton.SetActive(true);
        if (!GameController.instance.levelDataStorage.isClicked)
        {
            Play();
        }
        else
        {

            panelQuestion.SetActive(true);
        }
    }

    public void Resum()
    {
        panelQuestion.SetActive(false);
        Play();
    }

    public void Restart()
    {
        GameController.instance.GetLevel(level, level);
        GameController.instance.SaveLevel();
        dailyDay.ResetProgress();
        panelQuestion.SetActive(false);
        Play();
    }

    public void QuestionCancel()
    {
        panelQuestion.SetActive(false);
    }

    public void Play()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            string monthInEnglish = dailyDay.date.ToString("MMMM", new System.Globalization.CultureInfo("en-US"));
            label.text = monthInEnglish + " " + dailyDay.date.Day;
            daily.SetActive(false);
            gamePlay.SetActive(true);
            labelDaily.SetActive(true);
            back.SetActive(true);
            backOrigin.SetActive(false);
            panelQuestion.SetActive(false);
            GameController.instance.isLoadData = true;

            GameController.instance.level = level;
            GameController.instance.GetLevel(level, level);
            GameController.instance.LoadLevel();
            UIController.instance.gamePlay.SwitchFontWin(fontWin, out tempFontWin);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, delegate
            {
                GameController.instance.isLoadData = false;
            });
        });
    }

    public void BackDaily()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            gamePlay.SetActive(false);
            daily.SetActive(true);
            back.SetActive(false);
            backOrigin.SetActive(true);
            labelDaily.SetActive(false);
            LoadPage(dailyDay.date);
            ResetWin();
            UIController.instance.StopFxWin();
            UIController.instance.gamePlay.SwitchFontWin(tempFontWin, out fontWin);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void BackDailyButton()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            gamePlay.SetActive(false);
            daily.SetActive(true);
            back.SetActive(false);
            labelDaily.SetActive(false);
            dailyDay.CheckProgress();

            UIController.instance.gamePlay.SwitchFontWin(tempFontWin, out fontWin);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void BackHome()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            daily.SetActive(false);
            home.SetActive(true);

            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void SelectDay(DailyDay dailyDay, int level)
    {
        this.level = level;
        this.dailyDay = dailyDay;
    }

    void ResetWin()
    {
        fontWin.alpha = 0;
        tempFontWin.alpha = 0;
        UIController.instance.gamePlay.ResetWin();
    }
}

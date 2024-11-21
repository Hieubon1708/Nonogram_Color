using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    bool isDrag;
    float distancePage;
    float offsetX;
    float startX;
    float startXScreen;
    bool isMovingPage;
    int indexPage;
    DateTime dateSelect;
    bool isLessThan0;
    public DateTime releaseDate = new DateTime(2024, 10, 1);
    public GameObject intro;
    public bool isMouseDown;
    public bool isCanBack;
    public bool isCanNext;
    public TextMeshProUGUI[] date;
    public TextMeshProUGUI[] totalCompleted;
    public DailyDay dailyDay;
    public GameObject gamePlay;
    public GameObject daily;
    public GameObject labelDaily;
    public GameObject back;
    public GameObject home;
    public GameObject panelQuestion;
    public CanvasGroup fontWin;
    public CanvasGroup tempFontWin;
    public TextMeshProUGUI label;
    public Image arrowLeft;
    public Image arrowRight;
    public GameObject continueButton;

    public void ShowIntro()
    {
        intro.SetActive(true);
    }

    public void HideIntro()
    {
        intro.SetActive(false);
    }

    DailyDay GetDailyDay(GameObject d)
    {
        for (int i = 0; i < day[1].Length; i++)
        {
            if (day[1][i].gameObject == d) return day[1][i];
        }
        return null;
    }

    public void Generate()
    {
        DOVirtual.DelayedCall(0.02f, delegate
        {
            float width = canvas.sizeDelta.x;
            float startSize = -width;
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].position = UIController.instance.ScreenToWorldPoint(new Vector3(-Screen.width / 2 + (i * Screen.width), Screen.height / 2, 100));

                pages[i].offsetMin = new Vector2(-canvas.sizeDelta.x + (i * canvas.sizeDelta.x), pages[i].offsetMin.y);
                pages[i].offsetMax = new Vector2(-canvas.sizeDelta.x + (i * canvas.sizeDelta.x), pages[i].offsetMax.y);
            }
            distancePage = -pages[0].position.x;

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
        });
    }

    public void LoadData()
    {
        LoadPage(DateTime.Now);
        Color cor1 = arrowRight.color;
        cor1.a = 0f;
        arrowRight.color = cor1;
        arrowRight.raycastTarget = false;
        Color cor2 = arrowLeft.color;
        cor2.a = 1f;
        arrowLeft.color = cor2;
        arrowLeft.raycastTarget = true;
    }

    int GetIndex(DateTime currentTime)
    {
        TimeSpan timeSpan = currentTime - releaseDate;
        return timeSpan.Days;
    }

    void CheckArrowLeft(DateTime currentViewPage)
    {
        arrowLeft.DOKill();
        float alpha = 1;
        DateTime date = currentViewPage.AddMonths(-1);
        if (date.Month == releaseDate.Month && date.Year == releaseDate.Year)
        {
            alpha = 0;
            arrowLeft.raycastTarget = false;
        }
        else
        {
            arrowLeft.raycastTarget = true;
        }
        arrowLeft.DOFade(alpha, 0.15f).SetEase(Ease.Linear);
    }

    void CheckArrowRight(DateTime currentViewPage)
    {
        arrowRight.DOKill();
        float alpha = 1;
        DateTime date = currentViewPage.AddMonths(1);
        if (date.Month == DateTime.Now.Month && date.Year == DateTime.Now.Year)
        {
            alpha = 0;
            arrowRight.raycastTarget = false;
        }
        else
        {
            arrowRight.raycastTarget = true;
        }
        arrowRight.DOFade(alpha, 0.15f).SetEase(Ease.Linear);
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
            //Debug.LogWarning(currentViewPage);

            if (i == 1)
            {
                dateSelect = currentViewPage;
                if (dateSelect.Month == DateTime.Now.Month && dateSelect.Year == DateTime.Now.Year) isCanNext = false;
                else isCanNext = true;
                if (dateSelect.Month == releaseDate.Month && dateSelect.Year == releaseDate.Year) isCanBack = false;
                else isCanBack = true;
            }
            else
            {
                //HideBgAll(day[i]);
            }
            string monthInEnglish = currentViewPage.ToString("MMMM", new System.Globalization.CultureInfo("en-US"));
            date[i].text = monthInEnglish + " " + currentViewPage.Year;

            int startDayOfWeek = GetDayOfWeek((int)dayOfWeek);
            int d = 1;
            int dayInMonth = DateTime.DaysInMonth(currentViewPage.Year, currentViewPage.Month);
            // Debug.LogWarning("month " + currentViewPage.Month + " have " + dayInMonth);

            for (int j = 0; j < day[i].Length; j++)
            {
                day[i][j].SetDefaultField();
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
                            //if (i != 1) HideBgAll(day[i]);
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

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (panelQuestion.activeSelf || UIController.instance.uICommon.layerCover.gameObject.activeSelf) return;
            if (isMovingPage || EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.CompareTag("Arrow")) return;
            Vector2 mousePos = UIController.instance.ScreenToWorldPoint(Input.mousePosition);
            offsetX = pageParent.position.x - mousePos.x;
            startXScreen = Input.mousePosition.x;
            startX = mousePos.x;
            isDrag = true;
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            if (panelQuestion.activeSelf || !isMouseDown || UIController.instance.uICommon.layerCover.gameObject.activeSelf) return;
            isMouseDown = false;
            if (isMovingPage || EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.CompareTag("Arrow")) return;
            if (Mathf.Abs(Input.mousePosition.x - startXScreen) <= 2.5f)
            {
                pageParent.DOMoveX(distancePage * indexPage, 0.15f).SetEase(Ease.Linear);
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                eventDataCurrentPosition.position = Input.mousePosition;
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                for (int i = 0; i < results.Count; i++)
                {
                    GameObject d = results[i].gameObject;
                    if (d.CompareTag("Day"))
                    {
                        DailyDay dailyDay = GetDailyDay(d);
                        dailyDay.OnClick(0.15f);
                    }
                }
            }
            else
            {
                isLessThan0 = startX - UIController.instance.ScreenToWorldPoint(Input.mousePosition).x < 0;
                if (!isLessThan0)
                {
                    Next();
                }
                else
                {
                    Back();
                }
            }
        }

        if (isDrag)
        {
            Vector3 pos = UIController.instance.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Screen.height / 2, 100));
            float x = pos.x + offsetX;
            if (!isCanNext) x = Mathf.Clamp(x, indexPage * distancePage, float.MaxValue);
            if (!isCanBack) x = Mathf.Clamp(x, float.MinValue, indexPage * distancePage);
            pageParent.position = new Vector3(x, pos.y, pos.z);
        }
        else
        {
            /*if (isMovingPage)
            {
                pageParent.position = Vector3.MoveTowards(pageParent.position, new Vector3(distancePage * indexPage, pageParent.position.y, pageParent.position.z), 0.25f);
                if (pageParent.position.x == indexPage * distancePage)
                {
                    isMovingPage = false;
                    if (isLessThan0) LoadPage(dateSelect.AddMonths(-1));
                    else LoadPage(dateSelect.AddMonths(1));

                }
            }*/
        }
    }

    void MovePage()
    {
        pageParent.DOMoveX(distancePage * indexPage, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
        {
            if (isLessThan0) LoadPage(dateSelect.AddMonths(-1));
            else LoadPage(dateSelect.AddMonths(1));
            isMovingPage = false;
        });
    }


    public void Next()
    {
        if (isMovingPage || intro.activeSelf || !isCanNext) return;
        isMovingPage = true;
        isLessThan0 = false;
        pages[0].position = new Vector3(pages[pages.Length - 1].position.x + distancePage, pages[pages.Length - 1].position.y, pages[pages.Length - 1].position.z);
        RectTransform rect = pages[0];
        DailyDay[] d = day[0];
        TextMeshProUGUI t = date[0];
        TextMeshProUGUI tt = totalCompleted[0];
        for (int i = 0; i < pages.Length - 1; i++)
        {
            pages[i] = pages[i + 1];
            day[i] = day[i + 1];
            date[i] = date[i + 1];
            totalCompleted[i] = totalCompleted[i + 1];
        }
        pages[pages.Length - 1] = rect;
        day[day.Length - 1] = d;
        date[date.Length - 1] = t;
        totalCompleted[totalCompleted.Length - 1] = tt;
        indexPage--;
        CheckArrowLeft(dateSelect);
        CheckArrowRight(dateSelect);
        MovePage();
    }

    public void Back()
    {
        if (isMovingPage || intro.activeSelf || !isCanBack) return;
        isMovingPage = true;
        isLessThan0 = true;
        pages[pages.Length - 1].position = new Vector3(pages[0].position.x - distancePage, pages[0].position.y, pages[0].position.z);
        RectTransform rect = pages[pages.Length - 1];
        DailyDay[] d = day[day.Length - 1];
        TextMeshProUGUI t = date[date.Length - 1];
        TextMeshProUGUI tt = totalCompleted[totalCompleted.Length - 1];
        for (int i = pages.Length - 1; i > 0; i--)
        {
            pages[i] = pages[i - 1];
            day[i] = day[i - 1];
            date[i] = date[i - 1];
            totalCompleted[i] = totalCompleted[i - 1];
        }
        pages[0] = rect;
        day[0] = d;
        date[0] = t;
        totalCompleted[0] = tt;
        indexPage++;
        CheckArrowLeft(dateSelect);
        CheckArrowRight(dateSelect);
        MovePage();
    }

    public void HideBgAll(DailyDay[] dailyDays)
    {
        for (int i = 0; i < dailyDays.Length; i++)
        {
            dailyDays[i].BgHide();
        }
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
        isMouseDown = false;
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
            isMouseDown = false;
            labelDaily.SetActive(true);
            back.SetActive(true);
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

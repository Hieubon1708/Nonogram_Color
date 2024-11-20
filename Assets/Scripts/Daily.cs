using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
    DateTime releaseDate = new DateTime(2024, 10, 1);
    public GameObject intro;
    public bool isCanBack;
    public bool isCanNext;
    public TextMeshProUGUI[] date;

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

    public void Awake()
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

            LoadPage(DateTime.Now);
        });
    }

    public void LoadPage(DateTime currentViewPage)
    {
        for (int i = 0; i < day.Length; i++)
        {
            bool isShowBgLastDay = false;

            if (i == 0) currentViewPage = currentViewPage.AddMonths(-1);
            else currentViewPage = currentViewPage.AddMonths(1);
            DateTime firstDayOfMonth = new DateTime(currentViewPage.Year, currentViewPage.Month, 1);
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
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
            Debug.LogWarning("month " + currentViewPage.Month + " have " + dayInMonth);

            for (int j = 0; j < day[i].Length; j++)
            {
                if (j >= startDayOfWeek && d <= dayInMonth)
                {
                    if (new DateTime(currentViewPage.Year, currentViewPage.Month, d).Date > DateTime.Now.Date)
                    {
                        day[i][j].CanNotSelcect();
                    }
                    else
                    {
                        if(d <= dayInMonth)
                        {
                            Debug.Log("month " + currentViewPage.Month + " day  " + d);
                        }
                        day[i][j].CanSelcect();
                    }
                    day[i][j].num.text = d.ToString();
                    d++;

                }
                else
                {
                    day[i][j].num.text = "";
                }
            }
        }
    }
    public GameObject a;

    int GetDayOfWeek(int dayOfWeek)
    {
        if (dayOfWeek == 0) return 6;
        else return dayOfWeek - 1;
    }

    public void Update()
    {
        if (intro.activeSelf) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (isMovingPage || EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.CompareTag("Arrow")) return;
            Vector2 mousePos = UIController.instance.ScreenToWorldPoint(Input.mousePosition);
            offsetX = pageParent.position.x - mousePos.x;
            startXScreen = Input.mousePosition.x;
            startX = mousePos.x;
            isDrag = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
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
        for (int i = 0; i < pages.Length - 1; i++)
        {
            pages[i] = pages[i + 1];
            day[i] = day[i + 1];
            date[i] = date[i + 1];
        }
        pages[pages.Length - 1] = rect;
        day[day.Length - 1] = d;
        date[date.Length - 1] = t;
        indexPage--;
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
        for (int i = pages.Length - 1; i > 0; i--)
        {
            pages[i] = pages[i - 1];
            day[i] = day[i - 1];
            date[i] = date[i - 1];
        }
        pages[0] = rect;
        day[0] = d;
        date[0] = t;
        indexPage++;
        MovePage();
    }

    public void HideBgAll(DailyDay[] dailyDays)
    {
        for (int i = 0; i < dailyDays.Length; i++)
        {
            dailyDays[i].BgHide(0);
        }
    }

    public void SelectDay(DailyDay dailyDay, int level)
    {
        this.level = level;
    }
}

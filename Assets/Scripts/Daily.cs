using DG.Tweening;
using System;
using TMPro;
using Unity.Mathematics;
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
    bool isMovingPage;
    int indexPage;
    DateTime dateSelect;
    bool isLessThan0;
    DateTime releaseDate = new DateTime(2024, 10, 1);

    public TextMeshProUGUI[] date;

    public void LoadDaily()
    {

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
            if (i == 0) currentViewPage = currentViewPage.AddMonths(-1);
            else currentViewPage = currentViewPage.AddMonths(1);
            DateTime firstDayOfMonth = new DateTime(currentViewPage.Year, currentViewPage.Month, 1);
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
            //Debug.LogWarning(currentViewPage);

            if (i == 1) dateSelect = currentViewPage;
            string monthInEnglish = currentViewPage.ToString("MMMM", new System.Globalization.CultureInfo("en-US"));
            date[i].text = monthInEnglish + " " + currentViewPage.Year;

            int startDayOfWeek = GetDayOfWeek((int)dayOfWeek);
            int d = 1;

            for (int j = 0; j < day[i].Length; j++)
            {
                if (j >= startDayOfWeek && d <= DateTime.DaysInMonth(currentViewPage.Year, currentViewPage.Month))
                {
                    if (new DateTime(currentViewPage.Year, currentViewPage.Month, d).Date > DateTime.Now.Date)
                    {
                        day[i][j].CanNotSelcect();
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

    int GetDayOfWeek(int dayOfWeek)
    {
        if (dayOfWeek == 0) return 6;
        else return dayOfWeek - 1;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isMovingPage || EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.CompareTag("Arrow")) return;
            Vector2 mousePos = UIController.instance.ScreenToWorldPoint(Input.mousePosition);
            offsetX = pageParent.position.x - mousePos.x;
            startX = mousePos.x;
            isDrag = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isMovingPage || EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.CompareTag("Arrow")) return;
            isDrag = false;
            isLessThan0 = startX - UIController.instance.ScreenToWorldPoint(Input.mousePosition).x < 0;
            if (!isLessThan0)
            {
                Next();
            }
            else
            {
                Back();
            }
            /*pageParent.DOMoveX(distancePage * indexPage, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
            {
                isMovingPage = false;
            });*/
        }

        if (isDrag)
        {
            Vector3 pos = UIController.instance.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Screen.height / 2, 100));
            pageParent.position = new Vector3(pos.x + offsetX, pos.y, pos.z);
        }
        else
        {
            if (isMovingPage)
            {
                pageParent.position = Vector3.MoveTowards(pageParent.position, new Vector3(distancePage * indexPage, pageParent.position.y, pageParent.position.z), 0.25f);
                if (pageParent.position.x == indexPage * distancePage)
                {
                    isMovingPage = false;
                    if (isLessThan0) LoadPage(dateSelect.AddMonths(-1));
                    else LoadPage(dateSelect.AddMonths(1));

                }
            }
        }
    }


    public void Next()
    {
        if (isMovingPage) return;
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
    }

    public void Back()
    {
        if (isMovingPage) return;
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
    }

    void HideBgAll(int index)
    {
        for (int i = 0; i < day[1].Length; i++)
        {
            if (i != index) day[1][i].BgHide();
        }
    }

    public void SelectDay(int index, int level)
    {
        HideBgAll(index);
        this.level = level;
    }
}

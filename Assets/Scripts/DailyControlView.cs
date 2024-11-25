using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DailyControlView : ScrollRect
{
    public RectTransform canvas;
    public GridLayoutGroup gridLayoutGroup;
    float startX;
    public Scrollbar scrollbar;
    bool isMoving;
    public Daily daily;
    bool isCanBack;
    bool isCanNext;


    protected override void Awake()
    {
        DOVirtual.DelayedCall(0.02f, delegate
        {
            gridLayoutGroup.cellSize = new Vector2(canvas.sizeDelta.x, canvas.sizeDelta.y - 170);
            scrollbar.value = 0.5f;
        });
    }

    public void LoadData()
    {
        isCanNext = false;
        isCanBack = true;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log(isCanNext);
        //Debug.Log(isCanBack);
        startX = eventData.position.x;
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        bool isOk = true;
        if (!isCanBack && eventData.position.x - startX > 0)
        {
            isOk = false;
            Debug.LogWarning("Can not back");
        }
        if (!isCanNext && eventData.position.x - startX < 0)
        {
            isOk = false;
            Debug.LogWarning("Can not next");
        }

        if (isOk) base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isMoving) return;
        bool isLeft = startX - eventData.position.x < 0;
        float value = isLeft ? 0f : 1f;

        if (!isLeft && !isCanNext) return;
        if (isLeft && !isCanBack) return;

        isCanNext = daily.IsCanNext();
        isCanBack = daily.IsCanBack();

        isMoving = true;
        DOVirtual.Float(scrollbar.value, value, 0.25f, (v) =>
        {
            scrollbar.value = v;
        }).OnComplete(delegate
        {
            if (isLeft) daily.LoadPage(daily.dateSelect.AddMonths(-1));
            else daily.LoadPage(daily.dateSelect.AddMonths(1));
            scrollbar.value = 0.5f;
            isMoving = false;
        });
        base.OnEndDrag(eventData);
    }

    public void Next()
    {
        if (daily.arrowRight.color.a < 1 && !isCanNext) return;

        isCanNext = daily.IsCanNext();
        isCanBack = daily.IsCanBack();

        if (isMoving) return;
        isMoving = true;
        DOVirtual.Float(scrollbar.value, 1f, 0.25f, (v) =>
        {
            scrollbar.value = v;
        }).OnComplete(delegate
        {
            daily.LoadPage(daily.dateSelect.AddMonths(1));
            scrollbar.value = 0.5f;
            isMoving = false;
        });
    }

    public void Back()
    {
        if (daily.arrowLeft.color.a < 1 && !isCanBack) return;

        isCanNext = daily.IsCanNext();
        isCanBack = daily.IsCanBack();

        if (isMoving) return;
        isMoving = true;
        
        DOVirtual.Float(scrollbar.value, 0f, 0.25f, (v) =>
        {
            scrollbar.value = v;
        }).OnComplete(delegate
        {
            daily.LoadPage(daily.dateSelect.AddMonths(-1));
            scrollbar.value = 0.5f;
            isMoving = false;
        });
    }
}
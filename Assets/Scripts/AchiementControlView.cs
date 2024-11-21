using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AchiementControlView : MonoBehaviour
{
    bool isDrag;
    bool isMoving;
    bool isChoiceDirection;
    bool isCanMovingHori;
    float offsetX;
    float startX;
    public RectTransform bar;
    Vector2 lastMousePosition;
    public RectTransform rect;
    public RectTransform scrollChallenge;
    public RectTransform scrollEvent;
    public RectTransform canvas;

    public ScrollRect scrollRectChallenge;
    public ScrollRect scrollRectEvent;

    public void Awake()
    {
        DOVirtual.DelayedCall(0.02f, delegate
        {
            rect.sizeDelta = new Vector2(canvas.sizeDelta.x * 2, rect.sizeDelta.y);
            bar.sizeDelta = new Vector2(canvas.sizeDelta.x / 2, bar.sizeDelta.y);
            bar.anchoredPosition = new Vector2(0, 0);
        });
    }

    public void Resize()
    {
        bar.anchoredPosition = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
    }

    public void MoveBar()
    {
        bar.DOKill();
        isMoving = true;
        float targetX = 0;
        if (rect.anchoredPosition.x == 0) targetX = -canvas.sizeDelta.x;
        rect.DOAnchorPosX(targetX, 0.25f).SetEase(Ease.Linear).OnComplete(delegate { isMoving = false; });
        bar.DOAnchorPosX(-targetX / 2, 0.25f).SetEase(Ease.Linear);
    }

    void DisabeScroll(bool isDisable)
    {
        scrollRectChallenge.vertical = !isDisable;
        scrollRectEvent.vertical = !isDisable;
        isCanMovingHori = isDisable;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving) return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, UIController.instance.cam, out Vector2 localPoint);
            offsetX = rect.anchoredPosition.x - localPoint.x;
            startX = localPoint.x;

            lastMousePosition = Input.mousePosition;
            isDrag = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            isChoiceDirection = false;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, UIController.instance.cam, out Vector2 localPoint);
            bool isLessThan0 = startX - localPoint.x < 0;
            if (Mathf.Abs(startX - localPoint.x) <= 2.5f || isMoving || !isCanMovingHori)
            {
            }
            else
            {
                isMoving = true;
                float targetX = 0;
                if (!isLessThan0) targetX = -canvas.sizeDelta.x;
                rect.DOAnchorPosX(targetX, 0.25f).SetEase(Ease.Linear).OnComplete(delegate { isMoving = false; });
                bar.DOAnchorPosX(-targetX / 2, 0.25f).SetEase(Ease.Linear);
            }
            isCanMovingHori = false;
        }
        if (isDrag)
        {
            if (!isChoiceDirection)
            {
                if (Vector2.Distance(lastMousePosition, Input.mousePosition) >= 2.5f)
                {
                    if (Mathf.Abs(lastMousePosition.x - Input.mousePosition.x) < Mathf.Abs(lastMousePosition.y - Input.mousePosition.y)) DisabeScroll(false);
                    else DisabeScroll(true);
                    isChoiceDirection = true;
                }
            }
            if (!isCanMovingHori) return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, UIController.instance.cam, out Vector2 localPoint);
            rect.anchoredPosition = new Vector2(Mathf.Clamp(localPoint.x + offsetX, -canvas.sizeDelta.x, 0), 0);
        }
    }
}

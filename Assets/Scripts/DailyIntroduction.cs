using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DailyIntroduction : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    float startX;
    float offsetX;
    bool isMovingIntro;
    public RectTransform popup;
    public RectTransform parent;
    public GameObject buttonHide;

    public void OnDrag(PointerEventData eventData)
    {
        if (isMovingIntro) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(popup, eventData.position, UIController.instance.cam, out Vector2 localPoint);
        parent.anchoredPosition = new Vector2(Mathf.Clamp(localPoint.x + offsetX, -300, 0), 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isMovingIntro) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(popup, eventData.position, UIController.instance.cam, out Vector2 localPoint);
        offsetX = parent.anchoredPosition.x - localPoint.x;
        startX = localPoint.x;
    }

    public void NextIntro()
    {
        buttonHide.SetActive(true);
        isMovingIntro = true;
        parent.DOAnchorPosX(-300, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
        {
            isMovingIntro = false;
        });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.LogWarning(Mathf.Abs(startX - localPoint.x));
        if (isMovingIntro || !eventData.dragging) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(popup, eventData.position, UIController.instance.cam, out Vector2 localPoint);
        bool isLessThan0 = startX - localPoint.x < 0;
        /*Debug.LogWarning(eventData.button);
        Debug.LogWarning(eventData.altitudeAngle);
        Debug.LogWarning(eventData.azimuthAngle);
        Debug.LogWarning(eventData.clickCount);
        Debug.LogWarning(eventData.clickTime);
        Debug.LogWarning(eventData.currentInputModule);
        Debug.LogWarning(eventData.delta);
        Debug.LogWarning(eventData.displayIndex);
        Debug.LogWarning(eventData.eligibleForClick);
        Debug.LogWarning(eventData.enterEventCamera);
        Debug.LogWarning(eventData.fullyExited);
        Debug.LogWarning(eventData.lastPress);
        Debug.LogWarning(eventData.penStatus);
        Debug.LogWarning(eventData.pointerClick);
        Debug.LogWarning(eventData.pointerCurrentRaycast);
        Debug.LogWarning(eventData.pointerDrag);
        Debug.LogWarning(eventData.pointerEnter);
        Debug.LogWarning(eventData.pointerId);
        Debug.LogWarning(eventData.pointerPress);
        Debug.LogWarning(eventData.pointerPressRaycast);
        Debug.LogWarning(eventData.position);
        Debug.LogWarning(eventData.pressEventCamera);
        Debug.LogWarning(eventData.pressPosition);
        Debug.LogWarning(eventData.pressure);
        Debug.LogWarning(eventData.radius);
        Debug.LogWarning(eventData.radiusVariance);
        Debug.LogWarning(eventData.rawPointerPress);
        Debug.LogWarning(eventData.reentered);
        Debug.LogWarning(eventData.scrollDelta);
        Debug.LogWarning(eventData.selectedObject);
        Debug.LogWarning(eventData.tangentialPressure);
        Debug.LogWarning(eventData.tilt);
        Debug.LogWarning(eventData.twist);
        Debug.LogWarning(eventData.used);
        Debug.LogWarning(eventData.useDragThreshold);
        Debug.LogWarning(eventData.worldNormal);
        Debug.LogWarning(eventData.worldPosition);*/
        float targetX = 0;
        if (!isLessThan0)
        {
            buttonHide.SetActive(true);
            targetX = -300;
        }
        else buttonHide.SetActive(false);

        isMovingIntro = true;
        parent.DOAnchorPosX(targetX, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
        {
            isMovingIntro = false;
        });
    }
}

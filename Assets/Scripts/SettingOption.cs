using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingOption : MonoBehaviour, IPointerClickHandler
{
    public UICommon.TypeSetting type;
    public GameObject buttonActive;
    public RectTransform dot;
    float targetX = 55;

    public void OnPointerClick(PointerEventData eventData)
    {
        UIController.instance.uICommon.ChangeSettingOption(this, !buttonActive.activeSelf, 0.25f);
    }

    public void SwitchStateHandle(bool isActive, float duration)
    {
        DoDot(isActive ? targetX : -targetX, duration);
        buttonActive.SetActive(isActive);
    }

    void DoDot(float x, float duration)
    {
        dot.DOKill();
        dot.DOAnchorPosX(x, duration);
    }
}

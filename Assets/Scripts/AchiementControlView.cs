using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AchiementControlView : MonoBehaviour
{
    public RectTransform canvas;
    public GridLayoutGroup gridLayoutGroup;
    public Scrollbar scrollbar;
    public RectTransform bar;

    public void Awake()
    {
        DOVirtual.DelayedCall(0.02f, delegate
        {
            gridLayoutGroup.cellSize = new Vector2(canvas.sizeDelta.x, canvas.sizeDelta.y - 288);
            bar.sizeDelta = new Vector2(canvas.sizeDelta.x / 2, bar.sizeDelta.y);
        });
    }

    public void LoadData()
    {
        scrollbar.value = 0;
        bar.anchoredPosition = Vector2.zero;
    }

    public void MovePage()
    {
        bool isLeft = scrollbar.value < 0.5f;
        float value = isLeft ? 1 : 0;
        DOVirtual.Float(scrollbar.value, value, 0.25f, (v) =>
        {
            scrollbar.value = v;
        });
        MoveBar(!isLeft);
    }

    public void MoveBar(bool isLeft)
    {
        bar.DOKill();
        float x = isLeft ? 0 : canvas.sizeDelta.x / 2;
        bar.DOAnchorPosX(x, 0.25f);
    }
}

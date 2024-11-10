using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;

    public CanvasGroup[] canvasBeforeWin;
    public CanvasGroup frameWin;
    public CanvasGroup panelWin;

    public RectTransform boxAreaParent;
    public RectTransform target;
    public GameObject wooden;
    Vector2 startBoxAreaParent;

    public Health[] healths;

    private void Awake()
    {
        startBoxAreaParent = boxAreaParent.position;
    }

    public void Back()
    {
        gamePlay.SetActive(false);
        home.SetActive(true);
    }

    public void Book()
    {

    }

    public void Hint()
    {

    }

    public void Win()
    {
        for (int i = 0; i < canvasBeforeWin.Length; i++)
        {
            canvasBeforeWin[i].DOFade(0f, 0.5f).SetEase(Ease.Linear);
        }
        DOVirtual.DelayedCall(0.5f, delegate
        {
            frameWin.gameObject.SetActive(true);
            frameWin.DOFade(1f, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
            {
                DOVirtual.DelayedCall(0.25f, delegate
                {
                    wooden.SetActive(true);
                    panelWin.gameObject.SetActive(true);
                    panelWin.DOFade(1f, 0.25f).SetEase(Ease.Linear);
                });
                boxAreaParent.DOMove(target.position, 0.5f).SetEase(Ease.Linear);
                boxAreaParent.DOScale(0.725f, 0.5f).SetEase(Ease.InBack);
            });
        });
    }

    public void Lose()
    {

    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Win();
        }
    }

    public void HealthSubstractAni(int index)
    {
        healths[index].SubtractHealthAni();
    }

    public void ResetHealth(out int health)
    {
        health = 3;
    }

    public void ResetWin()
    {
        wooden.SetActive(false);
        frameWin.gameObject.SetActive(false);
        panelWin.gameObject.SetActive(false);
        wooden.SetActive(false);
        frameWin.alpha = 0f;
        boxAreaParent.position = startBoxAreaParent;
        boxAreaParent.localScale = Vector3.one;
        for (int i = 0; i < canvasBeforeWin.Length; i++)
        {
            canvasBeforeWin[i].alpha = 0f;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < canvasBeforeWin.Length; i++)
        {
            canvasBeforeWin[i].DOKill();
        }
        panelWin.DOKill();
        frameWin.DOKill();
    }
}

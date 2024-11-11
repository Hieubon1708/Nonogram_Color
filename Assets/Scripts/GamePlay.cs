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

    public GameObject layerCover;
    public Image panelLose;
    public RectTransform popupLose;
    public RectTransform boxAreaParent;
    public RectTransform target;
    public GameObject wooden;
    public Mask mask;
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
            canvasBeforeWin[i].DOFade(0f, 0.25f).SetEase(Ease.Linear);
        }
        DOVirtual.DelayedCall(0.25f, delegate
        {
            mask.enabled = true;
            frameWin.gameObject.SetActive(true);
            frameWin.DOFade(1f, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
            {
                DOVirtual.DelayedCall(0.25f, delegate
                {
                    wooden.SetActive(true);
                    panelWin.gameObject.SetActive(true);
                    panelWin.DOFade(1f, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
                    {
                        layerCover.SetActive(false);
                    }) ;
                });
                boxAreaParent.DOMove(target.position, 0.5f).SetEase(Ease.Linear);
                boxAreaParent.DOScale(0.725f, 0.5f).SetEase(Ease.InBack);
            });
        });
    }

    public void ShowPanelLose()
    {
        layerCover.SetActive(true);
        DOVirtual.DelayedCall(1.25f, delegate
        {
            panelLose.gameObject.SetActive(true);
            UIController.instance.uICommon.ScalePopup(panelLose, popupLose, 234f / 255f, 0.1f, 1f, 0.5f);
        });
    }

    public void HidePanelLose()
    {
        UIController.instance.uICommon.ScalePopup(panelLose, popupLose, 0f, 0f, 0.9f, 0f);
        panelLose.gameObject.SetActive(false);
        layerCover.SetActive(false);
    }

    public void Replay()
    {
        GameController.instance.LoadLevel(GameController.instance.level);
        HidePanelLose();
        for (int i = 0; i < healths.Length; i++)
        {
            healths[i].Replay();
        }
    }

    public void PlusHealth()
    {
        GameController.instance.playerController.health++;
        HidePanelLose();
        healths[0].Replay();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameController.instance.boxController.Win();
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
        mask.enabled = false;
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

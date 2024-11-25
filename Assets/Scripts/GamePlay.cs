using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    public Hint hint;

    public GameObject home;
    public GameObject gamePlay;

    public CanvasGroup[] canvasBeforeWin;
    public CanvasGroup frameWin;
    public CanvasGroup panelWinFront;
    public CanvasGroup panelWinBack;

    public GameObject layerCover;
    public GameObject panelLose;
    public RectTransform boxAreaParent;
    public RectTransform target;
    public GameObject wooden;
    public TextMeshProUGUI textObject;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textNextLevel;
    public Mask mask;
    Vector3 startBoxAreaParent = Vector3.zero;

    public Health[] healths;

    public void SwitchFontWin(CanvasGroup canvas, out CanvasGroup temp)
    {
        temp = panelWinFront;
        panelWinFront = canvas;
    }

    public void LoadLevel(LevelConfig levelConfig)
    {
        textObject.text = levelConfig.name;
        textLevel.text = "Level " + PlayerPrefs.GetInt("Level", 1);
        textNextLevel.text = "Next Level " + (PlayerPrefs.GetInt("Level", 1) + 1);
    }

    public void Back()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            gamePlay.SetActive(false);
            home.SetActive(true);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void Book()
    {

    }

    public void Hint()
    {

    }

    public void Win()
    {
        if(startBoxAreaParent == Vector3.zero) startBoxAreaParent = boxAreaParent.position;
        for (int i = 0; i < canvasBeforeWin.Length; i++)
        {
            canvasBeforeWin[i].DOFade(0f, 0.5f).SetEase(Ease.Linear);
        }
        DOVirtual.DelayedCall(0.5f, delegate
        {
            mask.enabled = true;
            frameWin.gameObject.SetActive(true);
            frameWin.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
            {
                DOVirtual.DelayedCall(0.5f, delegate
                {
                    wooden.SetActive(true);
                    panelWinFront.gameObject.SetActive(true);
                    panelWinFront.DOFade(1f, 0.5f).SetEase(Ease.Linear);
                    panelWinBack.gameObject.SetActive(true);
                    panelWinBack.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
                    {
                        layerCover.SetActive(false);
                    });
                    UIController.instance.PlayFxWin();
                });
                boxAreaParent.DOMove(target.position, 0.5f).SetEase(Ease.Linear);
                boxAreaParent.DOScale(0.725f, 0.5f).SetEase(Ease.Linear);
            });
        });
    }

    public void ShowPanelLose()
    {
        layerCover.SetActive(true);
        DOVirtual.DelayedCall(1.25f, delegate
        {
            panelLose.SetActive(true);
            layerCover.SetActive(false);
        });
    }

    public void NextLevel()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            ResetWin();
            for (int i = 0; i < healths.Length; i++)
            {
                healths[i].Replay();
            }
            UIController.instance.StopFxWin();
            GameController.instance.GetLevel(PlayerPrefs.GetInt("Level", 1), -1);
            GameController.instance.SaveLevel();
            GameController.instance.LoadLevel();
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void Home()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            UIController.instance.StopFxWin();
            ResetWin();
            gamePlay.SetActive(false);
            home.SetActive(true);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void HidePanelLose()
    {
        panelLose.SetActive(false);
        layerCover.SetActive(false);
    }

    public void Replay()
    {
        GameController.instance.SaveLevel();
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            GameController.instance.GetLevel(GameController.instance.level, GameController.instance.levelStorage);
            GameController.instance.LoadLevel();
            HidePanelLose();
            for (int i = 0; i < healths.Length; i++)
            {
                healths[i].Replay();
            }
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void LoadDataStorage(int health)
    {
        for (int i = 0; i < healths.Length; i++)
        {
            if (i < health) healths[i].Replay();
            else healths[i].SubtractHealthAni();
        }
    }

    public void PlusHealth()
    {
        GameController.instance.playerController.health++;
        GameController.instance.SaveLevel(GameController.instance.playerController.health);
        HidePanelLose();
        healths[0].Replay();
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
        panelWinBack.gameObject.SetActive(false);
        panelWinFront.gameObject.SetActive(false);
        wooden.SetActive(false);
        mask.enabled = false;
        frameWin.alpha = 0f;
        boxAreaParent.position = startBoxAreaParent;
        boxAreaParent.localScale = Vector3.one;
        for (int i = 0; i < canvasBeforeWin.Length; i++)
        {
            canvasBeforeWin[i].alpha = 1f;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < canvasBeforeWin.Length; i++)
        {
            canvasBeforeWin[i].DOKill();
        }
        panelWinBack.DOKill();
        panelWinFront.DOKill();
        frameWin.DOKill();
    }
}

using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public UICommon uICommon;
    public GamePlay gamePlay;
    public Home home;

    public FalseCircle[] falseCircles;
    int falseIndex;

    private void Awake()
    {
        instance = this;
    }

    public void LoadLevel(LevelConfig levelConfig)
    {
        for (int i = 0; i < falseCircles.Length; i++)
        {
            falseCircles[i].SetSize(levelConfig);
        }
        gamePlay.LoadLevel(levelConfig);
    }

    public void PlayFalse(Vector2 pos, Box box)
    {
        FalseCircle falseCircle = falseCircles[falseIndex];
        falseCircle.boxSelected = box;
        falseCircle.gameObject.transform.position = pos;
        falseCircle.gameObject.SetActive(true);
        falseIndex++;
        if (falseIndex == falseCircles.Length) falseIndex = 0;
        DOVirtual.DelayedCall(0.4f, delegate
        {
            falseCircle.EndFalse();
        }).SetUpdate(true);
    }

    public void ButtonSelect(ButtonSelector[] buttons, ButtonSelector buttonSelected, float timeIn, float timeOut)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].DOKill();
            int index = i;
            if (buttons[index] == buttonSelected)
            {
                buttons[index].bgSelected.gameObject.SetActive(true);
                buttons[index].bgSelected.transform.DOScale(1f, timeOut).SetEase(Ease.OutBack);
                buttons[index].bgSelected.DOFade(1f, timeOut).SetEase(Ease.OutBack);
            }
            else
            {
                buttons[index].bgSelected.transform.DOScale(0.7f, timeIn).SetEase(Ease.Linear);
                buttons[index].bgSelected.DOFade(0f, timeIn).SetEase(Ease.Linear).OnComplete(delegate
                {
                    buttons[index].bgSelected.gameObject.SetActive(false);
                });
            }
        }
    }

    public int GetButtonIndex()
    {
        for (int i = 0; i < GameController.instance.playerController.buttonSelectors.Length; i++)
        {
            if (GameController.instance.playerController.buttonSelectors[i].hex == GameController.instance.playerController.hexSelected)
            {
                return i;
            }
        }
        return -1;
    }

    public ButtonSelector GetButtonByHex(string hex)
    {
        for (int i = 0; i < GameController.instance.playerController.buttonSelectors.Length; i++)
        {
            if (GameController.instance.playerController.buttonSelectors[i].hex == hex)
            {
                return GameController.instance.playerController.buttonSelectors[i];
            }
        }
        return null;
    }

    public void CheckRemainingDominantColor(string hex)
    {
        Box[][] boxes = GameController.instance.boxController.boxes;

        bool isRemaining = false;

        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes.Length; j++)
            {
                if (boxes[i][j].mainHex == hex && !boxes[i][j].isVisible)
                {
                    isRemaining = true;
                    break;
                }
            }
            if (isRemaining) break;
        }

        if (!isRemaining)
        {
            ButtonSelector buttonSelector = GetButtonByHex(hex);
            if (buttonSelector != null)
            {
                buttonSelector.isDone = true;
                buttonSelector.ButtonFade();

                for (int i = 0; i < GameController.instance.playerController.buttonSelectors.Length; i++)
                {
                    if (!GameController.instance.playerController.buttonSelectors[i].isDone && GameController.instance.playerController.buttonSelectors[i].hex != "#FFFFFF")
                    {
                        GameController.instance.playerController.buttonSelectors[i].OnPointerClick(null);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("Button is null");
            }
        }
    }
}

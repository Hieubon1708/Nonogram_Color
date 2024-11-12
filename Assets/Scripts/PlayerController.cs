using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string hexSelected;
    public int health;
    public RectTransform gridButton;
    public ButtonSelector[] buttonSelectors;
    public bool isDrag;
    public int totalBoxSelected;
    public int totalToWin;

    public void LoadLevel(LevelConfig levelConfig)
    {
        ResetButtons();
        UIController.instance.gamePlay.ResetHealth(out health);
        totalBoxSelected = 0;
        totalToWin = levelConfig.totalToWin;
        for (int i = 0; i < levelConfig.buttonConfigs.Length; i++)
        {
            buttonSelectors[i + 1].LoadLevel(levelConfig.buttonConfigs[i].buttonHex, levelConfig.buttonConfigs[i].fontHex);
        }
        gridButton.localScale = Vector3.one * (0.6f + ((5 - levelConfig.buttonConfigs.Length) * 0.08f));
        GameController.instance.uIController.ButtonSelect(buttonSelectors, buttonSelectors[1], 0f, 0f);
        SetColorSelect(buttonSelectors[1].hex);
    }

    public void SubtractHealth()
    {
        UIController.instance.gamePlay.HealthSubstractAni(health - 1);
        health--;
        if (health == 0 && totalBoxSelected != totalToWin)
        {
            GameController.instance.uIController.gamePlay.ShowPanelLose();
        }
    }

    public void CheckWin()
    {
        if (totalBoxSelected == totalToWin)
        {
            GameController.instance.boxController.Win();
            GameController.instance.uIController.gamePlay.layerCover.SetActive(true);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            GameController.instance.uIController.home.UpdateTextLevel();
        }
    }

    void ResetButtons()
    {
        for (int i = 1; i < buttonSelectors.Length; i++)
        {
            buttonSelectors[i].ResetButton();
        }
    }

    public void SetColorSelect(string hex)
    {
        hexSelected = hex;
    }

    public int GetButtonIndex()
    {
        for (int i = 0; i < buttonSelectors.Length; i++)
        {
            if (buttonSelectors[i].hex == hexSelected)
            {
                return i;
            }
        }
        return -1;
    }
}

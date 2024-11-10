using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string hexSelected;
    public int health;
    public RectTransform gridButton;
    public ButtonSelector[] buttonSelectors;

    public void LoadLevel(LevelConfig levelConfig)
    {
        ResetButtons();
        UIController.instance.gamePlay.ResetHealth(out health);
        for (int i = 0; i < levelConfig.buttonConfigs.Length; i++)
        {
            buttonSelectors[i + 1].LoadLevel(levelConfig.buttonConfigs[i].buttonHex, levelConfig.buttonConfigs[i].fontHex);
        }
        gridButton.localScale = Vector3.one * (0.6f + (levelConfig.buttonConfigs.Length * (0.4f / 5)));
        GameController.instance.uIController.ButtonSelect(buttonSelectors, buttonSelectors[1], 0f, 0f);
        SetColorSelect(buttonSelectors[1].hex);
    }

    public void SubtractHealth()
    {
        UIController.instance.gamePlay.HealthSubstractAni(health - 1);
        health--;
        if (health == 0)
        {
            GameController.instance.uIController.gamePlay.Lose();
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
}

using TMPro;
using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;
    public TextMeshProUGUI textLevel;

    public TMP_InputField inputField;

    public void ChangeLevel()
    {
        int level = int.Parse(inputField.text);
        if (level >= 1 && level <= 16)
        {
            PlayerPrefs.SetInt("Level", level);
            Play();
        }
    }

    public void NoAds()
    {

    }

    public void Play()
    {
        inputField.gameObject.SetActive(false);
        home.SetActive(false);
        gamePlay.SetActive(true);
        GameController.instance.LoadLevel(PlayerPrefs.GetInt("Level", 1));
    }

    public void UpdateTextLevel()
    {
        textLevel.text = "Lv." + PlayerPrefs.GetInt("Level", 1);
    }

    public void Achievement()
    {

    }

    public void DailyChallenge()
    {

    }

    public void Collection()
    {

    }
}

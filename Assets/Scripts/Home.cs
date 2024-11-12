using TMPro;
using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;
    public TextMeshProUGUI textLevel;
    public void NoAds()
    {

    }

    public void Play()
    {
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

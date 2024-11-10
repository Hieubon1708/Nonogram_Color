using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;

    public void NoAds()
    {

    }

    public void Play()
    {
        home.SetActive(false);
        gamePlay.SetActive(true);
        GameController.instance.LoadLevel(1);
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

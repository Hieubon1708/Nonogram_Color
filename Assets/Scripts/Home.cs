using TMPro;
using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;
    public GameObject collection;
    public TextMeshProUGUI textLevel;

    public TMP_InputField inputField;

    public void ChangeLevel()
    {
        int level = int.Parse(inputField.text);
        PlayerPrefs.SetInt("Level", level);
        Play();
    }

    public void NoAds()
    {

    }

    public void Play()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            inputField.gameObject.SetActive(false);
            home.SetActive(false);
            gamePlay.SetActive(true);
            if (UIController.instance.collection.collection.activeSelf) UIController.instance.collection.collection.SetActive(false);
            GameController.instance.LoadLevel(PlayerPrefs.GetInt("Level", 1));

            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
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
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            home.SetActive(false);
            collection.SetActive(true);

            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }
}

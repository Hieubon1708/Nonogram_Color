using TMPro;
using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;
    public GameObject collection;
    public GameObject daily;
    public GameObject achievement;
    public TextMeshProUGUI textLevel;
    public GameObject question;
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


    public void ShowQuestion()
    {
        GameController.instance.levelDataStorage = GameController.instance.dataManager.GetLevelStorage(-1);
        if (!GameController.instance.levelDataStorage.isClicked)
        {
            Play();
        }
        else
        {
            question.SetActive(true);
        }
    }

    public void Resum()
    {
        question.SetActive(false);
        Play();
    }
    
    public void Restart()
    {
        GameController.instance.GetLevel(PlayerPrefs.GetInt("Level", 1), -1);
        GameController.instance.SaveLevel();
        question.SetActive(false);
        Play();
    }

    public void QuestionCancel()
    {
        question.SetActive(false);
    }

    public void Play()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            GameController.instance.isLoadData = true;
            home.SetActive(false);
            gamePlay.SetActive(true);
            if (UIController.instance.collection.collection.activeSelf) UIController.instance.collection.collection.SetActive(false);
            GameController.instance.GetLevel(PlayerPrefs.GetInt("Level", 1), -1);
            GameController.instance.LoadLevel();

            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, delegate
            {
                GameController.instance.isLoadData = false;
            });
        });
    }

    public void UpdateTextLevel()
    {
        textLevel.text = "Lv." + PlayerPrefs.GetInt("Level", 1);
    }

    public void Achievement()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            home.SetActive(false);
            achievement.SetActive(true);
            UIController.instance.challenge.LoadData();
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void DailyChallenge()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            home.SetActive(false);
            daily.SetActive(true);
            UIController.instance.daily.LoadData();
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
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

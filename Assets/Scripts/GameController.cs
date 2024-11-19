using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public DataManager.TypeLevel typeLevel;

    public BoxController boxController;
    public ClusterController clusterController;
    public PlayerController playerController;
    public DataManager dataManager;
    public UIController uIController;
    public LineGenerator lineGenerator;
    public GameObject tutorial;

    public int level;
    public LevelConfig levelConfig;
    public LevelDataStorage levelDataStorage;

    public void Awake()
    {
        Application.targetFrameRate = 60;
        DOTween.SetTweensCapacity(200, 500);
        instance = this;
        lineGenerator.Generate();
        dataManager.DataReader();
        uIController.home.UpdateTextLevel();
        //PlayerPrefs.SetInt("Level", level);

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            tutorial.SetActive(true);
        }
    }

    public void Resume(int level)
    {
        levelDataStorage = dataManager.GetLevelStorage(level);

    }

    public void SaveLevel(int i, int j, string mainHex)
    {
        levelDataStorage.boxDataStorage[i][j].isVisible = true;
        if (mainHex != "#FFFFFFF")
        {
            levelDataStorage.totalSelect++;
            levelDataStorage.isClicked = true;
        }
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public void SaveLevel(bool isDone, int index)
    {
        levelDataStorage.buttonDataStorage[index].isDone = isDone;
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public void SaveLevel(int health)
    {
        levelDataStorage.healthRemaining = health;
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public void SaveLevel()
    {
        levelDataStorage.isClicked = false;
        levelDataStorage.totalSelect = 0;
        for (int i = 0; i < levelConfig.boxConfigs.Length; i++)
        {
            for (int j = 0; j < levelDataStorage.boxDataStorage[i].Length; j++)
            {
                levelDataStorage.boxDataStorage[i][j].isVisible = false;
            }
        }
        for (int i = 0; i < levelConfig.buttonConfigs.Length; i++)
        {
            levelDataStorage.buttonDataStorage[i].isDone = false;
        }
        levelDataStorage.healthRemaining = 3;
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public void SaveLevel(int i, int j)
    {
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public void LoadLevel(int level)
    {
        levelConfig = dataManager.GetLevel(level);
        levelDataStorage = dataManager.GetLevelStorage(level);
        typeLevel = levelConfig.typeLevel;

        boxController.LoadLevel(levelConfig, levelDataStorage);
        playerController.LoadLevel(levelConfig, levelDataStorage);
        clusterController.LoadLevel(levelConfig);
        lineGenerator.LoadLevel(levelConfig);
        uIController.LoadLevel(levelConfig);
    }

    public string GetFontColor(string bgHex)
    {
        for (int i = 0; i < playerController.buttonSelectors.Length; i++)
        {
            if (playerController.buttonSelectors[i].hex == bgHex) return playerController.buttonSelectors[i].fontHex;
        }
        return "#FFFFF";
    }

    public bool ColorConvert(string hex, out Color color)
    {
        if (ColorUtility.TryParseHtmlString(hex, out color)) return true;
        return false;
    }
}

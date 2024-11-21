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
    public int levelStorage;
    public LevelConfig levelConfig;
    public LevelDataStorage levelDataStorage;

    public bool isLoadData;

    public void Awake()
    { 
        instance = this;
        Application.targetFrameRate = 60;
        DOTween.SetTweensCapacity(200, 500);

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

    }

    public void SaveLevel(int health)
    {
        levelDataStorage.healthRemaining = health;
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public void SaveLevel(int i, int j, string mainHex, string hexSelect)
    {
        levelDataStorage.boxDataStorage[i][j].isVisible = true;
        levelDataStorage.isClicked = true;
        levelDataStorage.boxDataStorage[i][j].hexSelect = hexSelect;
        if (mainHex != "#FFFFFF")
        {
            levelDataStorage.totalSelect++;
            if(levelDataStorage.totalSelect == levelConfig.totalToWin) levelDataStorage.isCompleted = true;
        }
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public void SaveLevel(int i, int j)
    {
        levelDataStorage.boxDataStorage[i][j].isX = true;
        //Debug.LogWarning(i + " - " + j);
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public void SaveLevel()
    {
        levelDataStorage.isClicked = false;
        levelDataStorage.totalSelect = 0;
        levelDataStorage.healthRemaining = 3;
        for (int i = 0; i < levelDataStorage.boxDataStorage.Length; i++)
        {
            for (int j = 0; j < levelDataStorage.boxDataStorage[i].Length; j++)
            {
                levelDataStorage.boxDataStorage[i][j].isVisible = false;
                levelDataStorage.boxDataStorage[i][j].isX = false;
            }
        }
        dataManager.SaveLevel(levelDataStorage, level);
    }

    public int GetX()
    {
        int c = 0;
        for (int i = 0; i < levelDataStorage.boxDataStorage.Length; i++)
        {
            for (int j = 0; j < levelDataStorage.boxDataStorage[i].Length; j++)
            {
                if (levelDataStorage.boxDataStorage[i][j].isX) c++;
            }
        }
        return c;
    }

    public void GetLevel(int level, int levelStorage)
    {
        this.level = level;
        this.levelStorage = levelStorage;
        levelConfig = dataManager.GetLevel(level);
        levelDataStorage = dataManager.GetLevelStorage(levelStorage);
        typeLevel = levelConfig.typeLevel;
    }

    public void LoadLevel()
    {
        playerController.LoadLevel(levelConfig, levelDataStorage);
        boxController.LoadLevel(levelConfig, levelDataStorage);
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

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

    public void Awake()
    {
        DOTween.SetTweensCapacity(200, 125);
        instance = this;
        lineGenerator.Generate();
        dataManager.DataReader();
        uIController.home.UpdateTextLevel();
        PlayerPrefs.SetInt("Level", 12);
    }

    public void LoadLevel(int level)
    {
        LevelConfig levelConfig = dataManager.GetLevel(level);
        typeLevel = levelConfig.typeLevel;

        boxController.LoadLevel(levelConfig);
        playerController.LoadLevel(levelConfig);
        clusterController.LoadLevel(levelConfig);
        lineGenerator.LoadLevel(levelConfig);
        uIController.LoadLevel(levelConfig);
    }

    public string GetFontColor(string bgHex)
    {
        for (int i = 0; i < playerController.buttonSelectors.Length; i++)
        {
            if(playerController.buttonSelectors[i].hex == bgHex) return playerController.buttonSelectors[i].fontHex;
        }
        return "#FFFFF";
    }

    public bool ColorConvert(string hex, out Color color)
    {
        if (ColorUtility.TryParseHtmlString(hex, out color)) return true;
        return false;
    }
}
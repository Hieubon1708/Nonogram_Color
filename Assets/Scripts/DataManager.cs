using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public enum TypeLevel
    {
        Level5x5, Level10x10, Level15x15, Level20x20
    }

    public SizeConfig[] sizeConfig;   

    public void DataReader()
    {
        TextAsset sizeConfigJs = Resources.Load<TextAsset>("SizeConfig");
        sizeConfig = JsonConvert.DeserializeObject<SizeConfig[]>(sizeConfigJs.text);
    }

    public LevelConfig GetLevel(int level)
    {
        TextAsset js = Resources.Load<TextAsset>("Levels/" + level);
        return JsonConvert.DeserializeObject<LevelConfig>(js.text);
    }

    public void SaveLevel()
    {

    }
}

[System.Serializable]
public class SizeConfig
{
    public int percentX;
    public int amountBox;
    public float boxCellSize;
    public float fontSize;
    public int contrainCount;
    public int limitSpaceInCluster;
}

[System.Serializable]
public class LevelConfig
{
    public string name;
    public DataManager.TypeLevel typeLevel;
    public int totalToWin;
    public ButtonConfig[] buttonConfigs;
    public BoxConfig[][] boxConfigs;
}

[System.Serializable]
public class ButtonConfig
{
    public string buttonHex;
    public string fontHex;
}

[System.Serializable]
public class BoxConfig
{
    public string mainHex;
    public string extraHex;
}

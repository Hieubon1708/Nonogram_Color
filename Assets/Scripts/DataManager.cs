using Newtonsoft.Json;
using System.IO;
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

    public LevelDataStorage GetLevelStorage(int level)
    {
        string dataJs = Path.Combine(Application.persistentDataPath, level + ".json");
        if (File.Exists(dataJs))
        {
            string levelDataStorage = File.ReadAllText(dataJs);
            return JsonConvert.DeserializeObject<LevelDataStorage>(levelDataStorage);
        }
        return new LevelDataStorage();
    }

    public void SaveLevel(LevelDataStorage levelDataStorage, int level)
    {
        string js = JsonConvert.SerializeObject(levelDataStorage);
        string path = Path.Combine(Application.persistentDataPath, level + ".json");
        File.WriteAllText(path, js);
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
public class LevelDataStorage
{
    public bool isClicked;
    public int totalSelect;
    public int healthRemaining;
    public BoxDataStorage[][] boxDataStorage;

    public LevelDataStorage()
    {
        healthRemaining = 3;
    }
}

[System.Serializable]
public class BoxDataStorage
{
    public bool isVisible;
    public string hexSelect;
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

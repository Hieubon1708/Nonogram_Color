using DG.Tweening;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class PixelGetter : MonoBehaviour
{
    public Texture2D[] images;

    public ColorSeter[] colorSeters;

    string HexConvert(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }

    void Start()
    {
        for (int i = 0; i < images.Length; i++)
        {
            LevelConfig levelConfig = new LevelConfig();
            Color[] pixels = images[i].GetPixels();

            if (pixels.Length == 25) levelConfig.typeLevel = DataManager.TypeLevel.Level5x5;
            if (pixels.Length == 100) levelConfig.typeLevel = DataManager.TypeLevel.Level10x10;
            if (pixels.Length == 225) levelConfig.typeLevel = DataManager.TypeLevel.Level15x15;
            if (pixels.Length == 400) levelConfig.typeLevel = DataManager.TypeLevel.Level20x20;

            ButtonConfig[] buttonConfigs = new ButtonConfig[colorSeters[i].dominantColors.Length];
            for (int k = 0; k < buttonConfigs.Length; k++)
            {
                ButtonConfig buttonConfig = new ButtonConfig();
                buttonConfig.buttonHex = HexConvert(colorSeters[i].dominantColors[k].dominantColor);
                buttonConfig.fontHex = HexConvert(colorSeters[i].dominantColors[k].fontColor);

                buttonConfigs[k] = buttonConfig;
            }
            BoxConfig[][] boxConfigs = new BoxConfig[(int)Mathf.Sqrt(pixels.Length)][];

            int totalToWin = 0;
            int count = 0;
            int index = boxConfigs.Length - 1;
            BoxConfig[] boxConfigsChild = null;
            for (int h = 0; h < pixels.Length; h++)
            {
                string extraHex = HexConvert(pixels[h]);
                if (extraHex != "#FFFFFF") totalToWin++;
                if (count == 0)
                {
                    boxConfigsChild = new BoxConfig[boxConfigs.Length];
                }
                BoxConfig boxConfig = new BoxConfig();
                boxConfig.mainHex = HexConvert(GetDominantColor(pixels[h], i));
                boxConfig.extraHex = extraHex;

                boxConfigsChild[count] = boxConfig;
                count++;
                if (count == boxConfigs.Length)
                {
                    boxConfigs[index] = boxConfigsChild;
                    index--;
                    count = 0;
                }
            }
            levelConfig.totalToWin = totalToWin;
            levelConfig.buttonConfigs = buttonConfigs;
            levelConfig.boxConfigs = boxConfigs;

            string js = JsonConvert.SerializeObject(levelConfig);
            string path = Path.Combine(Application.dataPath, "Resources/Levels/" + (i + 1) + ".json");
            File.WriteAllText(path, js);
        }
    }

    Color GetDominantColor(Color color, int index)
    {
        for (int i = 0; i < colorSeters[index].dominantColors.Length; i++)
        {
            for (int j = 0; j < colorSeters[index].dominantColors[i].uniformColor.Length; j++)
            {
                if (colorSeters[index].dominantColors[i].uniformColor[j] == color)
                {
                    return colorSeters[index].dominantColors[i].dominantColor;
                }
            }
        }
        return Color.white;
    }
}

[System.Serializable]
public class ColorSeter
{
    public DominantColor[] dominantColors;
}

[System.Serializable]
public class DominantColor
{
    public Color dominantColor;
    public Color fontColor;
    public Color[] uniformColor;
}


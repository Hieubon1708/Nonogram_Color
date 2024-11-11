using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClusterIndex : MonoBehaviour
{
    public Image bg;
    public TextMeshProUGUI num;
    public Animation flicker;

    public void LoadData(int amount, string hex)
    {
        gameObject.SetActive(true);
        if (hex == "#FFFFFF")
        {
            bg.color = Vector4.zero;
            num.color = Vector4.zero;
            return;
        }
        Color color;
        if (GameController.instance.ColorConvert(hex, out color)) bg.color = color;
        else Debug.LogError("Not found " + gameObject.name + " / " + hex);
        num.text = amount.ToString();
        num.fontSize = GameController.instance.dataManager.sizeConfig[(int)GameController.instance.typeLevel].fontSize;

        if (GameController.instance.ColorConvert(GameController.instance.GetFontColor(hex), out color)) num.color = color;
        else Debug.LogError("Not found " + gameObject.name + " / " + hex);
    }

    public void Flicker()
    {
        flicker.Play("FadeCluster");
    }

    public void ResetClusterIndex()
    {
        gameObject.SetActive(false);
        bg.color = Vector4.one;
        num.color = Vector4.one;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClusterIndex : MonoBehaviour
{
    public Image bg;
    public TextMeshProUGUI num;
    public Animation flicker;
    public CanvasGroup canvasGroup;
    public bool isDone;

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
        num.fontSizeMax = GameController.instance.dataManager.sizeConfig[(int)GameController.instance.typeLevel].fontSize;

        if (GameController.instance.ColorConvert(GameController.instance.GetFontColor(hex), out color)) num.color = color;
        else Debug.LogError("Not found " + gameObject.name + " / " + hex);
    }

    public void Flicker()
    {
        if (canvasGroup.alpha == 0.5f) return;
        if (!GameController.instance.isLoadData)
        {
            flicker.Play("FadeCluster");
        }
        else
        {
            canvasGroup.alpha = 0.5f;
        }
    }

    public void CompletedCluster()
    {
        isDone = true;
    }

    public void ResetClusterIndex()
    {
        isDone = false;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(false);
        bg.color = Vector4.one;
        num.color = Vector4.one;
    }
}

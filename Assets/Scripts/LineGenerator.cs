using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject preLineDown;
    public GameObject preLineLeft;

    public RectTransform[] lineDown = new RectTransform[20];
    public RectTransform[] lineLeft = new RectTransform[20];

    float sizeArea = 850;

    public void Generate()
    {
        for (int i = 0; i < lineDown.Length; i++)
        {
            lineDown[i] = Instantiate(preLineDown, transform).GetComponent<RectTransform>();
            lineLeft[i] = Instantiate(preLineLeft, transform).GetComponent<RectTransform>();
        }
    }

    public void LoadLevel(LevelConfig levelConfig)
    {
        ResetLine();
        SizeConfig sizeConfig = GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel];
        for (int i = 0; i < sizeConfig.amountBox; i++)
        {
            lineDown[i].anchoredPosition = new Vector2(0, i * sizeConfig.boxCellSize);
            lineLeft[i].anchoredPosition = new Vector2(i * sizeConfig.boxCellSize, 0);
        }
    }

    public void ResetLine()
    {
        for (int i = 0; i < lineDown.Length; i++)
        {
            lineDown[i].anchoredPosition = Vector2.zero;
            lineLeft[i].anchoredPosition = Vector2.zero;
        }
    }
}

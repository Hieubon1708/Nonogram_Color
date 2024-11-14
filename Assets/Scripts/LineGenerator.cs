using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject preBorderHor;
    public GameObject preBorderVer;
    public GameObject preLineHor;
    public GameObject preLineVer;

    public RectTransform[] borderHor = new RectTransform[4];
    public RectTransform[] borderVer = new RectTransform[4];
    public RectTransform[] lineHor = new RectTransform[20];
    public RectTransform[] lineVer = new RectTransform[20];

    public RectTransform[] borderOut;

    float sizeArea = 850;

    public void Generate()
    {
        for (int i = 0; i < lineHor.Length; i++)
        {
            lineHor[i] = Instantiate(preLineHor, transform).GetComponent<RectTransform>();
            lineVer[i] = Instantiate(preLineVer, transform).GetComponent<RectTransform>();
        }
        for (int i = 0; i < borderHor.Length; i++)
        {
            borderHor[i] = Instantiate(preBorderHor, transform).GetComponent<RectTransform>();
            borderVer[i] = Instantiate(preBorderVer, transform).GetComponent<RectTransform>();
        }
    }

    public void LoadLevel(LevelConfig levelConfig)
    {
        ResetLine();
        SizeConfig sizeConfig = GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel];

        int widthLine = 0;

        if (sizeConfig.amountBox == 5) widthLine = 5;
        if (sizeConfig.amountBox == 10) widthLine = 4;
        if (sizeConfig.amountBox == 15) widthLine = 3;
        if (sizeConfig.amountBox == 20) widthLine = 3;

        for (int i = 1; i < sizeConfig.amountBox; i++)
        {
            lineHor[i].anchoredPosition = new Vector2(0, i * sizeConfig.boxCellSize - 425);
            lineVer[i].anchoredPosition = new Vector2(i * sizeConfig.boxCellSize - 425, 0);

            lineHor[i].sizeDelta = new Vector2(0, widthLine);
            lineVer[i].sizeDelta = new Vector2(widthLine, 0);

            lineHor[i].gameObject.SetActive(true);
            lineVer[i].gameObject.SetActive(true);
        }

        int widthBorder = 0;

        if (sizeConfig.amountBox == 5) widthBorder = 6;
        if (sizeConfig.amountBox == 10) widthBorder = 5;
        if (sizeConfig.amountBox == 15) widthBorder = 4;
        if (sizeConfig.amountBox == 20) widthBorder = 4;

        for (int i = 1; i < sizeConfig.amountBox / 5; i++)
        {
            borderHor[i].anchoredPosition = new Vector2(0, i * sizeConfig.boxCellSize * 5 - 425);
            borderVer[i].anchoredPosition = new Vector2(i * sizeConfig.boxCellSize * 5 - 425, 0);

            borderHor[i].sizeDelta = new Vector2(0, widthBorder);
            borderVer[i].sizeDelta = new Vector2(widthBorder, 0);

            borderHor[i].gameObject.SetActive(true);
            borderVer[i].gameObject.SetActive(true);
        }

        borderOut[0].sizeDelta = new Vector2(0, widthBorder);
        borderOut[1].sizeDelta = new Vector2(0, widthBorder);
        borderOut[2].sizeDelta = new Vector2(widthBorder, 0);
        borderOut[3].sizeDelta = new Vector2(widthBorder, 0);
    }

    public void ResetLine()
    {
        for (int i = 0; i < lineHor.Length; i++)
        {
            lineHor[i].gameObject.SetActive(false);
            lineVer[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < borderHor.Length; i++)
        {
            borderHor[i].gameObject.SetActive(false);
            borderVer[i].gameObject.SetActive(false);
        }
    }
}

using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public GameObject collectorPre;
    public Collector[] collector;
    public RectTransform container;
    public Sprite[] sprites;
    public Image image;
    public TextMeshProUGUI nameObj;
    public int level;
    public GameObject panelCollection;
    public GameObject barBackCollection;
    public GameObject gamePlay;
    public GameObject collection;
    public GameObject back;
    public GameObject backOrigin;
    public GameObject home;
    public CanvasGroup fontWin;
    public CanvasGroup tempFontWin;
    public GameObject playButton;
    public GameObject label;

    public void Awake()
    {
        collector = new Collector[sprites.Length];
        for (int i = 0; i < collector.Length; i++)
        {
            collector[i] = Instantiate(collectorPre, container).GetComponent<Collector>();
            collector[i].gameObject.SetActive(false);
        }
    }

    public void Start()
    {
        int length = PlayerPrefs.GetInt("Level");
        bool isOk = false;
        for (int i = 0; i < length - 1; i++)
        {
            if (!isOk) isOk = true;
            AddCollector(i);
        }
        if (!isOk)
        {
            playButton.SetActive(true);
        }
    }

    public void AddCollector(int index)
    {
        if (playButton.activeSelf) playButton.SetActive(false);
        collector[index].nameObj = Regex.Replace(sprites[index].name, "[0-9_]", "");
        collector[index].index = index + 1;
        collector[index].image.sprite = sprites[index];
        collector[index].gameObject.SetActive(true);
    }

    public void ShowPanelCollection(Sprite sprite, string name, int level)
    {
        image.sprite = sprite;
        nameObj.text = name;
        this.level = level;
        panelCollection.SetActive(true);
    }

    public void AcceptResart()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            collection.SetActive(false);
            gamePlay.SetActive(true);
            back.SetActive(true);
            backOrigin.SetActive(false);
            label.SetActive(true);
            panelCollection.SetActive(false);
            UIController.instance.gamePlay.SwitchFontWin(fontWin, out tempFontWin);
            GameController.instance.GetLevel(level, -100);
            GameController.instance.SaveLevel();
            GameController.instance.LoadLevel();

            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void BackQuestion()
    {
        bool isClick = false;
        for (int i = 0; i < GameController.instance.boxController.boxes.Length; i++)
        {
            for (int j = 0; j < GameController.instance.boxController.boxes[i].Length; j++)
            {
                if (GameController.instance.boxController.boxes[i][j].isVisible)
                {
                    isClick = true;
                    break;
                }
            }
        }

        if (isClick) barBackCollection.SetActive(true);
        else Back();
    }

    public void Back()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            gamePlay.SetActive(false);
            collection.SetActive(true);
            back.SetActive(false);
            backOrigin.SetActive(true);
            label.SetActive(false);
            barBackCollection.SetActive(false);
            ResetWin();
            UIController.instance.StopFxWin();
            UIController.instance.gamePlay.SwitchFontWin(tempFontWin, out fontWin);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void BackHome()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            home.SetActive(true);
            collection.SetActive(false);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void BackCancel()
    {
        barBackCollection.SetActive(false);
    }

    public void Cancel()
    {
        panelCollection.SetActive(false);
    }

    void ResetWin()
    {
        fontWin.alpha = 0;
        tempFontWin.alpha = 0;
        UIController.instance.gamePlay.ResetWin();
    }
}

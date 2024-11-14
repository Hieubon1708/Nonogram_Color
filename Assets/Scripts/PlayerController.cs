using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public string hexSelected;
    public int health;
    public RectTransform gridButton;
    public ButtonSelector[] buttonSelectors;
    public bool isDrag;
    public int totalBoxSelected;
    public int totalToWin;
    public List<GameObject> boxPassed = new List<GameObject>();

    public void LoadLevel(LevelConfig levelConfig)
    {
        ResetButtons();
        UIController.instance.gamePlay.ResetHealth(out health);
        totalBoxSelected = 0;
        totalToWin = levelConfig.totalToWin;
        for (int i = 0; i < levelConfig.buttonConfigs.Length; i++)
        {
            buttonSelectors[i + 1].LoadLevel(levelConfig.buttonConfigs[i].buttonHex, levelConfig.buttonConfigs[i].fontHex);
        }
        gridButton.localScale = Vector3.one * (0.6f + ((5 - levelConfig.buttonConfigs.Length) * 0.08f));
        GameController.instance.uIController.ButtonSelect(buttonSelectors, buttonSelectors[1], 0f, 0f);
        SetColorSelect(buttonSelectors[1].hex);
    }

    public void SubtractHealth()
    {
        UIController.instance.gamePlay.HealthSubstractAni(health - 1);
        health--;
        if (health == 0 && totalBoxSelected != totalToWin)
        {
            GameController.instance.uIController.gamePlay.ShowPanelLose();
        }
    }

    float GetSameBoxPassed(ref int isSameRow)
    {
        float value = 0f;
        if (boxPassed[0].transform.position.x == boxPassed[1].transform.position.x)
        {
            value = boxPassed[0].transform.position.x;
            isSameRow = 1;
        }
        if (boxPassed[0].transform.position.y == boxPassed[1].transform.position.y)
        {
            value = boxPassed[0].transform.position.y;
            isSameRow = -1;
        }
        return value;
    }


    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            boxPassed.Clear();
        }

        if (isDrag)
        {
            Vector2 mousePosition = Input.mousePosition;
            if (boxPassed.Count >= 2)
            {
                int isSameRow = 0;
                float value = GetSameBoxPassed(ref isSameRow);

                float x = isSameRow == 1 ? value : Input.mousePosition.x;
                float y = isSameRow == -1 ? value : Input.mousePosition.y;

                mousePosition = new Vector2(x, y);
            }
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            for (int i = 0; i < results.Count; i++)
            {
                GameObject e = results[i].gameObject;
                if (e.CompareTag("Box") && !boxPassed.Contains(e))
                {
                    boxPassed.Add(e);
                    Box box = GameController.instance.boxController.GetBox(e);
                    box.Show();
                }
            }
        }
    }

    public void CheckWin()
    {
        if (totalBoxSelected == totalToWin)
        {
            GameController.instance.boxController.Win();
            GameController.instance.uIController.gamePlay.layerCover.SetActive(true);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            GameController.instance.uIController.home.UpdateTextLevel();
        }
    }

    void ResetButtons()
    {
        for (int i = 1; i < buttonSelectors.Length; i++)
        {
            buttonSelectors[i].ResetButton();
        }
    }

    public void SetColorSelect(string hex)
    {
        hexSelected = hex;
    }
}

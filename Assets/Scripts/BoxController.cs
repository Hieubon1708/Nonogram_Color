using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BoxController : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public Box[] pool;
    public Box[][] boxes;

    public void LoadLevel(LevelConfig levelConfig)
    {
        ResetBoxes();

        gridLayoutGroup.constraintCount = GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel].contrainCount;
        float cellSize = GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel].boxCellSize;
        gridLayoutGroup.cellSize = Vector2.one * cellSize;

        List<Box> x = new List<Box>();
        int indexPool = 0;
        int row = levelConfig.boxConfigs.Length;
        ResizeX(row);
        boxes = new Box[row][];
        for (int i = 0; i < row; i++)
        {
            int col = levelConfig.boxConfigs[i].Length;
            Box[] boxesInRow = new Box[col];
            for (int j = 0; j < col; j++)
            {
                boxesInRow[j] = pool[indexPool];
                string mainColor = levelConfig.boxConfigs[i][j].mainHex;
                string extraColor = levelConfig.boxConfigs[i][j].extraHex;

                if (mainColor == "#FFFFFF") x.Add(pool[indexPool]);
                boxesInRow[j].LoadLevel(mainColor, extraColor);
                indexPool++;
            }
            boxes[i] = boxesInRow;
        }
        int xCount = x.Count * GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel].percentX / 100;
        while (xCount > 0)
        {
            int indexRandom = Random.Range(0, x.Count);
            x[indexRandom].IsX();
            x.RemoveAt(indexRandom);
            xCount--;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                for (int j = 0; j < boxes[i].Length; j++)
                {
                    Color color;
                    ColorUtility.TryParseHtmlString(boxes[i][j].mainHex, out color);
                    boxes[i][j].image.color = color;
                }
            }
        }
    }

    void ResizeX(int row)
    {
        float size = 0;
        if (row == 5) size = 95;
        if (row == 10) size = 55;
        if (row == 15) size = 35;
        if (row == 20) size = 25;
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i].ResizeX(size);
        }
    }

    public void Win()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                Box box = boxes[j][i];
                DOVirtual.DelayedCall(0.25f * (j * 0.25f), delegate
                {
                    box.EndDo(0.25f);
                });
            }
        }
        DOVirtual.DelayedCall((boxes.Length) * (0.25f * 0.25f), delegate
        {
            GameController.instance.uIController.gamePlay.Win();
        });
    }

    void ResetBoxes()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i].ResetBox();
        }
    }

    public Box GetBox(Box box)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                if (boxes[i][j] == box)
                {
                    return boxes[i][j];
                }
            }
        }
        return null;
    }

    public int GetRow(Box box)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                if (boxes[i][j] == box)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public int GetCol(Box box)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                if (boxes[i][j] == box)
                {
                    return j;
                }
            }
        }
        return -1;
    }
}

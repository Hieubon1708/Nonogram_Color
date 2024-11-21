using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxController : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public Box[] pool;
    public Box[][] boxes;
    public List<BOXDO> boxDO = new List<BOXDO>();

    public void LoadLevel(LevelConfig levelConfig, LevelDataStorage levelDataStorage)
    {
        ResetBoxes();
        bool isNull = false;
        if (levelDataStorage.boxDataStorage == null)
        {
            isNull = true;
            levelDataStorage.boxDataStorage = new BoxDataStorage[levelConfig.boxConfigs.Length][];
            for (int i = 0; i < levelConfig.boxConfigs.Length; i++)
            {
                BoxDataStorage[] boxesChild = new BoxDataStorage[levelConfig.boxConfigs[i].Length];
                for (int j = 0; j < boxesChild.Length; j++)
                {
                    boxesChild[j] = new BoxDataStorage();
                }
                levelDataStorage.boxDataStorage[i] = boxesChild;
            }
        }
        gridLayoutGroup.constraintCount = GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel].contrainCount;
        float cellSize = GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel].boxCellSize;
        gridLayoutGroup.cellSize = Vector2.one * cellSize;
        if (!levelDataStorage.isClicked)
        {
            boxDO.Clear();
            GameController.instance.SaveLevel();
        }
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

                if (mainColor == "#FFFFFF" && !levelDataStorage.isClicked && boxes.Length != 5)
                {
                    x.Add(pool[indexPool]);
                    boxDO.Add(new BOXDO(i, j));
                }
                boxesInRow[j].LoadLevel(mainColor, extraColor);
                indexPool++;
            }
            boxes[i] = boxesInRow;
        }
        if (!levelDataStorage.isClicked && boxes.Length != 5)
        {
            int xCount = x.Count * GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel].percentX / 100;
            /*Debug.LogWarning(xCount);
            Debug.LogWarning(GameController.instance.GetX());*/
            while (xCount > 0)
            {
                int indexRandom = Random.Range(0, x.Count);
                x[indexRandom].IsX();
                GameController.instance.SaveLevel(boxDO[indexRandom].i, boxDO[indexRandom].j);
                x.RemoveAt(indexRandom);
                boxDO.RemoveAt(indexRandom);
                xCount--;
            }
            //Debug.LogWarning(GameController.instance.GetX());
        }

        if (isNull) return;
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                if (levelDataStorage.boxDataStorage[i][j].isVisible)
                {
                    GameController.instance.playerController.hexSelected = levelDataStorage.boxDataStorage[i][j].hexSelect;
                    GameController.instance.playerController.isDrag = true;
                    boxes[i][j].Show();
                    GameController.instance.playerController.isDrag = false;
                }
                if (levelDataStorage.boxDataStorage[i][j].isX)
                {
                    boxes[i][j].x.gameObject.SetActive(true);
                    boxes[i][j].isVisible = true;
                }
            }
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

    public Box GetBox(GameObject box)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                if (boxes[i][j].gameObject == box)
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

[System.Serializable]
public class BOXDO
{
    public int i;
    public int j;
    public BOXDO(int i, int j)
    {
        this.i = i;
        this.j = j;
    }
}
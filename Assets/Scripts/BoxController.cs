using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Box[] pool;
    public Box[][] boxes;

    public void LoadLevel(LevelConfig levelConfig)
    {
        ResetBoxes();
        int indexPool = 0;
        int row = levelConfig.boxConfigs.Length;
        boxes = new Box[row][];
        for (int i = 0; i < row; i++)
        {
            int col = levelConfig.boxConfigs[i].Length;
            Box[] boxesInRow = new Box[col];
            for (int j = 0; j < col; j++)
            {
                boxesInRow[j] = pool[indexPool++];

                string mainColor = levelConfig.boxConfigs[i][j].mainHex;
                string extraColor = levelConfig.boxConfigs[i][j].extraHex;

                boxesInRow[j].LoadLevel(mainColor, extraColor);
            }
            boxes[i] = boxesInRow;
        }
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
                if (boxes[i][j] == box)
                {
                    return boxes[i][j];
                }
            }
        }
        return null;
    }
}

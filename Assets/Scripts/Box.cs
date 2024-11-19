using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    public bool isVisible;
    public string mainHex;
    public string extraHex;
    public Image image;
    public Image x;
    public RectTransform rectX;
    public Image xSelected;
    public RectTransform rectXSelected;
    public List<Box> rowClusters;
    public List<Box> colClusters;
    public ClusterIndex rowClusterIndex;
    public ClusterIndex colClusterIndex;
    public Animation ani;

    public void LoadLevel(string mainHex, string extraHex)
    {
        this.mainHex = mainHex;
        this.extraHex = extraHex;
        gameObject.SetActive(true);
    }

    public void ResizeX(float size)
    {
        rectX.sizeDelta = Vector2.one * size;
        rectXSelected.sizeDelta = Vector2.one * size;
    }

    public void Show()
    {
        if (!GameController.instance.playerController.isDrag || isVisible || x.gameObject.activeSelf || GameController.instance.playerController.health == 0) return;
        isVisible = true;
        string hexSelected = GameController.instance.playerController.hexSelected;
        if (mainHex != "#FFFFFF") UIController.instance.CheckRemainingDominantColor(mainHex);
        Color color;
        if (GameController.instance.uIController.gamePlay.hint.isHint)
        {
            if (mainHex == "#FFFFFF")
            {
                x.gameObject.SetActive(true);
                CheckLineByX();
            }
            else
            {
                if (GameController.instance.ColorConvert(mainHex, out color))
                {
                    image.DOColor(color, 0.1f);
                    CheckLine();

                    GameController.instance.playerController.totalBoxSelected++;
                    GameController.instance.playerController.CheckWin();
                }
                else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
            }
            if (GameController.instance.uIController.gamePlay.hint) GameController.instance.uIController.gamePlay.hint.Deselect();
        }
        else
        {
            if (hexSelected == "#FFFFFF")
            {
                if (mainHex != hexSelected)
                {
                    xSelected.gameObject.SetActive(true);
                    GameController.instance.playerController.isDrag = false;
                    DOVirtual.DelayedCall(0.15f, delegate
                    {
                        GameController.instance.playerController.totalBoxSelected++;
                        xSelected.gameObject.SetActive(false);
                        GameController.instance.playerController.SubtractHealth();
                        GameController.instance.uIController.PlayFalse(transform.position, this);
                    }).SetUpdate(true);
                }
                else
                {
                    x.gameObject.SetActive(true);
                    CheckLineByX();
                }
            }
            else if (GameController.instance.ColorConvert(hexSelected, out color))
            {
                image.DOColor(color, 0.1f).OnComplete(delegate
                {
                    if (mainHex != hexSelected)
                    {
                        if (mainHex != "#FFFFFF") GameController.instance.playerController.totalBoxSelected++;
                        image.color = Color.white;
                        GameController.instance.playerController.SubtractHealth();
                        GameController.instance.uIController.PlayFalse(transform.position, this);
                    }
                });
                if (mainHex != hexSelected) GameController.instance.playerController.isDrag = false;
                else
                {
                    if (mainHex != "#FFFFFF")
                    {
                        GameController.instance.playerController.totalBoxSelected++;
                        GameController.instance.playerController.CheckWin();
                    }
                    CheckLine();
                }
            }
            else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
        }
    }

    void CheckLineByX()
    {
        int row = GameController.instance.boxController.GetRow(this);
        int col = GameController.instance.boxController.GetCol(this);
        GameController.instance.SaveLevel(row, col, mainHex);
        Box[][] boxes = GameController.instance.boxController.boxes;
        CheckRowClusterIndex(boxes, row);
        CheckColClusterIndex(boxes, col);
    }

    public void ShowX()
    {
        ani.Play("Flicker");
        if (!x.gameObject.activeSelf && !xSelected.gameObject.activeSelf) x.gameObject.SetActive(true);
    }

    public void CheckLine()
    {
        int row = GameController.instance.boxController.GetRow(this);
        int col = GameController.instance.boxController.GetCol(this);
        GameController.instance.SaveLevel(row, col, mainHex);
        bool isRowOk = true;
        bool isColOk = true;

        Box[][] boxes = GameController.instance.boxController.boxes;

        if (GameController.instance.playerController.totalToWin != GameController.instance.playerController.totalBoxSelected)
        {
            for (int i = 0; i < GameController.instance.boxController.boxes[row].Length; i++)
            {
                if (!boxes[row][i].isVisible
                    && boxes[row][i].mainHex != "#FFFFFF") isRowOk = false;
            }
            if (isRowOk)
            {
                for (int i = col; i >= 0; i--)
                {
                    if (boxes[row][i].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        boxes[row][index].isVisible = true;
                        DOVirtual.DelayedCall(0.15f * ((col - i) * 0.15f), delegate
                        {
                            boxes[row][index].ShowX();
                        });
                    }
                }
                DOVirtual.DelayedCall(0.15f * ((col + 1) * 0.15f), delegate
                {
                    GameController.instance.clusterController.rowClusters[row].Flicker();
                    CheckRowClusterIndex(boxes, row);
                });
                for (int i = col; i < boxes[row].Length; i++)
                {
                    if (boxes[row][i].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        boxes[row][index].isVisible = true;
                        DOVirtual.DelayedCall(0.15f * ((col - i) * 0.15f), delegate
                        {
                            boxes[row][index].ShowX();
                        });
                    }
                }
            }
            for (int i = 0; i < GameController.instance.boxController.boxes.Length; i++)
            {
                if (!boxes[i][col].isVisible
                    && boxes[i][col].mainHex != "#FFFFFF") isColOk = false;
            }
            if (isColOk)
            {
                for (int i = row; i >= 0; i--)
                {
                    if (boxes[i][col].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        boxes[index][col].isVisible = true;
                        DOVirtual.DelayedCall(0.15f * ((row - i) * 0.15f), delegate
                        {
                            boxes[index][col].ShowX();
                        });
                    }
                }
                DOVirtual.DelayedCall(0.15f * ((row + 1) * 0.15f), delegate
                {
                    GameController.instance.clusterController.colClusters[col].Flicker();
                    CheckColClusterIndex(boxes, col);
                });
                for (int i = row; i < boxes.Length; i++)
                {
                    if (boxes[i][col].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        boxes[index][col].isVisible = true;
                        DOVirtual.DelayedCall(0.15f * ((row - i) * 0.15f), delegate
                        {
                            boxes[index][col].ShowX();
                        });
                    }
                }
            }
        }

        bool isClusterOk = true;
        for (int i = 0; i < rowClusters.Count; i++)
        {
            if (!rowClusters[i].isVisible) isClusterOk = false;
        }
        if (isClusterOk)
        {
            rowClusterIndex.CompletedCluster();
            if (!isRowOk)
            {
                CheckRowClusterIndex(boxes, row);
            }
        }
        isClusterOk = true;
        for (int i = 0; i < colClusters.Count; i++)
        {
            if (!colClusters[i].isVisible) isClusterOk = false;
        }
        if (isClusterOk)
        {
            colClusterIndex.CompletedCluster();
            if (!isColOk)
            {
                CheckColClusterIndex(boxes, col);
            }
        }
    }

    void CheckRowClusterIndex(Box[][] boxes, int index)
    {
        for (int i = 0; i < boxes[index].Length; i++)
        {
            if (boxes[index][i].isVisible)
            {
                if (boxes[index][i].rowClusterIndex == null) continue;
                if (boxes[index][i].rowClusterIndex.isDone)
                {
                    boxes[index][i].rowClusterIndex.Flicker();
                }
            }
            else break;
        }
        for (int i = boxes.Length - 1; i >= 0; i--)
        {
            if (boxes[index][i].isVisible)
            {
                if (boxes[index][i].rowClusterIndex == null) continue;
                if (boxes[index][i].rowClusterIndex.isDone)
                {
                    boxes[index][i].rowClusterIndex.Flicker();
                }
            }
            else break;
        }
    }

    void CheckColClusterIndex(Box[][] boxes, int index)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i][index].isVisible)
            {
                if (boxes[i][index].colClusterIndex == null) continue;
                if (boxes[i][index].colClusterIndex.isDone)
                {
                    boxes[i][index].colClusterIndex.Flicker();
                }
            }
            else break;
        }
        for (int i = boxes.Length - 1; i >= 0; i--)
        {
            if (boxes[i][index].isVisible)
            {
                if (boxes[i][index].colClusterIndex == null) continue;
                if (boxes[i][index].colClusterIndex.isDone)
                {
                    boxes[i][index].colClusterIndex.Flicker();
                }
            }
            else break;
        }
    }

    public void EndDo(float time)
    {
        Color color;
        if (x.gameObject.activeSelf) x.DOFade(0f, time);
        if (xSelected.gameObject.activeSelf) xSelected.DOFade(0f, time);
        if (GameController.instance.ColorConvert(extraHex, out color)) image.DOColor(color, time).SetEase(Ease.Linear);
        else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
    }

    public void EndFalse()
    {
        if (mainHex == "#FFFFFF")
        {
            xSelected.gameObject.SetActive(true);
        }
        else
        {
            Color color;
            if (GameController.instance.ColorConvert(mainHex, out color))
            {
                image.DOColor(color, 0.1f);
                CheckLine();
                GameController.instance.playerController.CheckWin();
            }
            else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
        }
    }

    public void IsX()
    {
        if (mainHex == "#FFFFFF") x.gameObject.SetActive(true);
        isVisible = true;
    }

    public void ResetBox()
    {
        gameObject.SetActive(false);
        x.gameObject.SetActive(false);
        xSelected.gameObject.SetActive(false);
        image.color = Color.white;
        isVisible = false;
    }

    public void OnDestroy()
    {
        transform.DOKill();
        x.DOKill();
        xSelected.DOKill();
        image.DOKill();
    }
}

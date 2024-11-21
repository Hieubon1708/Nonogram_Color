using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Reflection;
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
    public Animation xAni;
    public Animation xSelectAni;

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
        if (!GameController.instance.isLoadData) SaveLevel();
        string hexSelected = GameController.instance.playerController.hexSelected;
        if (mainHex != "#FFFFFF") UIController.instance.CheckRemainingDominantColor(mainHex);
        Color color;
        if (GameController.instance.uIController.gamePlay.hint.isHint)
        {
            if (mainHex == "#FFFFFF")
            {
                x.gameObject.SetActive(true);
                CheckLineByX(this);
            }
            else
            {
                if (GameController.instance.ColorConvert(mainHex, out color))
                {
                    float time = !GameController.instance.isLoadData ? 0.1f : 0;
                    image.DOColor(color, time);
                    CheckLine();

                    if(!GameController.instance.isLoadData) GameController.instance.playerController.totalBoxSelected++;
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
                    float time = !GameController.instance.isLoadData ? 0.15f : 0;
                    DOVirtual.DelayedCall(time, delegate
                {
                    if (!GameController.instance.isLoadData) GameController.instance.playerController.totalBoxSelected++;
                    xSelected.gameObject.SetActive(false);
                    GameController.instance.playerController.SubtractHealth();
                    if (!GameController.instance.isLoadData) GameController.instance.uIController.PlayFalse(transform.position, this);
                    else EndFalse();
                }).SetUpdate(true);
                }
                else
                {
                    x.gameObject.SetActive(true);
                    CheckLineByX(this);
                }
            }
            else if (GameController.instance.ColorConvert(hexSelected, out color))
            {
                float time = !GameController.instance.isLoadData ? 0.1f : 0;
                image.DOColor(color, time).OnComplete(delegate
                {
                    if (mainHex != hexSelected)
                    {
                        if (mainHex != "#FFFFFF" && !GameController.instance.isLoadData) GameController.instance.playerController.totalBoxSelected++;
                        image.color = Color.white;
                        GameController.instance.playerController.SubtractHealth();
                        if (!GameController.instance.isLoadData) GameController.instance.uIController.PlayFalse(transform.position, this);
                        else EndFalse();
                    }
                });
                if (mainHex != hexSelected) GameController.instance.playerController.isDrag = false;
                else
                {
                    if (mainHex != "#FFFFFF")
                    {
                        if (!GameController.instance.isLoadData) GameController.instance.playerController.totalBoxSelected++;
                        GameController.instance.playerController.CheckWin();
                    }
                    CheckLine();
                }
            }
            else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
        }
    }

    void CheckLineByX(Box box)
    {
        int row = GameController.instance.boxController.GetRow(box);
        int col = GameController.instance.boxController.GetCol(box);
        Box[][] boxes = GameController.instance.boxController.boxes;
        CheckRowClusterIndex(boxes, row);
        CheckColClusterIndex(boxes, col);
    }

    public void ShowX()
    {
        if (!GameController.instance.isLoadData) ani.Play("Flicker");
        if (!x.gameObject.activeSelf && !xSelected.gameObject.activeSelf)
        {
            x.gameObject.SetActive(true);
            if (!GameController.instance.isLoadData)
            {
                xAni.Play("ScaleX");
            }
        }
    }

    void SaveLevel()
    {
        int row = GameController.instance.boxController.GetRow(this);
        int col = GameController.instance.boxController.GetCol(this);
        GameController.instance.SaveLevel(row, col, mainHex, GameController.instance.playerController.hexSelected);
    }

    public void CheckLine()
    {
        int row = GameController.instance.boxController.GetRow(this);
        int col = GameController.instance.boxController.GetCol(this);

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
                        float timeShowX = !GameController.instance.isLoadData ? 0.15f * ((col - i) * 0.15f) : 0;
                        DOVirtual.DelayedCall(timeShowX, delegate
                        {
                            boxes[row][index].ShowX();
                            CheckColCluster(boxes, index, false, 1);
                        });
                    }
                }
                float time = !GameController.instance.isLoadData ? 0.15f * ((col + 1) * 0.15f) : 0;
                DOVirtual.DelayedCall(time, delegate
                {
                    if (!GameController.instance.isLoadData) GameController.instance.clusterController.rowClusters[row].Flicker();
                    CheckRowClusterIndex(boxes, row);
                });
                for (int i = col; i < boxes[row].Length; i++)
                {
                    if (boxes[row][i].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        boxes[row][index].isVisible = true;
                        float timeShowX = !GameController.instance.isLoadData ? 0.15f * ((col - i) * 0.15f) : 0;
                        DOVirtual.DelayedCall(timeShowX, delegate
                        {
                            boxes[row][index].ShowX();
                            CheckColCluster(boxes, index, false, 1);
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
                        float timeShowX = !GameController.instance.isLoadData ? 0.15f * ((row - i) * 0.15f) : 0;
                        DOVirtual.DelayedCall(timeShowX, delegate
                        {
                            boxes[index][col].ShowX();
                            //CheckRowCluster(boxes[index][col], boxes, index, false);
                        });
                    }
                }
                float time = !GameController.instance.isLoadData ? 0.15f * ((row + 1) * 0.15f) : 0;
                DOVirtual.DelayedCall(time, delegate
                {
                    if (!GameController.instance.isLoadData) GameController.instance.clusterController.colClusters[col].Flicker();
                    CheckColClusterIndex(boxes, col);
                });
                for (int i = row; i < boxes.Length; i++)
                {
                    if (boxes[i][col].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        boxes[index][col].isVisible = true;
                        float timeShowX = !GameController.instance.isLoadData ? 0.15f * ((row - i) * 0.15f) : 0;
                        DOVirtual.DelayedCall(timeShowX, delegate
                        {
                            boxes[index][col].ShowX();
                            //CheckRowCluster(boxes[index][col], boxes, index, false);
                        });
                    }
                }
            }
        }

        CheckColCluster(boxes, col, isColOk, 0);
        CheckRowCluster(boxes, row, isRowOk);
    }

    void CheckColCluster(Box[][] boxes, int col, bool isColOk, int a)
    {
        for (int j = 0; j < boxes[col].Length; j++)
        {
            bool isClusterOk = true;
            if (boxes[j][col].colClusters.Count == 0) continue;
            for (int i = 0; i < boxes[j][col].colClusters.Count; i++)
            {
                if (!boxes[j][col].colClusters[i].isVisible) isClusterOk = false;
            }
            if (isClusterOk)
            {
                boxes[j][col].colClusterIndex.CompletedCluster();
                if (!isColOk)
                {
                    CheckColClusterIndex(boxes, col);
                }
            }
        }
    }

    void CheckRowCluster(Box[][] boxes, int row, bool isRowOk)
    {
        for (int j = 0; j < boxes[row].Length; j++)
        {
            bool isClusterOk = true;
            if (boxes[row][j].colClusters.Count == 0) continue;
            for (int i = 0; i < boxes[row][j].rowClusters.Count; i++)
            {
                if (!boxes[row][j].rowClusters[i].isVisible) isClusterOk = false;
            }
            if (isClusterOk)
            {
                boxes[row][j].rowClusterIndex.CompletedCluster();
                if (!isRowOk)
                {
                    CheckRowClusterIndex(boxes, row);
                }
            }
        }
    }

    void CheckRowClusterIndex(Box[][] boxes, int index)
    {
        /*for (int j = 0; j < boxes.Length; j++)
        {
            for (int i = 0; i < boxes[j].Length; i++)
            {
                if (boxes[j][i].isVisible)
                {
                    if (boxes[j][i].rowClusterIndex == null) continue;
                    if (boxes[j][i].rowClusterIndex.isDone)
                    {
                        boxes[j][i].rowClusterIndex.Flicker();
                    }
                }
                else break;
            }
        }
        for (int j = boxes.Length - 1; j >= 0; j--)
        {
            for (int i = boxes[j].Length - 1; i >= 0; i--)
            {
                if (boxes[j][i].isVisible)
                {
                    if (boxes[j][i].rowClusterIndex == null) continue;
                    if (boxes[j][i].rowClusterIndex.isDone)
                    {
                        boxes[j][i].rowClusterIndex.Flicker();
                    }
                }
                else break;
            }
        }*/
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
        /*for (int j = 0; j < boxes.Length; j++)
        {
            for (int i = 0; i < boxes[j].Length; i++)
            {
                if (boxes[i][j].isVisible)
                {
                    if (boxes[i][j].colClusterIndex == null) continue;
                    if (boxes[i][j].colClusterIndex.isDone)
                    {
                        boxes[i][j].colClusterIndex.Flicker();
                    }
                }
                else break;
            }
        }
        for (int j = boxes.Length - 1; j >= 0; j--)
        {
            for (int i = boxes[j].Length - 1; i >= 0; i--)
            {
                if (boxes[i][j].isVisible)
                {
                    if (boxes[i][j].colClusterIndex == null) continue;
                    if (boxes[i][j].colClusterIndex.isDone)
                    {
                        Debug.LogWarning(boxes[i][j].colClusterIndex);
                        boxes[i][j].colClusterIndex.Flicker();
                    }
                }
                else break;
            }
        }*/
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
            if (!GameController.instance.isLoadData) xSelectAni.Play("ScaleXSelected");
        }
        else
        {
            Color color;
            if (GameController.instance.ColorConvert(mainHex, out color))
            {
                float time = !GameController.instance.isLoadData ? 0.1f : 0;
                image.DOColor(color, time);
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

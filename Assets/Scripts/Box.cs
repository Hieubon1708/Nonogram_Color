using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Box : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameController.instance.playerController.isDrag = true;
        Show();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameController.instance.playerController.isDrag = false;
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
        Color color;
        if (GameController.instance.uIController.gamePlay.hint.isHint)
        {
            if (mainHex == "#FFFFFF")
            {
                x.gameObject.SetActive(true);
            }
            else
            {
                if (GameController.instance.ColorConvert(mainHex, out color))
                {
                    image.DOColor(color, 0.1f);
                    CheckLine();
                    GameController.instance.playerController.CheckWin();
                }
                else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
            }
            if (GameController.instance.uIController.gamePlay.hint) GameController.instance.uIController.gamePlay.hint.Deselect();
        }
        else
        {
            if (GameController.instance.playerController.hexSelected == "#FFFFFF")
            {
                if (mainHex != GameController.instance.playerController.hexSelected)
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
                else x.gameObject.SetActive(true);
            }
            else if (GameController.instance.ColorConvert(GameController.instance.playerController.hexSelected, out color))
            {
                image.DOColor(color, 0.1f).OnComplete(delegate
                {
                    if (mainHex != GameController.instance.playerController.hexSelected)
                    {
                        if (mainHex != "#FFFFFF") GameController.instance.playerController.totalBoxSelected++;
                        image.color = Color.white;
                        GameController.instance.playerController.SubtractHealth();
                        GameController.instance.uIController.PlayFalse(transform.position, this);
                    }
                });
                if (mainHex != GameController.instance.playerController.hexSelected) GameController.instance.playerController.isDrag = false;
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

    public void ShowX()
    {
        ani.Play("Flicker");
        if (!x.gameObject.activeSelf && !xSelected.gameObject.activeSelf) x.gameObject.SetActive(true);
    }

    public void CheckLine()
    {
        int row = GameController.instance.boxController.GetRow(this);
        int col = GameController.instance.boxController.GetCol(this);

        bool isRowOk = true;
        bool isColOk = true;

        if (GameController.instance.playerController.totalToWin != GameController.instance.playerController.totalBoxSelected)
        {
            for (int i = 0; i < GameController.instance.boxController.boxes[row].Length; i++)
            {
                if (!GameController.instance.boxController.boxes[row][i].isVisible
                    && GameController.instance.boxController.boxes[row][i].mainHex != "#FFFFFF") isRowOk = false;
            }
            if (isRowOk)
            {
                for (int i = col; i >= 0; i--)
                {
                    if (GameController.instance.boxController.boxes[row][i].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        GameController.instance.boxController.boxes[row][index].isVisible = true;
                        DOVirtual.DelayedCall(0.15f * ((col - i) * 0.15f), delegate
                        {
                            GameController.instance.boxController.boxes[row][index].ShowX();
                        });
                    }
                }
                DOVirtual.DelayedCall(0.15f * ((col + 1) * 0.15f), delegate
                {
                    GameController.instance.clusterController.rowClusters[row].Flicker();
                    rowClusterIndex.Flicker();
                });
                for (int i = col; i < GameController.instance.boxController.boxes[row].Length; i++)
                {
                    if (GameController.instance.boxController.boxes[row][i].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        GameController.instance.boxController.boxes[row][index].isVisible = true;
                        DOVirtual.DelayedCall(0.15f * ((col - i) * 0.15f), delegate
                        {
                            GameController.instance.boxController.boxes[row][index].ShowX();
                        });
                    }
                }
            }
            for (int i = 0; i < GameController.instance.boxController.boxes.Length; i++)
            {
                if (!GameController.instance.boxController.boxes[i][col].isVisible
                    && GameController.instance.boxController.boxes[i][col].mainHex != "#FFFFFF") isColOk = false;
            }
            if (isColOk)
            {
                for (int i = row; i >= 0; i--)
                {
                    if (GameController.instance.boxController.boxes[i][col].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        GameController.instance.boxController.boxes[index][col].isVisible = true;
                        DOVirtual.DelayedCall(0.15f * ((row - i) * 0.15f), delegate
                        {
                            GameController.instance.boxController.boxes[index][col].ShowX();
                        });
                    }
                }
                DOVirtual.DelayedCall(0.15f * ((row + 1) * 0.15f), delegate
                {
                    GameController.instance.clusterController.colClusters[col].Flicker();
                    colClusterIndex.Flicker();
                });
                for (int i = row; i < GameController.instance.boxController.boxes.Length; i++)
                {
                    if (GameController.instance.boxController.boxes[i][col].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        GameController.instance.boxController.boxes[index][col].isVisible = true;
                        DOVirtual.DelayedCall(0.15f * ((row - i) * 0.15f), delegate
                        {
                            GameController.instance.boxController.boxes[index][col].ShowX();
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
            if (!isRowOk)
            {
                GameController.instance.clusterController.rowClusters[row].Flicker();
            }
            rowClusterIndex.Flicker();
        }
        isClusterOk = true;
        for (int i = 0; i < colClusters.Count; i++)
        {
            if (!colClusters[i].isVisible) isClusterOk = false;
        }
        if (isClusterOk)
        {
            if (!isColOk)
            {
                GameController.instance.clusterController.colClusters[col].Flicker();
            }
            colClusterIndex.Flicker();
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

using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Box : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public bool isVisible;
    public string mainHex;
    public string extraHex;
    public Image image;
    public GameObject x;
    public ClusterIndex clusterIndex;

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

    public void Show()
    {
        if (!GameController.instance.playerController.isDrag || isVisible || x.activeSelf || GameController.instance.playerController.health == 0) return;
        isVisible = true;
        Color color;
        if (GameController.instance.ColorConvert(GameController.instance.playerController.hexSelected, out color))
        {
            image.DOColor(color, 0.1f).OnComplete(delegate
            {
                if (mainHex != GameController.instance.playerController.hexSelected)
                {
                    GameController.instance.playerController.isDrag = false;
                    GameController.instance.playerController.SubtractHealth();
                    image.color = Color.white;
                    GameController.instance.uIController.PlayFalse(transform.position, this);
                }
                if (mainHex != "#FFFFFF") GameController.instance.playerController.PlusBoxSelected();
            });
        }
        else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
    }

    public void EndDo(float time)
    {
        Color color;
        if (GameController.instance.ColorConvert(extraHex, out color)) image.DOColor(color, time).SetEase(Ease.Linear);
        else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
    }

    public void EndFalse()
    {
        if (mainHex == "#FFFFFF")
        {
            x.SetActive(true);
        }
        else
        {
            Color color;
            if (GameController.instance.ColorConvert(mainHex, out color))
            {
                image.DOColor(color, 0.1f);
            }
            else Debug.LogError("Not found " + gameObject.name + " / " + mainHex);
        }
    }

    public void IsX()
    {
        if (mainHex == "#FFFFFF") x.SetActive(true);
    }

    public void ResetBox()
    {
        gameObject.SetActive(false);
        x.SetActive(false);
        image.color = Color.white;
        isVisible = false;
    }

    public void OnDestroy()
    {
        transform.DOKill();
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Box : MonoBehaviour, IPointerClickHandler
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
        IsX();
        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isVisible || x.activeSelf || GameController.instance.playerController.health == 0) return;
        Show();
    }

    public void Show()
    {
        isVisible = true;
        Color color;
        if (GameController.instance.ColorConvert(GameController.instance.playerController.hexSelected, out color))
        {
            image.DOColor(color, 0.1f).OnComplete(delegate
            {
                if (mainHex != GameController.instance.playerController.hexSelected)
                {
                    GameController.instance.playerController.SubtractHealth();
                    image.color = Color.white;
                    GameController.instance.uIController.PlayFalse(transform.position, this);
                }
            });
        }
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
}

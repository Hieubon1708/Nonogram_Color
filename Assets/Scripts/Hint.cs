using DG.Tweening;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public GameObject selected;
    public GameObject ads;
    public bool isHint;
    int indexButtonSelect;

    public void OnClick()
    {
        if (!isHint) Select();
        else Deselect();
    }

    public void Select()
    {
        if (!ads)
        {
            
        }
        isHint = true;
        indexButtonSelect = GameController.instance.playerController.GetButtonIndex();
        selected.SetActive(true);
        ads.SetActive(false);
        transform.DOKill();
        transform.DOScale(1.1f, 0.25f);
        GameController.instance.uIController.ButtonSelect(GameController.instance.playerController.buttonSelectors, null, 0.15f, 0.25f);
    }

    public void Deselect()
    {
        isHint = false;
        selected.SetActive(false);
        ads.SetActive(true);
        transform.DOKill();
        transform.DOScale(1f, 0.15f);
        GameController.instance.uIController.ButtonSelect(GameController.instance.playerController.buttonSelectors, GameController.instance.playerController.buttonSelectors[indexButtonSelect], 0.15f, 0.25f);
    }

    public void HideHint()
    {
        isHint = false;
        selected.SetActive(false);
        transform.DOKill();
        transform.DOScale(1f, 0.15f);
    }

    public void OnDestroy()
    {
        transform.DOKill();
    }
}

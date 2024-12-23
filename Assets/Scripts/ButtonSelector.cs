using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour, IPointerClickHandler
{
    public string hex;
    public Image initial;
    public Image selected;
    public Image bgSelected;
    public string fontHex;
    public bool isDone;
    public CanvasGroup canvasGroup;
    public Animation ani;

    public void LoadLevel(string hex, string fontHex)
    {
        this.hex = hex;
        this.fontHex = fontHex;
        Color color;
        if (GameController.instance.ColorConvert(hex, out color))
        {
            initial.color = color;
            selected.color = color;
        }
        else Debug.LogError("Not found " + gameObject.name + " / " + hex);
        gameObject.SetActive(true);
    }

    public void ButtonFade()
    {
        if (!GameController.instance.isLoadData) ani.Play();
        else canvasGroup.alpha = 0.5f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isDone || !GameController.instance.playerController.gameObject.activeSelf) return;
        if(GameController.instance.uIController.gamePlay.hint) GameController.instance.uIController.gamePlay.hint.HideHint();
        GameController.instance.playerController.SetColorSelect(hex);
        float time1 = !GameController.instance.isLoadData ? 0.15f : 0;
        float time2 = !GameController.instance.isLoadData ? 0.25f : 0;
        GameController.instance.uIController.ButtonSelect(GameController.instance.playerController.buttonSelectors, this, time1, time2);
    }

    public void ResetButton()
    {
        canvasGroup.alpha = 1f;
        isDone = false;
        gameObject.SetActive(false);
    }
}

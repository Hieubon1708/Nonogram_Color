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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameController.instance.uIController.gamePlay.hint) GameController.instance.uIController.gamePlay.hint.HideHint();
        GameController.instance.playerController.SetColorSelect(hex);
        GameController.instance.uIController.ButtonSelect(GameController.instance.playerController.buttonSelectors, this, 0.15f, 0.25f);
    }

    public void ResetButton()
    {
        gameObject.SetActive(false);
    }
}

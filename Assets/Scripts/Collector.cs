using UnityEngine;
using UnityEngine.UI;

public class Collector : MonoBehaviour
{
    public int index;
    public Image image;
    public string nameObj;

    public void OnClick()
    {
        UIController.instance.collection.ShowPanelCollection(image.sprite, nameObj, index);
    }
}

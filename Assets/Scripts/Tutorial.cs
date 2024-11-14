using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;
    public int step = 1;
    public string[] content;
    public GameObject panelStart;
    public GameObject popupContent;
    public TextMeshProUGUI textContent;
    public TextMeshProUGUI textStep;

    public void StartTutorial()
    {
        panelStart.SetActive(false);
        Step();
    }

    public void Step()
    {
        if (step < 12)
        {
            textContent.text = content[step - 1];
            textStep.text = "step " + step + "/12";

            popupContent.SetActive(false);
            popupContent.SetActive(true);
            step++;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartTutorial();
        }if (Input.GetKeyDown(KeyCode.S))
        {
            Step();
        }
    }
}

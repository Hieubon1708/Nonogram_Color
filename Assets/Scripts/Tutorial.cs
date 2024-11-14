using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;

    public void StartTutorial()
    {

    }

    public void SkipTutorial()
    {

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartTutorial();
        }
    }
}

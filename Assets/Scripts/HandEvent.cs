using UnityEngine;

public class HandEvent : MonoBehaviour
{
    public Tutorial tutorial;

    public void MoveHand()
    {
        tutorial.MoveHand();
    }

    public void StartHand()
    {
        tutorial.StartHand();
    }
    
    public void HandUp()
    {
        tutorial.HandUp();
    }

    public void HandDown()
    {
        tutorial.HandDown();
    }
}

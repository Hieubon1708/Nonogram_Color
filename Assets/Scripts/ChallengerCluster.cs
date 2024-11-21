using System;
using TMPro;
using UnityEngine;

public class ChallengerCluster : MonoBehaviour
{
    public TextMeshProUGUI year;
    public Challenger[] challengers;
    public int index;
    public RectTransform parent;
    public RectTransform child;

    public void ResetChallegerCluster()
    {
        index = 0;
        parent.sizeDelta = child.sizeDelta;     
    }

    public void LoadData(DateTime date, DateTime releaseDate, Sprite cup, DataManager dataManager)
    {
        challengers[index].LoadData(date, releaseDate, cup, dataManager);
        index++;
    }
}

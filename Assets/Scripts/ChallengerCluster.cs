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
    }

    public void SetChildSize(int amountMonth, float width)
    {
        float height = Mathf.Ceil((float)amountMonth / 3) * 450;
        child.sizeDelta = new Vector2(child.sizeDelta.x, height);
        parent.sizeDelta = new Vector2(width, height + 50 + 75);
    }

    public void LoadData(DateTime date, DateTime releaseDate, Sprite cup, DataManager dataManager)
    {
        challengers[index].LoadData(date, releaseDate, cup, dataManager);
        index++;
    }
}

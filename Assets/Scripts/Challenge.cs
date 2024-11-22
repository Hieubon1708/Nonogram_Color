using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    public Sprite[] cups;
    public RectTransform canvas;
    public RectTransform container;
    public DataManager dataManager;
    public GameObject home;
    public GameObject achievement;
    public GameObject challengerPre;
    public List<ChallengerCluster> challengerClusters = new List<ChallengerCluster>();
    public DateTime releaseDate = new DateTime(2024, 10, 1);
    public AchiementControlView achiementControlView;

    public void Awake()
    {
        DOVirtual.DelayedCall(0.02f, delegate
        {
            DateTime startDate = releaseDate;
            int amountYear = DateTime.Now.Year - startDate.Year;

            for (int i = 0; i <= amountYear; i++)
            {
                challengerClusters.Add(Instantiate(challengerPre, container).GetComponent<ChallengerCluster>());
            }

            ResetChallenger();
            int currentYear = startDate.Year;
            int indexMonth = 0;
            int indexYear = 0;

            challengerClusters[indexYear].year.text = startDate.Year.ToString();

            int amoutMonth = 0;
            while (startDate.Date <= DateTime.Now.Date)
            {
                challengerClusters[indexYear].LoadData(startDate, releaseDate, cups[indexMonth], dataManager);
                startDate = startDate.AddMonths(1);
                amoutMonth++;
                if (currentYear < startDate.Year)
                {
                    challengerClusters[indexYear].SetChildSize(amoutMonth, canvas.sizeDelta.x);
                    currentYear = startDate.Year;
                    amoutMonth = 0;
                    indexYear++;
                    challengerClusters[indexYear].year.text = startDate.Year.ToString();
                }
                indexMonth++;
            }
            challengerClusters[indexYear].SetChildSize(amoutMonth, canvas.sizeDelta.x);
        });
    }

    public void LoadData()
    {
        achiementControlView.LoadData();

        ResetChallenger();

        DateTime startDate = releaseDate;
        int currentYear = startDate.Year;
        int indexMonth = 0;
        int indexYear = 0;

        while (startDate.Date <= DateTime.Now.Date)
        {
            challengerClusters[indexYear].LoadData(startDate, releaseDate, cups[indexMonth], dataManager);
            startDate = startDate.AddMonths(1);
            if (currentYear < startDate.Year)
            {
                currentYear = startDate.Year;
                indexYear++;
                challengerClusters[indexYear].year.text = startDate.Year.ToString();
            }
            indexMonth++;
        }
    }

    void ResetChallenger()
    {
        for (int i = 0; i < challengerClusters.Count; i++)
        {
            challengerClusters[i].ResetChallegerCluster();
        }
    }

    public void BackHome()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            home.SetActive(true);
            achievement.SetActive(false);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }
}

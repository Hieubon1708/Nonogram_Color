using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UICommon : MonoBehaviour
{
    public GameManager gameManager;
    public Image layerCover;
    public SettingOption[] settingOptions;
    public GameObject panelSetting;

    void Awake()
    {
        LoadData();
    }

    public void LoadData()
    {
        ChangeSettingOption(settingOptions[0], gameManager.IsActiveMusic, 0);
        ChangeSettingOption(settingOptions[1], gameManager.IsAtiveSound, 0);
        ChangeSettingOption(settingOptions[2], gameManager.IsActiveVibrate, 0);
    }

    public void ChangeSettingOption(SettingOption settingOption, bool isActive, float time)
    {
        if (settingOption.type == TypeSetting.Music) gameManager.IsActiveMusic = isActive;
        if (settingOption.type == TypeSetting.Sound) gameManager.IsAtiveSound = isActive;
        if (settingOption.type == TypeSetting.Vibrate) gameManager.IsActiveVibrate = isActive;
        settingOption.SwitchStateHandle(isActive, time);
    }

    public void DOLayerCover(float alpha, float duration, bool isActive, Action callback)
    {
        if(isActive) layerCover.gameObject.SetActive(true);
        layerCover.DOFade(alpha, duration).OnComplete(delegate
        {
            if (!isActive) layerCover.gameObject.SetActive(false);
            if (callback != null) callback.Invoke();
        });
    }

    public void ShowPanelSetting()
    {
        panelSetting.SetActive(true);
    }

    public void HidePanelSetting()
    {
        panelSetting.SetActive(false);
    }

    public void Facebook()
    {

    }
    
    public void Tiktok()
    {

    }
    
    public void Youtube()
    {

    }

    public enum TypeSetting
    {
        None, Sound, Music, Vibrate
    }

    public void OnDestroy()
    {
        layerCover.DOKill();
    }
}

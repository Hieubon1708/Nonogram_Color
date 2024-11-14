using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UICommon : MonoBehaviour
{
    public GameManager gameManager;
    public Image layerCover;
    public SettingOption[] settingOptions;
    public Image panelSetting;
    public RectTransform popupSetting;

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

    public void ScalePopup(Image panel, RectTransform popup, float alpha, float durationAlpha, float scale, float durationScale)
    {
        panel.DOKill();
        popup.DOKill();

        panel.DOFade(alpha, durationAlpha);
        popup.DOScale(scale, durationScale).SetEase(Ease.OutBack);
    }

    public void ShowPanelSetting()
    {
        panelSetting.gameObject.SetActive(true);
        ScalePopup(panelSetting, popupSetting, 234f / 255f, 0.1f, 1f, 0.5f);
    }

    public void HidePanelSetting()
    {
        ScalePopup(panelSetting, popupSetting, 0f, 0f, 0.9f, 0f);
        panelSetting.gameObject.SetActive(false);
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
}

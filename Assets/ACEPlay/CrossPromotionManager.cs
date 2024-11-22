using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Duc
{
    public class CrossPromotionManager : MonoBehaviour
    {
        public static CrossPromotionManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Destroy(this.gameObject);
        }

        public Image sprBannerSetting;
        public VideoPlayer videoPlayer, videoPlayerWin, videoPlayerLose;
        public MeshRenderer meshVideo, meshVideoEndGame;

        private void Start()
        {
            if (videoPlayer != null)
            {
                videoPlayer.transform.parent.gameObject.SetActive(false);
                //if (!videoPlayer.transform.parent.gameObject.activeSelf && Duc.Manager.instance.Level >= 5)
                //    ShowVideoMenu();
            }

            if (videoPlayerWin != null)
            {
                videoPlayerWin.transform.parent.gameObject.SetActive(false);
            }

            if (videoPlayerLose != null)
            {
                videoPlayerLose.transform.parent.gameObject.SetActive(false);
            }
        }

        public void ShowVideoMenu()
        {
            if (videoPlayer.gameObject == null) return;
            if (ACEPlay.CrossPromotion.CrossPromotionManager.instance.EnableCrossPromotion)
            {
                if (ACEPlay.CrossPromotion.CrossPromotionManager.instance.EnableVideoOnStart)
                {
                    string videoPath = ACEPlay.CrossPromotion.CrossPromotionManager.instance.GetVideoURL();
                    if (string.IsNullOrEmpty(videoPath))
                    {
                        Debug.Log("No Video");
                        videoPlayer.transform.parent.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Video");
                        //meshVideo.sortingLayerID = 2;
                        videoPlayer.transform.parent.gameObject.SetActive(true);
                        videoPlayer.url = videoPath;
                        videoPlayer.Play();
                    }
                }
            }
            else
            {
                videoPlayer.transform.parent.gameObject.SetActive(false);
            }
        }

        public void _ShowBannerInSettings()
        {
            if (sprBannerSetting.gameObject.activeInHierarchy) return;

            if (ACEPlay.CrossPromotion.CrossPromotionManager.instance.EnableCrossPromotion)
            {
                if (ACEPlay.CrossPromotion.CrossPromotionManager.instance.EnableBannerOnSetting)
                {
                    ShowBanner(sprBannerSetting);
                }
            }
            else
            {
                sprBannerSetting.transform.gameObject.SetActive(false);
            }
        }

        public void ShowVideoWin()
        {
            if (videoPlayerWin.gameObject == null) return;
            if (videoPlayerWin.gameObject.activeInHierarchy) return;

            if (ACEPlay.CrossPromotion.CrossPromotionManager.instance.EnableCrossPromotion)
            {
                if (ACEPlay.CrossPromotion.CrossPromotionManager.instance.EnableVideoOnEndgame)
                {
                    string videoPath = ACEPlay.CrossPromotion.CrossPromotionManager.instance.GetVideoURL();
                    if (string.IsNullOrEmpty(videoPath))
                    {
                        Debug.Log("No Video");
                        videoPlayerWin.transform.parent.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Video");
                        videoPlayerWin.transform.parent.gameObject.SetActive(true);
                        videoPlayerWin.url = videoPath;
                        videoPlayerWin.Play();
                    }
                }
            }
            else
            {
                videoPlayerWin.transform.parent.gameObject.SetActive(false);
            }
        }

        public void ShowVideoLose()
        {
            if (videoPlayerLose.gameObject == null) return;
            if (videoPlayerLose.gameObject.activeInHierarchy) return;

            if (ACEPlay.CrossPromotion.CrossPromotionManager.instance.EnableCrossPromotion)
            {
                if (ACEPlay.CrossPromotion.CrossPromotionManager.instance.EnableVideoOnEndgame)
                {
                    string videoPath = ACEPlay.CrossPromotion.CrossPromotionManager.instance.GetVideoURL();
                    if (string.IsNullOrEmpty(videoPath))
                    {
                        Debug.Log("No Video");
                        videoPlayerLose.transform.parent.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Video");
                        videoPlayerLose.transform.parent.gameObject.SetActive(true);
                        videoPlayerLose.url = videoPath;
                        videoPlayerLose.Play();
                    }
                }
            }
            else
            {
                videoPlayerWin.transform.parent.gameObject.SetActive(false);
            }
        }

        public void ShowIcon(Image icon)
        {
            Debug.Log("Show Icon");
            Texture2D texture = ACEPlay.CrossPromotion.CrossPromotionManager.instance.appIcon;
            if (texture == null)
            {
                Debug.Log("No Icon");
                icon.transform.parent.gameObject.SetActive(false);
                return;
            }
            Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            icon.sprite = image;
            icon.transform.parent.gameObject.SetActive(true);
        }

        public void ShowBanner(Image banner)
        {
            Debug.Log("Show Banner Cross");
            Texture2D texture = ACEPlay.CrossPromotion.CrossPromotionManager.instance.appBanner;
            if (texture == null)
            {
                Debug.Log("No Banner Cross");
                banner.transform.gameObject.SetActive(false);
                return;
            }
            Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            banner.sprite = image;
            banner.gameObject.SetActive(true);
        }


#if UNITY_ANDOID || UNITY_IOS || UNITY_EDITOR
        public void clickCrossPromotionGame()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            //string url = ACEPlay.CrossPromotion.CrossPromotionManager.instance.GetCurrentUrlStoreCrossAppByVideo();
#elif UNITY_IOS || UNITY_EDITOR
		string url = ACEPlay.CrossPromotion.CrossPromotionManager.instance.GetCurrentUrlStoreCrossAppByVideo();
#endif
            /*if (!url.Equals(""))
            {
                Application.OpenURL(url);
                ACEPlay.Bridge.BridgeController.instance.LogEvent("cross_promotion");
            }*/
        }
#endif
        public void _PressedCloseVideoMain()
        {
            videoPlayer.transform.parent.gameObject.SetActive(false);
        }
        public void _PressedCloseVideoWin()
        {
            videoPlayerWin.transform.parent.gameObject.SetActive(false);
        }

        public void _PressedCloseVideoLose()
        {
            videoPlayerLose.transform.parent.gameObject.SetActive(false);
        }
    }
}
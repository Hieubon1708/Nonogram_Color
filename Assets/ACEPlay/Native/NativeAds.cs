using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duc
{
    public class NativeAds : MonoBehaviour
    {
        public static NativeAds instance;

        [Header("Native ADS")]

        [SerializeField] private GameObject nativeAds;
        [SerializeField] private RawImage mainRawImage;
        [SerializeField] private RawImage iconRawImage;
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtDescription;
        [SerializeField] private TextMeshProUGUI txtButton;

        public float maxWidth = 386, maxHeght = 224;
        bool nativeAdLoaded;
        bool isShowNativeAds;
        RectTransform rectNativeAds;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            rectNativeAds = nativeAds.GetComponent<RectTransform>();
            nativeAds.SetActive(false);
            isShowNativeAds = false;
            if (mainRawImage != null) mainRawImage.texture = null;
            if (iconRawImage != null) iconRawImage.texture = null;
            if (txtTitle != null) txtTitle.text = string.Empty;
            if (txtDescription != null) txtDescription.text = string.Empty;
            if (txtButton != null) txtButton.text = string.Empty;

        }

        public void DisplayNativeAds(bool enable)
        {
            nativeAds.SetActive(enable);
        }

        public void NewPosition(float y)
        {
            rectNativeAds.anchoredPosition = new Vector2(rectNativeAds.anchoredPosition.x, y);
        }
    }
}

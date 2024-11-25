using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CheckSizeCanvas : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;

    void Awake()
    {
        CheckSize();
    }

    public void CheckSize()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }
        canvasScaler.matchWidthOrHeight = Camera.main.aspect < 0.55f ? 0 : 1;
    }
}

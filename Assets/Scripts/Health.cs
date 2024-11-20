using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animation ani;
    public GameObject healthGray;

    public void SubtractHealthAni()
    {
        if (!GameController.instance.isLoadData) ani.Play("HealthSubtract");
        else healthGray.SetActive(true);
    }

    public void Replay()
    {
        ani.Play("ScaleHealth");
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}

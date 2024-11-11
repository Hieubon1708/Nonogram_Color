using DG.Tweening;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Animation ani;

    public void SubtractHealthAni()
    {
        ani.Play("HealthSubtract");
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

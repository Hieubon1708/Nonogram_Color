using DG.Tweening;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Animation ani;

    public void SubtractHealthAni()
    {
        ani.Play("HealthSubtract");
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}

using DG.Tweening;
using UnityEngine;

public class FalseCircle : MonoBehaviour
{
    public Box boxSelected;

    public void SetSize(LevelConfig levelConfig)
    {
        float boxCellSize = GameController.instance.dataManager.sizeConfig[(int)levelConfig.typeLevel].boxCellSize;
        transform.localScale = Vector3.one * (boxCellSize / 72);
    }

    public void EndFalse()
    {
        gameObject.SetActive(false);
        DOVirtual.DelayedCall(0.1f, delegate
        {
            boxSelected.EndFalse();
        });
    }
}

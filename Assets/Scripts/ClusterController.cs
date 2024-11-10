using UnityEngine;
using UnityEngine.UI;

public class ClusterController : MonoBehaviour
{
    public HorizontalLayoutGroup horizontalLayoutGroup;
    public VerticalLayoutGroup verticalLayoutGroup;

    public ColCluster[] colClusters;
    public RowCluster[] rowClusters;

    public void LoadLevel(LevelConfig levelConfig)
    {
        ResetColRowCluster();
        Box[][] boxes = GameController.instance.boxController.boxes;

        if(boxes.Length == 0)
        {
            Debug.LogError("Boxes Lenght = 0");
            return;
        }
        for (int i = 0; i < boxes[0].Length; i++)
        {
            rowClusters[i].LoadLevel(i, boxes);
        }
        for (int i = 0; i < boxes.Length; i++)
        {
            colClusters[i].LoadLevel(i, boxes);
        }
    }

    void ResetColRowCluster()
    {
        for (int i = 0; i < colClusters.Length; i++)
        {
            colClusters[i].ResetColCluster();
        }
        for (int i = 0; i < rowClusters.Length; i++)
        {
            rowClusters[i].ResetRowCluster();
        }
    }
}

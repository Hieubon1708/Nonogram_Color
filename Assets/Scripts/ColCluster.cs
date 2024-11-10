using UnityEngine;
using UnityEngine.UI;

public class ColCluster : MonoBehaviour
{
    public VerticalLayoutGroup verticalLayoutGroup;
    public ClusterIndex[] clusterIndexs;

    public void LoadLevel(int index, Box[][] boxes)
    {
        if (boxes[index].Length == 0)
        {
            Debug.LogError(gameObject.name + " Length = 0");
            return;
        }

        int amountIndex = 0;
        int count = 0;
        int indexCluster = 0;
        string hexPrevious = boxes[0][index].mainHex;
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i][index].mainHex != hexPrevious)
            {
                if (hexPrevious != "#FFFFFF")
                {
                    amountIndex++;
                    clusterIndexs[indexCluster].LoadData(count, hexPrevious);
                }
                indexCluster++;
                count = 0;
            }
            hexPrevious = boxes[i][index].mainHex;
            count++;
            if (i == boxes.Length - 1 && hexPrevious != "#FFFFFF")
            {
                amountIndex++;
                clusterIndexs[indexCluster].LoadData(count, hexPrevious);
            }
        }
        if (amountIndex < GameController.instance.dataManager.sizeConfig[(int)GameController.instance.typeLevel].limitSpaceInCluster)
        {
            for (int i = 0; i < clusterIndexs.Length; i++)
            {
                if (!clusterIndexs[i].gameObject.activeSelf && amountIndex < GameController.instance.dataManager.sizeConfig[(int)GameController.instance.typeLevel].limitSpaceInCluster)
                {
                    clusterIndexs[i].LoadData(0, "#FFFFFF");
                    clusterIndexs[i].transform.SetAsFirstSibling();
                    amountIndex++;
                }
            }
        }
        gameObject.SetActive(true);
    }

    public void ResetColCluster()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < clusterIndexs.Length; i++)
        {
            clusterIndexs[i].ResetClusterIndex();
        }
    }
}

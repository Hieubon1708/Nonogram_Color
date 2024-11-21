using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public RectTransform[] parent;
    public RectTransform[] child;

    public void Awake()
    {
        parent[0].sizeDelta = child[0].sizeDelta;
        parent[1].sizeDelta = child[1].sizeDelta;
    }
}

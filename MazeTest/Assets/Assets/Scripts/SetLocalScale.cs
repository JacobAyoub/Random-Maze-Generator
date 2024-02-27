using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocalScale : MonoBehaviour
{

    public float xScale;
    public float yScale;
    public float zScale;

    void Start()
    {
        transform.localScale = new Vector3(xScale, yScale, zScale);
    }
}

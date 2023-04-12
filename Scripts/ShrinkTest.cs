using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkTest : MonoBehaviour
{
    public float LerpRatio = 5;
    private Vector3 NormalSize;
    void Start()
    {
        NormalSize = this.transform.localScale;    
    }
    void Update()
    {
        if (this.transform.localScale != NormalSize)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, NormalSize, LerpRatio * Time.deltaTime);
        }
    }
}

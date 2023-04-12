using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroller : MonoBehaviour
{
    public float speed;
    public bool scroll;

    void FixedUpdate()
    {
        if(scroll)
        this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y + speed * Time.deltaTime, this.gameObject.transform.localPosition.z);    
    }
}

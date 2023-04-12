using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTheCam : MonoBehaviour
{
    public bool OnlyY;
    void Update()
    {
        if (!OnlyY)
        {
            if (Camera.main == null) return;
            this.transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            if (Camera.main == null) return;
            this.transform.rotation = new Quaternion(this.transform.rotation.x, Camera.main.transform.rotation.y, this.transform.rotation.z, Camera.main.transform.rotation.w);
        }
    }
}

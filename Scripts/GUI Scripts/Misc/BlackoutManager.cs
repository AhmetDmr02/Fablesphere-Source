using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutManager : MonoBehaviour
{
    public float Speed,Stayblack;
    public CanvasGroup Cnv;
    private bool Launched;
    private bool Increased;
    void Update()
    {
        if (!Launched)
        {
            return;
        }
        if (!Increased)
        {
            if (Cnv.alpha < 1)
            {
                Cnv.alpha += Speed * Time.deltaTime;
                if (Cnv.alpha >= 1)
                {
                    if (Stayblack <= 0)
                    {
                        Increased = true;
                    }
                    else
                    {
                        Invoke("ToggleIncreseBar", Stayblack);
                    }
                }
            }
        }
        else
        {
            Cnv.alpha -= Speed * Time.deltaTime;
            if (Cnv.alpha <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void ToggleIncreseBar() {
        Increased = true;
    }
    public void Launch()
    {
        Launched = true;
    }
}

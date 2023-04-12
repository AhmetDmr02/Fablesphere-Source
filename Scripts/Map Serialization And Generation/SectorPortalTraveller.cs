using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorPortalTraveller : MonoBehaviour
{
    public bool preventSpamming = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !preventSpamming)
        {
            preventSpamming = true;
            if (SectorManager.instance.currentSector < ProceduralModuleGenerator.instance.totalSectors)
            {
                LoadingManager.instance.reverseComing = false;
                EffectManager.instance.CreateBlackoutEffect(0.5f, 2, false, 1);
                PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = true;
                PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = PostProcessingManager.instance.gameObject;
                AudioListener.volume = 0;
                Invoke("SectorManagerCallNew", 3f);
            }else
            {
                //End
                LoadingManager.instance.reverseComing = false;
                EffectManager.instance.CreateBlackoutEffect(0.5f, 2, false, 1);
                PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = true;
                PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = PostProcessingManager.instance.gameObject;
                AudioListener.volume = 0;
                Invoke("SectorManagerCallEnd", 3f);
            }
        }
    }
    public void SectorManagerCallNew()
    {
        SectorManager.instance.goOnNextSector();
    }
    public void SectorManagerCallEnd()
    {
        SectorManager.instance.goOnEnding();
    }
}

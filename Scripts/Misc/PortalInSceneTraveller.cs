using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInSceneTraveller : MonoBehaviour
{
    [SerializeField] private Transform _travelLoc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //Teleport
            EffectManager.instance.CreateBlackoutEffect(1f, 1, false, 0.1f);
            PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = true;
            PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = PostProcessingManager.instance.gameObject;
            AudioListener.volume = 0;
            FPSController.instance.gameObject.GetComponent<CharacterController>().enabled = false;
            Invoke("ActivateM",1.1f);
        }
    }
    private void ActivateM()
    {
        FPSController.instance.transform.position = _travelLoc.position;
        AudioListener.volume = LoadingManager.instance.desiredFloatVolume;
        FPSController.instance.gameObject.GetComponent<CharacterController>().enabled = true;
        PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().CutsceneActive = false;
        PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = false;
        PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = null;
    }
}

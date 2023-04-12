using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class LavaSpike : MonoBehaviour
{
    public float MarkerFlipSize,SpikeRiseSpeed;
    public float DelayForSpike = 2;
    public GameObject Marker, WarningSphere, MainBeam,PartEffect;
    public AudioSource MySource;


    public void Start()
    {
        Invoke("HitSpike", DelayForSpike);
    }
    private void Update()
    {
        Vector3 MarkerFlipper = new Vector3(0, 0, Marker.transform.localEulerAngles.z + MarkerFlipSize * Time.deltaTime);
        Marker.transform.localEulerAngles = MarkerFlipper;
        if (RiseSpike)
        {
            if (MainBeam.transform.localPosition.z > -11.8f)
            {
                Vector3 RiseSec = new Vector3(0, 0, MainBeam.transform.localPosition.z - SpikeRiseSpeed * Time.deltaTime);
                MainBeam.transform.localPosition = RiseSec;
            }
            else
            {
                Destroy(this.gameObject, 3f);
            }
        }
    }

    private bool RiseSpike;
    public void HitSpike()
    {
        MainBeam.transform.localPosition = new Vector3(0, 0, 3.7f);
        PartEffect.GetComponent<ParticleSystem>().Play();
        PartEffect.AddComponent<DestroyMe>().DestroyTime = 10;
        WarningSphere.SetActive(false);
        Marker.SetActive(false);
        RiseSpike = true;
        MySource.Play();
        CameraShaker.Instance.ShakeOnce(4, 2, 0, 4);
    }
}

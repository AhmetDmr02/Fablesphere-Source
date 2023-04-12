using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaStartTrigger : MonoBehaviour
{
    public Camera HighLightCamera;
    public U10PS_DissolveOverTime DissolveObject;
    public GameObject BlockObject,MainLavaObject;
    public LavaGolemMain LavaBeh;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (LavaMainCont.instance.isBossDead)
            {
                Destroy(this.gameObject);
                return;
            }
            //Cutscene
            if (DissolveObject != null)
            {

                    DissolveObject.gameObject.SetActive(false);
                    Material[] mats = DissolveObject.GetComponent<MeshRenderer>().materials;
                    mats[0].SetFloat("_Cutoff", 0);

            }
            BlockObject.SetActive(true);
            this.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Manager").GetComponent<EffectManager>().CreateBlackoutEffect(0.5f,5,true,8);
            Invoke("launchScene", 2f);
        }
    }
    private void launchScene()
    {
        LavaMainCont CAC = this.MainLavaObject.GetComponent<LavaMainCont>();
        CAC.StartCoroutine(CAC.LaunchScene());
    }
}

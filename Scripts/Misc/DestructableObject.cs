using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DestructableObject : MonoBehaviour,IMapGetSerializer
{
    [SerializeField][Header("Main Stat")]
    private int HitPoint;
    [SerializeField][Header("Misc Stats")]
    private float CamerShakeScale;
    [SerializeField]
    private float CameraShakeFadeOut;
    [SerializeField]
    private float YAxisBoost;
    [SerializeField]
    private float DestroyTime;
    [SerializeField]
    private ParticleSystem ExplosionEffect;
    [SerializeField]
    private AudioSource[] ExplosionAudio;
    [SerializeField]
    private GameObject[] CloseObjects;

    private bool destroyed;

    public void Explode()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        if (this.GetComponent<BoxCollider>() != null) this.GetComponent<BoxCollider>().enabled = false;
        if (this.GetComponent<MeshCollider>() != null) this.GetComponent<MeshCollider>().enabled = false;
        foreach (GameObject GO in CloseObjects) GO.SetActive(false);
        GameObject ExplosionCreated = Instantiate(ExplosionEffect.gameObject, this.gameObject.transform.position, ExplosionEffect.transform.rotation, this.gameObject.transform);
        ExplosionCreated.gameObject.transform.localPosition = new Vector3(0, YAxisBoost, 0);
        ExplosionCreated.GetComponent<ParticleSystem>().Play();
        CameraShaker.Instance.ShakeOnce(CamerShakeScale, CamerShakeScale, 0, CameraShakeFadeOut);
        foreach (AudioSource AS in ExplosionAudio)
        {
            GameObject GO = Instantiate(AS.gameObject, this.transform.position, this.transform.rotation, this.transform);
            GO.transform.localPosition = new Vector3(0, 0, 0);
            GO.GetComponent<AudioSource>().Play();
        }
        destroyed = true;
        serialize();
    }
    public int GetHitPoint()
    {
        int HitP = HitPoint;
        return HitP;
    }
    public void DecreaseHitPoint(int DecreaseInt)
    {
        HitPoint -= DecreaseInt;
    }

    public void serialize()
    {
        //Serialize When Boss Died
        SerializationInfoClass SIC = HelperOfDmr.UtilitesOfDmr.CreateDefaultSIC(this.gameObject, this);
        SIC.dontDeleteThis = true;
        SIC.serialBoolList.Add(destroyed);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
        Destroy(this.gameObject, DestroyTime);
    }

    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        if (SIC.serialBoolList[0])
        {
            Destroy(this.gameObject);
        }
    }
}

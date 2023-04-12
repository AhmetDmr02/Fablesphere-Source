using UnityEngine;
using HelperOfDmr;
using EZCameraShake;
using System.Collections.Generic;

public class ForgeSystemMain : MonoBehaviour, IMapGetSerializer
{
    public AudioSource hammerHit;
    public ParticleSystem sparks,healEffect;
    public Transform lootSpawnLoc;
    public GameObject originSpawner;
    public List<DataItem> blacksmithItems;
    public bool usedOnce,animStarted;
    bool serialized;


    private void Start()
    {
        foreach (DataItem di in Inventory.instance.DataItems)
        {
            if (di.Category == "Armour" || di.Category == "Helmet" || di.Category == "Weapon")
            {
                blacksmithItems.Add(di);
            }
        }
    }
    public void PlayhammerHit()
    {
        hammerHit.Play();
    }
    public void shakeCamera()
    {
        CameraShaker.Instance.ShakeOnce(3, 2, 0, 0.2f);
    }
    public void PlaySparks()
    {
        sparks.Play();
    }
    public void SpawnLoot()
    {
        usedOnce = true;
        blacksmithItems.Shuffle();
        healEffect.Play();
        originSpawner = Resources.Load<GameObject>("PickUpSpecial");
        GameObject GO = Instantiate(originSpawner,lootSpawnLoc.transform.position, this.gameObject.transform.rotation);
        GO.AddComponent<GeneratedObject>();
        GO.GetComponent<GeneratedObject>()._objInAssets = originSpawner;
        PickableItem PI = GO.GetComponent<PickableItem>();
        PI.Id = blacksmithItems[0].Id;
        PI.Count = 1;
        PI.Icon = blacksmithItems[0].Photo;
        PI.ItemName = blacksmithItems[0].ItemName;
        serializeMe();
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        usedOnce = SIC.serialBoolList[0];
        serialized = true;
    }
    public void StartInvokeToSpawn()
    {
        Invoke("SpawnLoot", 5f);
    }
    public void serializeMe()
    {
        SerializationInfoClass SIC = UtilitesOfDmr.CreateDefaultSIC(this.gameObject, this);
        SIC.dontDeleteThis = true;
        SIC.serialBoolList.Add(usedOnce);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
}

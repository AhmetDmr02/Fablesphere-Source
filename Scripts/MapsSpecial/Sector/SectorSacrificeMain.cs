using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorSacrificeMain : MonoBehaviour, IMapGetSerializer
{
    public bool SacrificeDone;

    public GameObject portalObject;
    private bool serialized;
    public static SectorSacrificeMain instance;
    public DataScene thisScene;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        ProceduralModuleGenerator PMG = ProceduralModuleGenerator.instance;
        bool doWeHaveOnPMG = false;
        foreach (ModuleInformations M in ProceduralModuleGenerator.instance._moduleInformations)
        {
            if (M.dataScene == thisScene)
            {
                doWeHaveOnPMG = true;
                break;
            }
        }
        if (!doWeHaveOnPMG)
        {
            ModuleInformations MI = new ModuleInformations();
            MI.mapIndex = 0;
            MI.dataScene = thisScene;
            MI.SIC_Informations = null;//change;
            PMG._moduleInformations.Add(MI);
            PMG.currentIndex = 0;
        }
        FadingTextCreator.instance.CreateFadeText("You need to sacrifice some of your equipment with you in order to teleport next sector",1,15,null,Color.red);
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
        if (!serialized)
        {
            SacrificeDone = false;
            calculateMe();
        }
    }

    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        SacrificeDone = SIC.serialBoolList[0];
        calculateMe();
    }
    public void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        SerializationInfoClass SIC = HelperOfDmr.UtilitesOfDmr.CreateDefaultSIC(this.gameObject, this);
        SIC.serialBoolList.Add(SacrificeDone);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void calculateMe()
    {
        if (SacrificeDone)
        {
            portalObject.GetComponent<BoxCollider>().enabled = true;
            portalObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            portalObject.GetComponent<BoxCollider>().enabled = false;
            portalObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
}

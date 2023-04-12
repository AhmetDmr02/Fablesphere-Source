using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.XR;
using System.IO;

public class EnteranceModuleScript : MonoBehaviour
{
    [Header("Enterance Module Stuff")]
    public TextMeshProUGUI eachSectorText, sectoreText,totalMixture,PlayerName;
    public DataScene enteranceScene;
    public Sprite banner;
    public TextAsset ta;
    public static EnteranceModuleScript instance;

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
        int sideCount = PMG.generationRules.marketRules.min + PMG.generationRules.labRules.min + PMG.generationRules.blacksmithRules.min;
        int mainHallCount = PMG.generationRules.mainHallRules.min;
        int sectorCount = PMG.totalSectors;
        eachSectorText.text = "Each Sector Will Have:\n" + mainHallCount + " : Minimum Main Rooms\n" + sideCount + " : Minimum Side Rooms";
        sectoreText.text = sectorCount + " Total Sectors";
        totalMixture.text = "TOTAL MERCY COUNT:" + PostProcessingManager.instance.gameObject.GetComponent<BarManager>().maxMercy;
        bool doWeHaveOnPMG = false;
        foreach (ModuleInformations MIx in PMG._moduleInformations)
        {
            if (MIx.dataScene.scenePath == enteranceScene.scenePath)
            {
                doWeHaveOnPMG = true;
                PMG.currentIndex = 0;
                break;
            }
        }
        if (!doWeHaveOnPMG)
        {
            ModuleInformations MI = new ModuleInformations();
            MI.mapIndex = 0;
            MI.dataScene = enteranceScene;
            MI.SIC_Informations = null;//change;
            PMG._moduleInformations.Add(MI);
            PMG.currentIndex = 0;
        }
        if (MainDatabaseManager.instance == null) return;
        if (MainDatabaseManager.instance.closeDATABASE)
        {
            PlayerName.enabled = false;
        }else
        {
            PlayerName.text = "New Challenger Appeared:\n" + MainDatabaseManager.instance.databasePiercer.playerName;
        }
    }
    public void reloadScript()
    {
        ProceduralModuleGenerator PMG = ProceduralModuleGenerator.instance;
        int sideCount = PMG.generationRules.marketRules.min + PMG.generationRules.labRules.min + PMG.generationRules.blacksmithRules.min;
        int mainHallCount = PMG.generationRules.mainHallRules.min;
        int sectorCount = PMG.totalSectors;
        eachSectorText.text = "Each Sector Will Have:\n" + mainHallCount + " : Minimum Main Rooms\n" + sideCount + " : Minimum Side Rooms";
        sectoreText.text = sectorCount + " Total Sectors";
        totalMixture.text = "TOTAL MERCY COUNT:" + PostProcessingManager.instance.gameObject.GetComponent<BarManager>().maxMercy;
    }
}

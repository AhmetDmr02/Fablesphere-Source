using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;

public class ProceduralModuleGenerator : MonoBehaviour,IMapSetSerializer
{
    public static ProceduralModuleGenerator instance;
    [SerializeField] private List<DataScene> _sceneInformations = new List<DataScene>();
    [Space(35)]
    public List<ModuleInformations> _moduleInformations = new List<ModuleInformations>();
    [Header("Generation Rules")]
    public int totalSectors;
    public int totalModulesPerSectors;
    public GenerationRules generationRules;
    public int lastIndex;
    public int currentIndex;
    public static event Action<int> lastIndexFired;
    [SerializeField] private int labGenerated, blacksmithGenerated, marketGenerated, MainHallGenerated;
    private List<SerializationInfoClass> temperClass;
    public ModuleInformations temperCurrent, temperLast;
    #if UNITY_EDITOR

    private void OnValidate()
    {
        if (generationRules.marketRules.min > generationRules.marketRules.max) generationRules.marketRules.min = generationRules.marketRules.max;
        if (generationRules.labRules.min > generationRules.labRules.max) generationRules.labRules.min = generationRules.labRules.max;
        if (generationRules.blacksmithRules.min > generationRules.blacksmithRules.max) generationRules.blacksmithRules.min = generationRules.blacksmithRules.max;
        if (generationRules.mainHallRules.min > generationRules.mainHallRules.max) generationRules.mainHallRules.min = generationRules.mainHallRules.max;
        int maxPossibleModulesPerSector = 0;
        maxPossibleModulesPerSector += generationRules.marketRules.min + generationRules.labRules.min + generationRules.blacksmithRules.min + generationRules.mainHallRules.min;
        if (maxPossibleModulesPerSector > totalModulesPerSectors) totalModulesPerSectors = maxPossibleModulesPerSector;
    }
    #endif

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public IEnumerator delayedGenerateNewModuleStatement()
    {
        int nextIndex = labGenerated + blacksmithGenerated + marketGenerated + MainHallGenerated + 1;
        lastIndexFired?.Invoke(nextIndex);
        yield return new WaitForSecondsRealtime(3f);
        GenerateNewModuleStatement();
        StopCoroutine(delayedGenerateNewModuleStatement());
    }
    public void GenerateNewModuleStatement()
    {
        int nextIndex = labGenerated + blacksmithGenerated + marketGenerated + MainHallGenerated + 1;
        currentIndex = nextIndex;
        if (nextIndex < totalModulesPerSectors)
        {
            int RandomFour = UnityEngine.Random.Range(0, 2);
            Debug.Log("Random Four is : " + RandomFour);
            switch (RandomFour)
            {
                case 0:
                    if (labGenerated >= generationRules.labRules.min)
                    {
                        goto case 1;
                    }
                    else
                    {
                        GenerateNewModule(sceneKind.Lab, nextIndex);
                        labGenerated += 1;
                        break;
                    }
                case 1:
                    if (blacksmithGenerated >= generationRules.blacksmithRules.min)
                    {
                        if (RandomFour == 1)
                        {
                            RandomFour = -1;
                            goto case 0;
                        }else
                        {
                            goto case 2;
                        }
                    }
                    else
                    {
                        GenerateNewModule(sceneKind.Blacksmith, nextIndex);
                        blacksmithGenerated += 1;
                        break;
                    }
                case 2:
                    if (marketGenerated >= generationRules.marketRules.min)
                    {
                        if (RandomFour == 2)
                        {
                            RandomFour = -1;
                            goto case 0;
                        }
                        else
                        {
                            goto case 3;
                        }
                    }
                    else
                    {
                        GenerateNewModule(sceneKind.marketScene, nextIndex);
                        marketGenerated += 1;
                        break;
                    }
                case 3:
                    if (MainHallGenerated >= generationRules.mainHallRules.min)
                    {
                        //Generate Random Module Expect Boss & Max Modules
                        if (RandomFour == 3)
                        {
                            RandomFour = -1;
                            goto case 0;
                        }
                        else
                        {
                            int RandomFourMax = Random.Range(0, 4);
                            Debug.Log("Debug Random Four MAX is : " + RandomFourMax);
                            switch (RandomFourMax)
                            {
                                case 0:
                                    if (labGenerated >= generationRules.labRules.max)
                                    {
                                        goto case 1;
                                    }
                                    else
                                    {
                                        GenerateNewModule(sceneKind.Lab, nextIndex);
                                        labGenerated += 1;
                                        break;
                                    }
                                case 1:
                                    if (blacksmithGenerated >= generationRules.blacksmithRules.max)
                                    {
                                        if (RandomFourMax == 1)
                                        {
                                            RandomFourMax = -1;
                                            goto case 0;
                                        }else
                                        {
                                            goto case 2;
                                        }
                                    }
                                    else
                                    {
                                        GenerateNewModule(sceneKind.Blacksmith, nextIndex);
                                        blacksmithGenerated += 1;
                                        break;
                                    }
                                case 2:
                                    if (marketGenerated >= generationRules.marketRules.max)
                                    {
                                        if (RandomFourMax == 2)
                                        {
                                            RandomFourMax = -1;
                                            goto case 0;
                                        }
                                        else
                                        {
                                            goto case 3;
                                        }
                                    }
                                    else
                                    {
                                        GenerateNewModule(sceneKind.marketScene, nextIndex);
                                        marketGenerated += 1;
                                        break;
                                    }
                                case 3:
                                    if (MainHallGenerated >= generationRules.mainHallRules.max)
                                    {
                                        if (RandomFourMax == 3)
                                        {
                                            RandomFourMax = -1;
                                            goto case 0;
                                        }
                                        else
                                        {
                                            Debug.LogError("Generation Failed But I Still did created new module so player will not experience bug");
                                            GenerateNewModule(sceneKind.MainHall, nextIndex);
                                            MainHallGenerated += 1;
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        GenerateNewModule(sceneKind.MainHall, nextIndex);
                                        MainHallGenerated += 1;
                                        break;
                                    }
                            }
                            break;
                        }
                    }
                    else
                    {
                        GenerateNewModule(sceneKind.MainHall, nextIndex);
                        MainHallGenerated += 1;
                        break;
                    }
            }
        }
        else
        {
            GenerateNewModule(sceneKind.BossScene, nextIndex);
        }
    }   
    public void GenerateNewModule(sceneKind kind,int IndexNUM)
    {
        List<DataScene> scenes = new List<DataScene>();
        foreach (DataScene DS in _sceneInformations)
        {
            if (DS._sceneKind == kind)
            {
                scenes.Add(DS);
            }
        }
        int randomINT = Random.Range(0, scenes.Count);
        ModuleInformations MI = new ModuleInformations();
        MI.mapIndex = IndexNUM;
        MI.dataScene = scenes[randomINT];
        _moduleInformations.Add(MI);
        temperLast = temperCurrent;
        temperCurrent = MI;
        currentIndex = temperCurrent.mapIndex;
        lastIndex = temperLast.mapIndex;
        LoadingManager.instance.setTextByData(scenes[randomINT]);
        //LoadingManager.instance.LoadGivenScene(scenes[randomINT].scene,IndexNUM);
        LoadingManager.instance.StartCoroutine(LoadingManager.instance.LoadGivenScene(scenes[randomINT].scenePath, IndexNUM,true,null,null));
    }
    public IEnumerator delayedLoadModuleFromIndex(int DIndex,int delay)
    {
        lastIndexFired?.Invoke(DIndex);
        yield return new WaitForSecondsRealtime(delay);
        foreach (ModuleInformations MI in _moduleInformations)
        {
            if (MI.mapIndex == DIndex)
            {
                if (MI.SIC_Informations != null)
                {
                    temperClass = new List<SerializationInfoClass>(MI.SIC_Informations);
                }
                temperLast = temperCurrent;
                temperCurrent = MI;
                currentIndex = temperCurrent.mapIndex;
                lastIndex = temperLast.mapIndex;
                LoadingManager.instance.StartCoroutine(LoadingManager.instance.LoadGivenScene(MI.dataScene.scenePath, DIndex, false, temperClass,MI));
                LoadingManager.instance.setTextByData(MI.dataScene);
                if (MI.SIC_Informations != null)
                {
                    foreach (SerializationInfoClass SIC_CLASS in temperClass)
                    {
                        if (!SIC_CLASS.dontDeleteThis)
                        {
                            MI.SIC_Informations.Remove(SIC_CLASS);
                        }
                    }
                }
                break;
            }

        }
        yield return null;
    }
    public void LoadModuleFromIndex(int Index)
    {
        foreach (ModuleInformations MI in _moduleInformations)
        {
            if (MI.mapIndex == Index)
            {
                temperClass = new List<SerializationInfoClass>(MI.SIC_Informations);
                LoadingManager.instance.StartCoroutine(LoadingManager.instance.LoadGivenScene(MI.dataScene.scenePath, Index,false, temperClass, MI));
                LoadingManager.instance.setTextByData(MI.dataScene);
                if (MI.SIC_Informations != null)
                {
                    foreach (SerializationInfoClass SIC_CLASS in MI.SIC_Informations)
                    {
                        if (!SIC_CLASS.dontDeleteThis)
                        {
                            MI.SIC_Informations.Remove(SIC_CLASS);
                        }
                    }
                }
                break;
            }
        }
    }
    public void saveMapByInfoClassAndMapIndex(SerializationInfoClass SIC, int mapIndex)
    {
        foreach (ModuleInformations modules in _moduleInformations)
        {
            if (modules.mapIndex == mapIndex)
            {
                if (modules.SIC_Informations == null)
                {
                    modules.SIC_Informations = new List<SerializationInfoClass>();
                }
                modules.SIC_Informations.Add(SIC);
            }
        }
    }
    public void deleteSICbyIndexAndInstanceID(string instanceID_, int mapIndex)
    {
        foreach (ModuleInformations modules in _moduleInformations)
        {
            if (modules.mapIndex == mapIndex)
            {
                if (modules.SIC_Informations != null)
                {
                    foreach (SerializationInfoClass SIC in modules.SIC_Informations)
                    {
                        if (SIC.instanceID == instanceID_)
                        {
                            modules.SIC_Informations.Remove(SIC);
                            break;
                        }
                    }
                }
            }
        }
    }
    public bool isPMGContainsSIC(SerializationInfoClass SICs, int mapIndex, string instanceID)
    {
        bool found = false;
        foreach (ModuleInformations modules in _moduleInformations)
        {
            if (modules.mapIndex == mapIndex)
            {
                if (modules.SIC_Informations != null)
                {
                    foreach (SerializationInfoClass SIC in modules.SIC_Informations)
                    {
                        if (SIC.instanceID == instanceID)
                        {
                            if (SICs.serialIntList.Count == SIC.serialIntList.Count && SICs.serialBoolList.Count == SIC.serialBoolList.Count && SICs.serialFloatList.Count == SIC.serialFloatList.Count && SICs.serialStringList.Count == SIC.serialStringList.Count)
                            {
                                return true;
                                break;
                                found = true;
                            }
                        }
                    }
                }
            }
        }
        return found;
    }
    public void deleteAllCreatedData()
    {
        labGenerated = 0;
        blacksmithGenerated = 0;
        marketGenerated = 0;
        MainHallGenerated = 0;
        currentIndex = 0;
        lastIndex = 0;
        temperCurrent = null;
        temperLast = null;
        _moduleInformations.Clear();
    }
    public void setVariablesByRoomStats(RandomRooms RR)
    {
        PostProcessingManager.instance.gameObject.GetComponent<BarManager>().maxMercy = RR.mercyCount;
        PostProcessingManager.instance.gameObject.GetComponent<Currency>().recalculateMercy();
       totalSectors = RR.maxSector;
        totalModulesPerSectors = RR.maxRooms;
        generationRules.blacksmithRules.min = RR.minBlacksmith;
        generationRules.marketRules.min = RR.minMarket;
        generationRules.labRules.min = RR.minLab;
        generationRules.mainHallRules.min = RR.minMain;
    }
}

[System.Serializable]
public class ModuleInformations
{
    public int mapIndex;
    public DataScene dataScene;
    public List<SerializationInfoClass> SIC_Informations;
}
[System.Serializable]
public class GenerationRules
{
    public ModuleRules labRules,blacksmithRules,mainHallRules,marketRules;
}   
[System.Serializable]
public class ModuleRules
{
    public int min, max;
}

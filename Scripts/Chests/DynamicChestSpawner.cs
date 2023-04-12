using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicChestSpawner : MonoBehaviour,IMapGetSerializer
{
    public GameObject[] SpawnChestsObject;
    public List<GameObject> SelectedChests;
    public GameObject MainSelectedChest;
    public bool WooddenIncluded, SilverIncluded, GoldIncluded, EpicIncluded, LegendaryIncluded;
    public int WoodenRarity, SilverRarity, GoldRarity, EpicRarity, DeathRarity;
    public int WoodenRarityMax, SilverRarityMax, GoldRarityMax, EpicRarityMax, DeathRarityMax;
    public bool FirstCalculation;
    public GameObject SpawnedObject;
    public Transform MainParent;
    public bool OpenBound;
    public SerializationInfoClass mySIC;
    private GameObject serializedObject;
    private bool serialized;

    public bool Callback;
    private void Update()
    {
        if (FirstCalculation)
        {
            if (SpawnedObject == null && MainSelectedChest != null)
            {
                if (!OpenBound)
                {
                    if (!serialized)
                    {
                        GameObject GO = Instantiate(MainSelectedChest, this.transform.position, this.transform.rotation);
                        serializedObject = MainSelectedChest;
                        SpawnedObject = GO;
                        GO.transform.position = this.transform.position;
                        GO.transform.rotation = this.transform.rotation;
                        GO.transform.SetParent(MainParent);
                        GO.GetComponent<Chest>().Openable = true;
                        GO.AddComponent<GeneratedObject>();
                        GO.GetComponent<GeneratedObject>()._objInAssets = serializedObject;
                        SerilizeMe();
                    }
                }
                else
                {
                    if (!serialized)
                    {
                        GameObject GO = Instantiate(MainSelectedChest, this.transform.position, this.transform.rotation);
                        serializedObject = MainSelectedChest;
                        SpawnedObject = GO;
                        GO.transform.position = this.transform.position;
                        GO.transform.rotation = this.transform.rotation;
                        GO.transform.SetParent(MainParent);
                        GO.GetComponent<Chest>().Openable = false;
                        GO.AddComponent<GeneratedObject>();
                        GO.GetComponent<GeneratedObject>()._objInAssets = serializedObject; 
                        SerilizeMe();
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < SpawnChestsObject.Length; i++)
            {
                if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.WoodChest)
                {
                    WooddenIncluded = true;
                }
                if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.SilverChest)
                {
                    SilverIncluded = true;
                }
                if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.GoldChest)
                {
                    GoldIncluded = true;
                }
                if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.EpicChest)
                {
                    EpicIncluded = true;
                }
                if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.DeathChest)
                {
                    LegendaryIncluded = true;
                }
            }
            if (SelectedChests.Count == 0)
            {
                if (!Callback)
                {
                    Callback = true;
                    DetermineMainChest();
                }
               
            }
        }
    }
    private void DetermineMainChest()
    {
        int Percentage = Random.Range(0, 101);
        bool PercentageClaimed = false;
        if (WoodenRarity != -1)
        {
            if (Percentage >= WoodenRarity && Percentage <= WoodenRarityMax)
            {
                if (!WooddenIncluded)
                {
                    DetermineMainChest();
                    return;
                }
                else
                {
                    PercentageClaimed = true;
                }
                for (int i = 0; i < SpawnChestsObject.Length; i++)
                {
                    if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.WoodChest)
                    {
                        SelectedChests.Add(SpawnChestsObject[i]);
                    }
                }
                if (SelectedChests.Count >= 0)
                {
                    if (SelectedChests.Count == 1)
                    {
                        MainSelectedChest = SelectedChests[0];
                        FirstCalculation = true;
                    }
                    if (SelectedChests.Count > 1)
                    {
                        int RandomIndex = Random.Range(0, SelectedChests.Count);
                        MainSelectedChest = SelectedChests[RandomIndex];
                        FirstCalculation = true;
                    }
                }
            }
        }
        if (SilverRarity != -1)
        {
            if (Percentage > SilverRarity && Percentage <= SilverRarityMax)
            {
                if (!SilverIncluded)
                {
                    DetermineMainChest();
                    return;
                }
                else
                {
                    PercentageClaimed = true;
                }
                for (int i = 0; i < SpawnChestsObject.Length; i++)
                {
                    if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.SilverChest)
                    {
                        SelectedChests.Add(SpawnChestsObject[i]);
                    }
                }
                if (SelectedChests.Count >= 0)
                {
                    if (SelectedChests.Count == 1)
                    {
                        MainSelectedChest = SelectedChests[0];
                        FirstCalculation = true;
                    }
                    if (SelectedChests.Count > 1)
                    {
                        int RandomIndex = Random.Range(0, SelectedChests.Count);
                        MainSelectedChest = SelectedChests[RandomIndex];
                        FirstCalculation = true;
                    }
                }
            }
        }
        if (GoldRarity != -1)
        {
            if (Percentage > GoldRarity && Percentage <= GoldRarityMax)
            {
                if (!GoldIncluded)
                {
                    DetermineMainChest();
                    return;
                }
                else
                {
                    PercentageClaimed = true;
                }
                for (int i = 0; i < SpawnChestsObject.Length; i++)
                {
                    if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.GoldChest)
                    {
                        SelectedChests.Add(SpawnChestsObject[i]);
                    }
                }
                if (SelectedChests.Count >= 0)
                {
                    if (SelectedChests.Count == 1)
                    {
                        MainSelectedChest = SelectedChests[0];
                        FirstCalculation = true;
                    }
                    if (SelectedChests.Count > 1)
                    {
                        int RandomIndex = Random.Range(0, SelectedChests.Count);
                        MainSelectedChest = SelectedChests[RandomIndex];
                        FirstCalculation = true;
                    }
                }
            }
        }
        if (EpicRarity != -1)
        {
            if (Percentage > EpicRarity && Percentage <= EpicRarityMax)
            {
                if (!EpicIncluded)
                {
                    DetermineMainChest();
                    return;
                }
                else
                {
                    PercentageClaimed = true;
                }
                for (int i = 0; i < SpawnChestsObject.Length; i++)
                {
                    if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.EpicChest)
                    {
                        SelectedChests.Add(SpawnChestsObject[i]);
                    }
                }
                if (SelectedChests.Count >= 0)
                {
                    if (SelectedChests.Count == 1)
                    {
                        MainSelectedChest = SelectedChests[0];
                        FirstCalculation = true;
                    }
                    if (SelectedChests.Count > 1)
                    {
                        int RandomIndex = Random.Range(0, SelectedChests.Count);
                        MainSelectedChest = SelectedChests[RandomIndex];
                        FirstCalculation = true;
                    }
                }
            }
        }
        if (DeathRarity != -1)
        {
            if (Percentage > DeathRarity && Percentage <= DeathRarityMax)
            {
                if (!LegendaryIncluded)
                {
                    DetermineMainChest();
                    return;
                }
                else
                {
                    PercentageClaimed = true;
                }
                for (int i = 0; i < SpawnChestsObject.Length; i++)
                {
                    if (SpawnChestsObject[i].GetComponent<Chest>().ChestKind == Chest.QualityEnum.DeathChest)
                    {
                        SelectedChests.Add(SpawnChestsObject[i]);
                    }
                }
                if (SelectedChests.Count >= 0)
                {
                    if (SelectedChests.Count == 1)
                    {
                        MainSelectedChest = SelectedChests[0];
                        FirstCalculation = true;
                    }
                    if (SelectedChests.Count > 1)
                    {
                        int RandomIndex = Random.Range(0, SelectedChests.Count);
                        MainSelectedChest = SelectedChests[RandomIndex];
                        FirstCalculation = true;
                    }
                }
            }
        }
        if (!PercentageClaimed)
        {
            DetermineMainChest();
            return;
        }
    }
    public void OpenChestBound()
    {   
        if (SpawnedObject != null)
        {
            SpawnedObject.GetComponent<Chest>().Openable = true;
        }
    }

    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        //Deserialize
        Debug.Log("Deserialize Hit Me");
        serialized = true;
        if (SpawnedObject != null && SIC.serialBoolList[0])
        {
            Destroy(SpawnedObject);
            Destroy(this.gameObject);
        }
    }
    public void SerilizeMe()
    {
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.dontDeleteThis = true;
        bool Created__ = true;
        SIC.serialBoolList.Add(Created__);
        if (ProceduralModuleGenerator.instance.isPMGContainsSIC(SIC, ProceduralModuleGenerator.instance.currentIndex, this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w))
        {
            ProceduralModuleGenerator.instance.deleteSICbyIndexAndInstanceID(this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w, ProceduralModuleGenerator.instance.currentIndex);
        }
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
}

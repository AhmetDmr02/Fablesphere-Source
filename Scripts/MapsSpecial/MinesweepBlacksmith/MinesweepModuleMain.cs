using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesweepModuleMain : MonoBehaviour, IMapGetSerializer
{
    public static MinesweepModuleMain instance;
    public GameObject[] doors; //0 main door
    public List<int> openDoorIndexes = new List<int>(7);
    private bool serialized;


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
    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
        if (!serialized)
        {
            randomOpenDoors();
        }
    }
    void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        SerializationInfoClass SIC = HelperOfDmr.UtilitesOfDmr.CreateDefaultSIC(this.gameObject, this);
        SIC.serialIntList.Add(openDoorIndexes[0]);
        SIC.serialIntList.Add(openDoorIndexes[1]);
        SIC.serialIntList.Add(openDoorIndexes[2]);
        SIC.serialIntList.Add(openDoorIndexes[3]);
        SIC.serialIntList.Add(openDoorIndexes[4]);
        SIC.serialIntList.Add(openDoorIndexes[5]);
        SIC.serialIntList.Add(openDoorIndexes[6]);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        openDoorIndexes[0] = SIC.serialIntList[0];
        openDoorIndexes[1] = SIC.serialIntList[1];
        openDoorIndexes[2] = SIC.serialIntList[2];
        openDoorIndexes[3] = SIC.serialIntList[3];
        openDoorIndexes[4] = SIC.serialIntList[4];
        openDoorIndexes[5] = SIC.serialIntList[5];
        openDoorIndexes[6] = SIC.serialIntList[6];
        recalculateDoors();
    }
    public void randomOpenDoors()
    {
        openDoorIndexes[1] = HelperOfDmr.UtilitesOfDmr.RandomChanceForPercentageInt(1);
        openDoorIndexes[2] = HelperOfDmr.UtilitesOfDmr.RandomChanceForPercentageInt(1);
        openDoorIndexes[3] = HelperOfDmr.UtilitesOfDmr.RandomChanceForPercentageInt(1);
        openDoorIndexes[4] = HelperOfDmr.UtilitesOfDmr.RandomChanceForPercentageInt(1);
        openDoorIndexes[5] = HelperOfDmr.UtilitesOfDmr.RandomChanceForPercentageInt(1);
        openDoorIndexes[6] = HelperOfDmr.UtilitesOfDmr.RandomChanceForPercentageInt(100);
        recalculateDoors();
    }
    public void recalculateDoors()
    {
        for (int i = 0; i < openDoorIndexes.Count; i++)
        {
            if (openDoorIndexes[i] == 0)
            {
                doors[i].SetActive(true);
            }
            else
            {
                doors[i].SetActive(false);
            }
        }
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
}

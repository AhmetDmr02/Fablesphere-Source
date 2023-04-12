using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorManager : MonoBehaviour
{
    public int currentSector = 0;
    public DataScene sectorScene,LastScene;
    public static SectorManager instance;
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
    public void goOnNextSector()
    {
        currentSector += 1;
        ProceduralModuleGenerator.instance.deleteAllCreatedData();
        LoadingManager.instance.setTextByData(sectorScene);
        LoadingManager.instance.StartCoroutine(LoadingManager.instance.LoadGivenScene(sectorScene.scenePath, 0, true, null, null));
    }
    public void goOnEnding()
    {
        currentSector += 1;
        ProceduralModuleGenerator.instance.deleteAllCreatedData();
        LoadingManager.instance.setTextByData(LastScene);
        LoadingManager.instance.StartCoroutine(LoadingManager.instance.LoadGivenScene(LastScene.scenePath, 0, true, null, null));
    }
}

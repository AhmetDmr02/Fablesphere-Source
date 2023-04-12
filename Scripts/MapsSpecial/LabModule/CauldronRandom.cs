using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronRandom : MonoBehaviour,IMapGetSerializer
{
    public List<string> kindList = new List<string>();
    public string thisKind;
    private bool serialized;
    [SerializeField] private GameObject _goopObj;
    void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
        if (serialized) return;
        thisKind = kindList[Random.Range(0, kindList.Count)];
        switchGoopObj();
    }
    public void switchGoopObj()
    {
        if (thisKind == "Empty")
        {
            _goopObj.SetActive(false);
        }
        else
        {
            _goopObj.SetActive(true);
        }
    }
    void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        serialized = true;
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
        SIC.dontDeleteThis = false;
        SIC.generatedBySomething = false;
        SIC.serialStringList.Add(thisKind);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        thisKind = SIC.serialStringList[0];
        switchGoopObj();
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
}

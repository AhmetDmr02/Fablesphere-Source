using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDefinements : MonoBehaviour,IMapGetSerializer
{
    [SerializeField] private bool _dissolveCostRune;
    [HideInInspector] public bool DoesHaveDissolveRune => _dissolveCostRune;
    [HideInInspector] public bool DoesHaveDissolveItem => _dissolveCostItem;
    [HideInInspector] public int DissolveCostRune => _runeCost;
    [SerializeField] private bool _dissolveCostItem;
    [HideInInspector] public DataItem DissolveCostItem => _item;
    [HideInInspector] public int DissolveCostItemQuantity => _itemCount;
    [ShowWhen("_dissolveCostRune",true)]
    [SerializeField] private int _runeCost;
    [ShowWhen("_dissolveCostItem", true)]
    [SerializeField] private DataItem _item;
    [ShowWhen("_dissolveCostItem", true)]
    [SerializeField] private int _itemCount;

    [ShowWhen("_dissolveCostRune", true)]
    [SerializeField] private bool _consumeRune;
    [ShowWhen("_dissolveCostItem", true)]
    [SerializeField] private bool _consumeItem;
    [HideInInspector] public bool DissolveCostConsumeItem => _consumeItem;
    [HideInInspector] public bool DissolveCostConsumeRune => _consumeRune;
    [HideInInspector] public bool dissolved;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_item != null) _itemCount = (_itemCount == 0) ? _itemCount = 1 : _itemCount = _itemCount;
    }
    #endif

    public void serializeMe()
    {
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.scriptName = this.GetType().Name;
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w; 
        SIC.dontDeleteThis = true;
        bool Created__ = true;
        SIC.serialBoolList.Add(Created__);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }

    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        if (SIC.serialBoolList[0])
        {
            Destroy(this.gameObject);
        }
    }
}

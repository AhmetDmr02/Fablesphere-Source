using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickableItem : MonoBehaviour,IMapGetSerializer
{
    public int Id, Count;
    public Sprite Icon;
    public string ItemName;
    public Transform IconHolderTransform;
    [SerializeField]private Image IconHolder;
    [HideInInspector]
    public TextMeshProUGUI ItemNameText,ItemCountText;
    private GameObject IconObject; //Pull it from resources
    private bool CreatingObject;
    [HideInInspector]
    public bool Claimed;
    public bool serializing;
    private bool comingfromup;
    public GameObject customprefab,warmprefab,iconcreated;
    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += SerializeThis;
        if (IconHolderTransform == null)
        {
            IconHolderTransform = this.gameObject.transform;
        }
    }
    private void Update()
    {
        if (this.Id != 0 && this.IconHolder == null && !this.CreatingObject)
        {
            this.CreatingObject = true;
            CreateIconObject();
        }
        if (RaycastCenter.LookingItem && RaycastCenter.LookingItemObject == this.gameObject)
        {
            IconHolder.gameObject.SetActive(true);
        }
        else if (IconHolder.gameObject.activeInHierarchy == true)
        {
            IconHolder.gameObject.SetActive(false);
        }
        if (this.Claimed && !comingfromup)
        {
            ProceduralModuleGenerator.lastIndexFired -= SerializeThis;
            comingfromup = true;
            SerializeThis(31);
        }
    }
    public void CreateIconObject()
    {
        GameObject WorldSpaceCanvas = PostProcessingManager.instance.gameObject.GetComponent<SpellUIPusher>().worldCanvas.gameObject;
        this.IconObject = Resources.Load<GameObject>("ItemIconHolder");
        GameObject GO = Instantiate(this.IconObject, WorldSpaceCanvas.transform);
        if (this.IconHolderTransform == null)
        {
            this.IconHolderTransform = this.gameObject.transform;
        }
        GO.transform.position = this.IconHolderTransform.position;
        this.IconHolder = GO.GetComponent<Image>();
        this.IconHolder.sprite = this.Icon;
        this.ItemNameText = GO.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        this.ItemCountText = GO.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        this.ItemNameText.text = this.ItemName;
        this.ItemCountText.text = this.Count.ToString();
        iconcreated = GO;
        GO.SetActive(false);
    }
    public void UpdateAllSpritesAndText()
    {
        if (this.IconHolder == null) return;
        if (this.IconHolderTransform == null)
        {
            this.IconHolderTransform = this.gameObject.transform;
        }
        this.IconHolder.gameObject.transform.position = this.IconHolderTransform.position;
        this.ItemNameText.text = this.ItemName;
        this.ItemCountText.text = this.Count.ToString();
    }
    public void DestroyObject()
    {
        Destroy(this.IconHolder.gameObject);
        Destroy(this.gameObject);
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        if (SIC.serialBoolList[0])
        {
            DestroyObject();
        }
        else
        {
            Id = SIC.serialIntList[0];
            Count = SIC.serialIntList[1];
            ItemName = SIC.serialStringList[0];
            Icon = SIC.serialSpriteList[0];
            UpdateAllSpritesAndText();
            Claimed = false;
            if (SIC.serialAssetGameObject.Count == 2)
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
                GameObject G = Instantiate(SIC.serialAssetGameObject[1], this.transform.position, this.transform.rotation);
                G.transform.parent = this.transform;
                G.gameObject.transform.position = this.transform.position;
                customprefab = SIC.serialAssetGameObject[1];
            }
        }
    }
    public void SerializeThis(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= this.SerializeThis; 
        serializing = true;
        if (this.gameObject.GetComponent<GeneratedObject>() == null)
        {
            if (comingfromup)
            {
                SerializationInfoClass SIC = new SerializationInfoClass();
                MonoBehaviour mb = this;
                SIC.scriptName = mb.GetType().Name;
                SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
                SIC.dontDeleteThis = true;
                SIC.generatedBySomething = false;
                bool destroyMe = true;
                SIC.serialBoolList.Add(destroyMe);
                ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
                DestroyObject();
            }
        }
        else
        {
            if (comingfromup)
            {
                DestroyObject();
            }
            else
            {
                SerializationInfoClass SIC = new SerializationInfoClass();
                MonoBehaviour mb = this;
                SIC.scriptName = mb.GetType().Name;
                SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
                SIC.dontDeleteThis = false;
                SIC.generatedBySomething = true;
                bool destroyMe = false;
                SIC.serialBoolList.Add(destroyMe);
                SIC.serialIntList.Add(Id);
                SIC.serialIntList.Add(Count);
                SIC.serialStringList.Add(ItemName);
                SIC.serialVec3Single = this.transform.position;
                SIC.serialQuaternionSingle = this.transform.rotation;
                SIC.serialSpriteList.Add(Icon);
                SIC.serialAssetGameObject.Add(this.gameObject.GetComponent<GeneratedObject>()._objInAssets);
                if (customprefab != null)
                {
                    SIC.serialAssetGameObject.Add(this.customprefab);
                }
                ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
            }
        }
    }
    private void OnDestroy()
    {
        Destroy(iconcreated);
        ProceduralModuleGenerator.lastIndexFired -= SerializeThis;
    }
}

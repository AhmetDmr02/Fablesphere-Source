using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class SacrificeMonolith : MonoBehaviour, IInteractable, IMapGetSerializer
{
    public bool sacrificeDone => SectorSacrificeMain.instance.SacrificeDone;
    public int HowMuchLeft;
    public int HowMuchLeftTemp;

    private bool serialized;
    public Text howmuchLeftText;
    private bool panelActive;
    public GameObject sacrificePanel,contentDraw,templateItems;
    public List<GameObject> createdItems = new List<GameObject>();
    public List<Slot> slots = new List<Slot>();

    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
        sacrificePanel.GetComponent<Canvas>().enabled = false;
        if (!serialized)
        {
            HowMuchLeft = SectorManager.instance.currentSector;
            HowMuchLeftTemp = HowMuchLeft;
        }
    }
    private void Update()
    {
        howmuchLeftText.text = "You can save: " + HowMuchLeftTemp + " More Slots";
    }
    public string GetLookAtDescription()
    {
        if (panelActive) return "";
        if (!sacrificeDone)
        {
            return "[F] To Sacrifice Items";
        }
        else
        {
            return "";
        }
    }

    public Color GetTextColor()
    {
        return Color.cyan;
    }
    public void OnInteract()
    {
        if (sacrificeDone) return;
        if (!ActivatorManager.instance.SomeoneActive)
        {
            ActivatorManager.instance.SomeoneActive = true;
            ActivatorManager.instance.ActiveObject = sacrificePanel.gameObject;
            sacrificePanel.GetComponent<Canvas>().enabled = true;
            for (int i = 0; i < Inventory.instance.AllSlots.Length; i++) 
            {
                Slot s = Inventory.instance.AllSlots[i];
                if (s.Id != 0)
                {
                    GameObject GO = Instantiate(templateItems, contentDraw.transform);
                    SectorTemp ST = GO.GetComponent<SectorTemp>();
                    createdItems.Add(GO);
                    ST.sacBut.SM = this;
                    ST.slotIndex = i;
                    ST.id = s.Id;
                    ST.quantity = s.Quantity;
                    ST.calculate();
                    //test it
                }
            }
        }
    }
    public void QuitPanel()
    {
        if (ActivatorManager.instance.ActiveObject == sacrificePanel.gameObject)
        {
            sacrificePanel.GetComponent<Canvas>().enabled = false;
            panelActive = false;
            ActivatorManager.instance.SomeoneActive = false;
        }
        foreach (GameObject go in createdItems)
        {
            Destroy(go);
        }
        HowMuchLeftTemp = HowMuchLeft;
        createdItems.Clear();
        slots.Clear();
    }
    public void setSacrificeDone()
    {
        SectorSacrificeMain.instance.SacrificeDone = true;
        SectorSacrificeMain.instance.calculateMe();
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        HowMuchLeft = SIC.serialIntList[0];
        HowMuchLeftTemp = HowMuchLeft;
    }
    public void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        SerializationInfoClass SIC = HelperOfDmr.UtilitesOfDmr.CreateDefaultSIC(this.gameObject, this);
        SIC.serialIntList.Add(HowMuchLeft);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
}

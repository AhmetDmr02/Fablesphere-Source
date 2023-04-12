using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeStoneMain : MonoBehaviour,IMapGetSerializer
{
    [Header("Main Side")]
    [SerializeField] private Transform SpawnLocation;
    [SerializeField] private int AddWeaponMin;
    [SerializeField] private int AddWeaponMax;
    [SerializeField] private int AddArmourMin;
    [SerializeField] private int AddArmourMax;
    [SerializeField] private int AddHelmetMin;
    [SerializeField] private int AddHelmetMax;
    [SerializeField] private int AddPotionMin;
    [SerializeField] private int AddPotionMax;
    [Header("VFX Side")]
    [SerializeField] private ParticleSystem BoughtVFX;
    [SerializeField] private ParticleSystem SoldVFX;
    [Header("SFX Side")]
    [SerializeField] private AudioSource SoldSFX;
    [SerializeField] private AudioSource BoughtSFX;
    [SerializeField] private AudioSource Ambience;
    [SerializeField] private AudioClip[] AmbienceClips;
    [Header("Instance Side")]
    [SerializeField] private Canvas MyCanvasObject;
    [SerializeField] private Canvas MyCanvas;
    [SerializeField] private bool DidICreated;
    [SerializeField] private GameObject SellInstances;
    [SerializeField] private GameObject BuyInstances;
    private List<GameObject> SellInstancesCreated = new List<GameObject>();
    [Header("UI Side")]
    [SerializeField] private GameObject SellPanel;
    [SerializeField] private GameObject BuyPanel;
    [SerializeField] private Text runesText;
    [HideInInspector] public Inventory inv;
    ActivatorManager act;
    [HideInInspector] public Currency C;
    [HideInInspector] public UISfxManager USM;
    public TextMeshProUGUI nameItem;
    public TextMeshProUGUI descItem;
    public TextMeshProUGUI statItem;
    private bool CreatingATM;
    private List<GameObject> buyInstances = new List<GameObject>();
    private bool serialized;
    private void Start()
    {
        PlayAmbience();
        ProceduralModuleGenerator.lastIndexFired += SerializeThisObject;
        #region Assigning Stuff
        if (inv == null) inv = PostProcessingManager.instance.gameObject.GetComponent<Inventory>();
        if (act == null) act = PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>();
        if(C == null) C = PostProcessingManager.instance.gameObject.GetComponent<Currency>();
        if (USM == null) USM = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
        #endregion
        if (!DidICreated && !CreatingATM && !serialized)
        {
            CreatingATM = true;
            CreateAllInstances();
        }
    }
    private void Update()
    {
        #region Assigning Stuff
        if (inv == null) inv = PostProcessingManager.instance.gameObject.GetComponent<Inventory>();
        if (act == null) act = PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>();
        if (C == null) C = PostProcessingManager.instance.gameObject.GetComponent<Currency>();
        if (USM == null) USM = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
        #endregion
        if (act.ActiveObject == MyCanvas.gameObject)
        {
            if(runesText != null)
            runesText.text = "You Have: " + C.GetGem() + " Trade Runes";
        }
        if (!DidICreated && !CreatingATM)
        {
            CreatingATM = true;
            CreateAllInstances();
        }

    }
    private void CreateAllInstances()
    {
        List<DataItem> ArmourItems = new List<DataItem>();
        List<DataItem> HelmetItems = new List<DataItem>();
        List<DataItem> SwordItems = new List<DataItem>();
        List<DataItem> PotionItems = new List<DataItem>();
        foreach (DataItem DataI in inv.DataItems)
        {
            if (!DataI.BlockSpawnOnMarket)
            {
                if (DataI.Category == "Armour")
                {
                    ArmourItems.Add(DataI);
                }
                if (DataI.Category == "Helmet")
                {
                    HelmetItems.Add(DataI);
                }
                if (DataI.Category == "Potion")
                {
                    PotionItems.Add(DataI);
                }
                if (DataI.Category == "Weapon")
                {
                    SwordItems.Add(DataI);
                }
            }
        }
        int HowManySwordToAdd = AddWeaponMin;
        int HowManyHelmetToAdd = Random.Range(AddHelmetMin, AddHelmetMax +1);
        int HowManyArmourToAdd = Random.Range(AddArmourMin, AddArmourMax +1);
        int HowManyPotionToAdd = Random.Range(AddPotionMin, AddPotionMax + 1);
        MyCanvas = Instantiate(MyCanvasObject, this.transform);
        MyCanvas.GetComponent<MarketCanvas>().TSM = this;
        SellPanel = MyCanvas.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).gameObject;
        BuyPanel = MyCanvas.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject;
        runesText = MyCanvas.transform.GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<Text>();
        for (int i = 0; i < HowManySwordToAdd; i++)
        {
            GameObject GO = Instantiate(BuyInstances, BuyPanel.transform);
            MarketItemInstances MII = GO.GetComponent<MarketItemInstances>();
            buyInstances.Add(GO);
            int randomIndex = Random.Range(0, SwordItems.Count);
            DataItem SelectedDataItem = SwordItems[randomIndex];
            MII.ConstructCell(SelectedDataItem.Id, SelectedDataItem.Cost, SelectedDataItem.ItemName, SelectedDataItem.MaxStack, SelectedDataItem.Rarity.ToString(), SelectedDataItem.Photo);
            MII.TSM = this;
            MII.USM = USM;
        }
        for (int i = 0; i < HowManyHelmetToAdd; i++)
        {
            GameObject GO = Instantiate(BuyInstances, BuyPanel.transform);
            MarketItemInstances MII = GO.GetComponent<MarketItemInstances>();
            buyInstances.Add(GO);
            int randomIndex = Random.Range(0, HelmetItems.Count);
            DataItem SelectedDataItem = HelmetItems[randomIndex];
            MII.ConstructCell(SelectedDataItem.Id, SelectedDataItem.Cost, SelectedDataItem.ItemName, SelectedDataItem.MaxStack, SelectedDataItem.Rarity.ToString(), SelectedDataItem.Photo);
            MII.TSM = this;
            MII.USM = USM;
        }
        for (int i = 0; i < HowManyArmourToAdd; i++)
        {
            GameObject GO = Instantiate(BuyInstances, BuyPanel.transform);
            MarketItemInstances MII = GO.GetComponent<MarketItemInstances>();
            buyInstances.Add(GO);
            int randomIndex = Random.Range(0, ArmourItems.Count);
            DataItem SelectedDataItem = ArmourItems[randomIndex];
            MII.ConstructCell(SelectedDataItem.Id, SelectedDataItem.Cost, SelectedDataItem.ItemName, SelectedDataItem.MaxStack, SelectedDataItem.Rarity.ToString(), SelectedDataItem.Photo);
            MII.TSM = this;
            MII.USM = USM;
        }
        for (int i = 0; i < HowManyPotionToAdd; i++)
        {
            GameObject GO = Instantiate(BuyInstances, BuyPanel.transform);
            MarketItemInstances MII = GO.GetComponent<MarketItemInstances>();
            buyInstances.Add(GO);
            int randomIndex = Random.Range(0, PotionItems.Count);
            DataItem SelectedDataItem = PotionItems[randomIndex];
            MII.ConstructCell(SelectedDataItem.Id, SelectedDataItem.Cost, SelectedDataItem.ItemName, SelectedDataItem.MaxStack, SelectedDataItem.Rarity.ToString(), SelectedDataItem.Photo);
            MII.TSM = this;
            MII.USM = USM;
        }
        //Adding Stam For Extra
        foreach (DataItem D in PotionItems)
        {
            if (D.Id == 28)
            {
                GameObject GO = Instantiate(BuyInstances, BuyPanel.transform);
                MarketItemInstances MII = GO.GetComponent<MarketItemInstances>();
                buyInstances.Add(GO);
                MII.ConstructCell(28, D.Cost, D.ItemName, D.MaxStack, D.Rarity.ToString(), D.Photo);
                MII.TSM = this;
                MII.USM = USM;
                break;
            }   
        }
        RecalculateSellStuff(); //Construct Selling Cells
        DidICreated = true;
    }
    public void RecalculateSellStuff()
    {
        if (SellPanel == null) CreateAllInstances(); //Kinda error handling
        foreach (GameObject GO in SellInstancesCreated)
        {
            Destroy(GO);
        }     
        SellInstancesCreated.Clear();
        foreach (GameObject GO in inv.Slots)
        {
            Slot S = GO.GetComponent<Slot>();
            foreach (DataItem DI in inv.DataItems)
            {
                if (S.Id == DI.Id)
                {
                    GameObject GOB = Instantiate(SellInstances, SellPanel.transform);
                    MarketItemInstances MII = GOB.GetComponent<MarketItemInstances>();
                    MII.ConstructCell(DI.Id, DI.SellCost, DI.ItemName, S.Quantity, DI.Rarity.ToString(), DI.Photo);
                    MII.TSM = this;
                    MII.USM = USM;
                    SellInstancesCreated.Add(MII.gameObject);
                }
            }
        }
    }
    public void SellRequest(MarketItemInstances MII,bool onlyOneLeft)
    {
        if (onlyOneLeft)
        {
            int itemID = MII.GetID();
            foreach (GameObject GO in inv.Slots)
            {
                if (GO.GetComponent<Slot>().Id == itemID)
                {
                    if (GO.GetComponent<Slot>().Quantity > 1)
                    {
                        GO.GetComponent<Slot>().Quantity -= 1;
                    }
                    else
                    {
                        GO.GetComponent<Slot>().CleanSlot();
                    }
                    C.IncreaseGem(MII.GetPrice());
                    SellInstancesCreated.Remove(MII.gameObject);
                    Destroy(MII.gameObject);
                    PlaySoldEffect();
                    break;
                }
            }
        }
        else
        {
            int itemID = MII.GetID();
            foreach (GameObject GO in inv.Slots)
            {
                if (GO.GetComponent<Slot>().Id == itemID)
                {
                    if (GO.GetComponent<Slot>().Quantity > 1)
                    {
                        GO.GetComponent<Slot>().Quantity -= 1;
                    }
                    else
                    {
                        GO.GetComponent<Slot>().CleanSlot();
                    }
                    MII.DecreaseCount();
                    C.IncreaseGem(MII.GetPrice());
                    PlaySoldEffect();
                    break;
                }
            }
        }
    }
    public void BuyRequest(MarketItemInstances MII, bool onlyOneLeft)
    {
        if (onlyOneLeft)
        {
            int Currency = C.GetGem();
            if (MII.GetPrice() > Currency)
            {
                return;
            }
            else
            {
                inv.CalculateSlotCanHandle(MII.GetID());
                if (inv.AllSlotsCanHandleForLastAddedItem < 1)
                {
                    return;
                }
                else
                {
                    inv.AddItem(MII.GetID(), 1);
                    C.DecreaseGem(MII.GetPrice());
                    Destroy(MII.gameObject);
                    buyInstances.Remove(MII.gameObject);
                    PlayBoughtEffect();
                }
            }
        }
        else
        {
            int Currency = C.GetGem();
            if (MII.GetPrice() > Currency)
            {
                return;
            }
            else
            {
                inv.CalculateSlotCanHandle(MII.GetID());
                if (inv.AllSlotsCanHandleForLastAddedItem < 1)
                {
                    return;
                }
                else
                {
                    inv.AddItem(MII.GetID(), 1);
                    C.DecreaseGem(MII.GetPrice());
                    MII.DecreaseCount();
                    PlayBoughtEffect();
                }
            }
        }
    }
    public void QuitTradePanel()
    {
       if (act.SomeoneActive == MyCanvas.gameObject)
       {
            MyCanvas.enabled = false;
            act.SomeoneActive = false;
       }
    }
    public void SwitchToBuying()
    {
        SellPanel.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
        BuyPanel.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
    }
    public void SwitchToSelling()
    {
        SellPanel.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
        BuyPanel.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
    }
    public void openPanel()
    {
        if (act.SomeoneActive == false)
        {
            act.ActiveObject = MyCanvas.gameObject;
            act.SomeoneActive = true;
            MyCanvas.enabled = true;
        }
    }

    #region Ambience Side
    private void PlayAmbience()
    {
        AudioClip SelectedClip = AmbienceClips[Random.Range(0, AmbienceClips.Length)];
        Ambience.PlayOneShot(SelectedClip);
        Invoke("PlayAmbience", SelectedClip.length);
    }
    #endregion

    #region Effects
    private void PlaySoldEffect()
    {
        AudioSource SoldSFXTemp = Instantiate(SoldSFX, SpawnLocation.position, SpawnLocation.rotation);
        SoldSFXTemp.Play();
        SoldVFX.Play();
    }
    private void PlayBoughtEffect()
    {
        AudioSource BoughtVFXTemp = Instantiate(BoughtSFX, SpawnLocation.position, SpawnLocation.rotation);
        BoughtVFXTemp.Play();
        BoughtVFX.Play();
    }
    #endregion

    #region Serializing
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        //Deserialize
        serialized = true;
        CreatingATM = true;
        if (DidICreated)
        {
            foreach (GameObject GO in buyInstances)
            {
                Destroy(GO);
            }
        }
        buyInstances = new List<GameObject>();
        for (int i = 0; i < SIC.serialSpriteList.Count; i++)
        {
            GameObject GO = Instantiate(BuyInstances, BuyPanel.transform);
            MarketItemInstances MII = GO.GetComponent<MarketItemInstances>();
            buyInstances.Add(GO);
            if (i == 0)
            {
                MII.ConstructCell(SIC.serialIntList[i], SIC.serialIntList[i + 2], SIC.serialStringList[i + 1], SIC.serialIntList[i + 1], SIC.serialStringList[i], SIC.serialSpriteList[i]);
            }
            else
            {
                MII.ConstructCell(SIC.serialIntList[(i * 3)], SIC.serialIntList[(i * 3) + 2], SIC.serialStringList[(i * 2) + 1], SIC.serialIntList[(i * 3) + 1], SIC.serialStringList[(i * 2)], SIC.serialSpriteList[i]);
            }
            MII.TSM = this;
            MII.USM = USM;
        }
        RecalculateSellStuff(); //Construct Selling Cells
        DidICreated = true;
    }
    public void SerializeThisObject(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= SerializeThisObject;
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
        SIC.dontDeleteThis = false;
        foreach (GameObject GO in buyInstances)
        {
            MarketItemInstances MII = GO.GetComponent<MarketItemInstances>();
            SIC.serialIntList.Add(MII.ITEMID);
            SIC.serialIntList.Add(MII.COUNT);
            SIC.serialIntList.Add(MII.PRICE);
            SIC.serialSpriteList.Add(MII.ITEMPIC);
            SIC.serialStringList.Add(MII.RARITY);
            SIC.serialStringList.Add(MII.ITEMNAME);
        }
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= SerializeThisObject;
    }
    #endregion
}

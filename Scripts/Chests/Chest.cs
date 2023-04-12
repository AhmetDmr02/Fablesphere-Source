using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Chest : MonoBehaviour,IMapGetSerializer
{
    [Header("Rarity Things")]
    public Material LegendaryMat;
    public Material EpicMat;
    public Material RareMat;
    public Material CommonMat;
    public AudioSource ChestOpen;
    public AudioSource ChestRaritySound;
    public AudioClip LegendarySound, EpicSound, RareSound, CommonSound;
    public int itemId;
    public GameObject ShinyObject;
    [Header("Main Part")]
    public List<DataItem> LootTable;
    public Animator animEdit;
    public ParticleSystem PS;
    public ParticleSystem OuterparticleSystem, BubbleParticles;
    public bool SameLootTableAsInv;
    public bool Openable;
    public string Rarity = "";
    [HideInInspector]
    public bool Opening;
    [HideInInspector]
    public bool OpenState;
    [HideInInspector]
    public bool OpenStatDone;
    private Inventory inv;
    public enum QualityEnum { WoodChest, SilverChest, GoldChest, EpicChest, DeathChest };
    public QualityEnum ChestKind;
    private bool ContainCommon, ContainRare, ContainEpic, ContainLegendary;
    private bool SetColour;
    private int mySerialCount;
    public List<DataItem> LegendaryItems;
    public List<DataItem> EpicItems;
    public List<DataItem> RareItems;
    public List<DataItem> CommonItems;
    public bool Claimed;
    public bool ItemSett;
    [SerializeField]
    public SlotHolder MySlot;
    public bool serialized;
    void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += SerializeMe;
        inv = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();
        if (!serialized)
        {
            if (SameLootTableAsInv)
            {
                //LootTable = inv.DataItems;
                foreach (DataItem di in inv.DataItems)
                {
                    if (!di.BlockOnSpawnChest)
                    {
                        LootTable.Add(di);
                    }
                }
            }
            for (int i = 0; i < LootTable.Count; i++)
            {
                if (LootTable[i].Rarity == DataItem.RarityRnum.Legendary)
                {
                    ContainLegendary = true;
                }
                if (LootTable[i].Rarity == DataItem.RarityRnum.Epic)
                {
                    ContainEpic = true;
                }
                if (LootTable[i].Rarity == DataItem.RarityRnum.Rare)
                {
                    ContainRare = true;
                }
                if (LootTable[i].Rarity == DataItem.RarityRnum.Common)
                {
                    ContainCommon = true;
                }
            }
            DetermineQuality();
        }
        
    }
    void Update()
    {
        if (Opening && !OpenState && !serialized)
        {
            OpenState = true;
            if (Rarity == "Legendary" && !SetColour)
            {
                SetColour = true;
                ShinyObject.GetComponent<Renderer>().material = LegendaryMat;
                for (int i = 0; i < LootTable.Count; i++)
                {
                    if (LootTable[i].Rarity == DataItem.RarityRnum.Legendary)
                    {
                        LegendaryItems.Add(LootTable[i]);
                    }
                }
                int Randomness = Random.Range(0, LegendaryItems.Count);
                itemId = LegendaryItems[Randomness].Id;
            }
            if (Rarity == "Epic" && !SetColour)
            {
                SetColour = true;
                ShinyObject.GetComponent<Renderer>().material = EpicMat;
                for (int i = 0; i < LootTable.Count; i++)
                {
                    if (LootTable[i].Rarity == DataItem.RarityRnum.Epic)
                    {
                        Debug.Log("Found Epic Item");
                        EpicItems.Add(LootTable[i]);
                    }
                }
                int Randomness = Random.Range(0, EpicItems.Count);
                itemId = EpicItems[Randomness].Id;
            }
            if (Rarity == "Rare" && !SetColour)
            {
                SetColour = true;
                ShinyObject.GetComponent<Renderer>().material = RareMat;
                for (int i = 0; i < LootTable.Count; i++)
                {
                    if (LootTable[i].Rarity == DataItem.RarityRnum.Rare)
                    {
                        RareItems.Add(LootTable[i]);
                    }
                }
                int Randomness = Random.Range(0, RareItems.Count);
                itemId = RareItems[Randomness].Id;
            }
            if (Rarity == "Common" && !SetColour)
            {
                SetColour = true;
                ShinyObject.GetComponent<Renderer>().material = CommonMat;
                for (int i = 0; i < LootTable.Count; i++)
                {
                    if (LootTable[i].Rarity == DataItem.RarityRnum.Common)
                    {
                        CommonItems.Add(LootTable[i]);
                    }
                }
                int Randomness = Random.Range(0, CommonItems.Count);
                itemId = CommonItems[Randomness].Id;
            }
            animEdit.SetBool("Opening", true);
            ChestOpen.Play();
            Invoke("PlayParticleAndSound", 1f);
        }
        if (!MySlot.isFull && !Claimed && OpenStatDone && ItemSett)
        {
            Claimed = true;
            Debug.Log("Claimed");
        }
        if (Claimed)
        {
            ShinyObject.SetActive(false);
            if (OuterparticleSystem != null)
            {
                OuterparticleSystem.gameObject.SetActive(false);
            }
        }
    }
    void PlayParticleAndSound()
    {
        ParticleSystemRenderer PSR = PS.GetComponent<ParticleSystemRenderer>();
        if (Rarity == "Legendary")
        {
            PSR.material = LegendaryMat;
            if (BubbleParticles != null)
                BubbleParticles.startColor = LegendaryMat.color;       
            ChestRaritySound.clip = LegendarySound;
            ChestRaritySound.Play();
            PS.Play();
            OpenStatDone = true;
        }
        if (Rarity == "Epic")
        {
            PSR.material = EpicMat;
            if (BubbleParticles != null)
                BubbleParticles.startColor = EpicMat.color;
            ChestRaritySound.clip = EpicSound;
            ChestRaritySound.Play();
            PS.Play();
            OpenStatDone = true;
        }
        if (Rarity == "Rare")
        {
            PSR.material = RareMat;
            if (BubbleParticles != null)
                BubbleParticles.startColor = RareMat.color;
            ChestRaritySound.clip = RareSound;
            ChestRaritySound.Play();
            PS.Play();
            OpenStatDone = true;
        }
        if (Rarity == "Common")
        {
            PSR.material = CommonMat;
            if (BubbleParticles != null)
                BubbleParticles.startColor = CommonMat.color;
            ChestRaritySound.clip = CommonSound;
            ChestRaritySound.Play();
            PS.Play();
            OpenStatDone = true;
        }
        serialized = true;
    }
    void DetermineQuality()
    {
        int RandomNes = Random.Range(0, 100);
        if (ChestKind == QualityEnum.WoodChest)
        {
            if (RandomNes >= 0 && RandomNes <= 5)
            {
                Rarity = "Legendary";
                if (!ContainLegendary)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 5 && RandomNes <= 10)
            {
                Rarity = "Epic";
                if (!ContainEpic)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 10 && RandomNes <= 25)
            {
                Rarity = "Rare";
                if (!ContainRare)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 25 && RandomNes <= 100)
            {
                Rarity = "Common";
                if (!ContainCommon)
                {
                    DetermineQuality();
                    Debug.Log("No Common On Loot Table");
                }
            }
        }
        if (ChestKind == QualityEnum.SilverChest)
        {
            if (RandomNes >= 0 && RandomNes <= 5)
            {
                Rarity = "Legendary";
                if (!ContainLegendary)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 5 && RandomNes <= 15)
            {
                Rarity = "Epic";
                if (!ContainEpic)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 15 && RandomNes <= 40)
            {
                Rarity = "Rare";
                if (!ContainRare)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 40 && RandomNes <= 100)
            {
                Rarity = "Common";
                if (!ContainCommon)
                {
                    DetermineQuality();
                }
            }
        }
        if (ChestKind == QualityEnum.GoldChest)
        {
            if (RandomNes >= 0 && RandomNes <= 7)
            {
                Rarity = "Legendary";
                if (!ContainLegendary)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 7 && RandomNes <= 20)
            {
                Rarity = "Epic";
                if (!ContainEpic)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 20 && RandomNes <= 50)
            {
                Rarity = "Rare";
                if (!ContainRare)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 50 && RandomNes <= 100)
            {
                Rarity = "Common";
                if (!ContainCommon)
                {
                    DetermineQuality();
                }
            }
        }
        if (ChestKind == QualityEnum.EpicChest)
        {
            if (RandomNes >= 0 && RandomNes <= 25)
            {
                Rarity = "Legendary";
                if (!ContainLegendary)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 25 && RandomNes <= 50)
            {
                Rarity = "Epic";
                if (!ContainEpic)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 50 && RandomNes <= 100)
            {
                Rarity = "Rare";
                if (!ContainRare)
                {
                    DetermineQuality();
                }
            }
        }
        if (ChestKind == QualityEnum.DeathChest)
        {
            if (RandomNes >= 0 && RandomNes <= 50)
            {
                Rarity = "Legendary";
                if (!ContainLegendary)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 50 && RandomNes <= 80)
            {
                Rarity = "Epic";
                if (!ContainEpic)
                {
                    DetermineQuality();
                }
            }
            if (RandomNes > 80 && RandomNes <= 100)
            {
                Rarity = "Rare";
                if (!ContainRare)
                {
                    DetermineQuality();
                }
            }
        }
    }
    public void SetSlotItemAndOpenSlot(Slot chestSlot, ActivatorManager act)
    {
        if (act.ChestPoolActive)
        {
            act.OpenInventory();
            return;
        }
        if (itemId != 0 && !Claimed && !ItemSett)
        {
            Debug.Log("Itemset called");
            for (int i = 0; i < inv.DataItems.Length; i++)
            {
                if (inv.DataItems[i].Id == itemId)
                {
                    chestSlot.isFull = true;
                    chestSlot.Id = inv.DataItems[i].Id;
                    chestSlot.GetComponent<Slot>().Stackable = inv.DataItems[i].Stackable;
                    chestSlot.GetComponent<Slot>().ItemSprite = inv.DataItems[i].Photo;
                    chestSlot.GetComponent<Slot>().MaxStack = inv.DataItems[i].MaxStack;

                    if (chestSlot.GetComponent<Slot>().MaxStack > 5)
                    {
                        chestSlot.GetComponent<Slot>().Quantity = Random.Range(1,inv.DataItems[i].MaxStack / 2);
                        mySerialCount = chestSlot.GetComponent<Slot>().Quantity;
                    }
                    else
                    {
                        chestSlot.GetComponent<Slot>().Quantity = 1;
                        mySerialCount = 1;
                    }
                    chestSlot.GetComponent<Slot>().Category = inv.DataItems[i].Category;
                    chestSlot.GetComponent<Slot>().Cost = inv.DataItems[i].Cost;
                    chestSlot.GetComponent<Slot>().SellCost = inv.DataItems[i].SellCost;
                    chestSlot.GetComponent<Slot>().Rarity = inv.DataItems[i].Rarity.ToString();
                    MySlot.isFull = true;
                    MySlot.Id = chestSlot.Id;
                    MySlot.Stackable = chestSlot.Stackable;
                    MySlot.ItemSprite = chestSlot.ItemSprite;
                    MySlot.MaxStack = chestSlot.MaxStack;
                    MySlot.Quantity = chestSlot.Quantity;
                    MySlot.Category = chestSlot.Category;
                    MySlot.Cost = chestSlot.Cost;
                    MySlot.SellCost = chestSlot.SellCost;
                    MySlot.Rarity = chestSlot.Rarity;
                    ItemSett = true;
                    break;
                }
            }
            act.OpenInventory();
            act.ChestPoolActive = true;
        }
        else if (ItemSett && !Claimed)
        {
            chestSlot.isFull = MySlot.isFull;
            chestSlot.Id = MySlot.Id;
            chestSlot.Stackable = MySlot.Stackable;
            chestSlot.ItemSprite = MySlot.ItemSprite;
            chestSlot.MaxStack = MySlot.MaxStack;
            chestSlot.Quantity = MySlot.Quantity;
            chestSlot.Category = MySlot.Category;
            chestSlot.Cost = MySlot.Cost;
            chestSlot.SellCost = MySlot.SellCost;
            chestSlot.Rarity = MySlot.Rarity;
            act.OpenInventory();
            act.ChestPoolActive = true;
        }
        else if (Claimed)
        {
            chestSlot.CleanSlot();
            act.OpenInventory();
            act.ChestPoolActive = true;
        }
    }

    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = SIC.serialBoolList[3];
        itemId = SIC.serialIntList[0];
        Rarity = SIC.serialStringList[0];
        Opening = SIC.serialBoolList[0];
        OpenState = SIC.serialBoolList[1];
        ItemSett = SIC.serialBoolList[2];
        OpenStatDone = SIC.serialBoolList[4];
        Claimed = SIC.serialBoolList[5];
        Openable = SIC.serialBoolList[6];
        if (OpenState)
        {
            animEdit.SetBool("Opening", true);
            if (Rarity == "Legendary")
            {
                ShinyObject.GetComponent<Renderer>().material = LegendaryMat;
            }
            if (Rarity == "Epic")
            {
                ShinyObject.GetComponent<Renderer>().material = EpicMat;
            }
            if (Rarity == "Rare")
            {
                ShinyObject.GetComponent<Renderer>().material = RareMat;
            }
            if (Rarity == "Common")
            {
                ShinyObject.GetComponent<Renderer>().material = CommonMat;
            }
            if (ItemSett && !Claimed)
            {
                inv = PostProcessingManager.instance.gameObject.GetComponent<Inventory>();
                for (int i = 0; i < inv.DataItems.Length; i++)
                {
                    if (inv.DataItems[i].Id == itemId)
                    {
                        MySlot.isFull = true;
                        MySlot.Id = inv.DataItems[i].Id;
                        MySlot.Stackable = inv.DataItems[i].Stackable;
                        MySlot.ItemSprite = inv.DataItems[i].Photo;
                        MySlot.MaxStack = inv.DataItems[i].MaxStack;
                        MySlot.Quantity = SIC.serialIntList[1];
                        mySerialCount = SIC.serialIntList[1];
                        MySlot.Category = inv.DataItems[i].Category;
                        MySlot.Cost = inv.DataItems[i].Cost;
                        MySlot.SellCost = inv.DataItems[i].SellCost;
                        MySlot.Rarity = inv.DataItems[i].Rarity.ToString();
                        break;
                    }
                }
            }
        }
        if (!Openable && LavaMainCont.instance != null)
        {
            if (!LavaMainCont.instance.bountChests.Contains(this))
            {
                LavaMainCont.instance.bountChests.Add(this);
            }
        }
    }
    public void SerializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= SerializeMe;
        SerializationInfoClass SIC = new SerializationInfoClass();
        if (this.gameObject.GetComponent<GeneratedObject>() != null)
        {
            SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
            SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
            SIC.dontDeleteThis = false;
            SIC.generatedBySomething = true;
            SIC.serialQuaternionSingle = this.transform.rotation;
            SIC.serialVec3Single = this.transform.position;
            SIC.serialIntList.Add(itemId);
            SIC.serialIntList.Add(mySerialCount);
            SIC.serialStringList.Add(Rarity);
            SIC.serialBoolList.Add(Opening);
            SIC.serialBoolList.Add(OpenState);
            SIC.serialBoolList.Add(ItemSett);
            SIC.serialBoolList.Add(serialized);
            SIC.serialBoolList.Add(OpenStatDone);
            SIC.serialBoolList.Add(Claimed);
            SIC.serialBoolList.Add(Openable);
            SIC.serialAssetGameObject.Add(this.gameObject.GetComponent<GeneratedObject>()._objInAssets);
        }
        else
        {
            SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
            SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
            SIC.dontDeleteThis = false;
            SIC.generatedBySomething = false;
            SIC.serialIntList.Add(itemId);
            SIC.serialIntList.Add(mySerialCount);
            SIC.serialStringList.Add(Rarity);
            SIC.serialBoolList.Add(Opening);
            SIC.serialBoolList.Add(OpenState);
            SIC.serialBoolList.Add(ItemSett);
            SIC.serialBoolList.Add(serialized);
            SIC.serialBoolList.Add(OpenStatDone);
            SIC.serialBoolList.Add(Claimed);
            SIC.serialBoolList.Add(Openable);
        }
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= SerializeMe;
    }
    [System.Serializable]
    public class SlotHolder
    {
        public bool isFull;
        public int Id;
        public bool Stackable;
        public Sprite ItemSprite;
        public int MaxStack;
        public int Quantity;
        public string Category;
        public int Cost, SellCost;
        public string Rarity;
    }


}

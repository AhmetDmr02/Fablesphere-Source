using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrigSpecialScript : MonoBehaviour,IMapGetSerializer
{

    [SerializeField] private bool isPortalDestroyed,isDestroyPortalOpen,isDissolveGateOpen,isBuffTaken;

    [SerializeField] private string buffKind = "";
    [SerializeField] private GameObject destroyPortal,dissolveGate,portalSelf;
    [SerializeField] private ParticleSystem PSPortal,PSDissolve;
    private bool serialized;
    public static BrigSpecialScript instance;

    [HideInInspector] public bool IsDestroyPortalOpen => isDestroyPortalOpen;
    [HideInInspector] public bool IsDissolveGateOpen => isDissolveGateOpen;
    [HideInInspector] public bool IsBuffTaken { get => isBuffTaken; set { isBuffTaken = value; } }
    [HideInInspector] public string BuffKind => buffKind;
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
        ProceduralModuleGenerator.lastIndexFired += SerializeMe;
        if (serialized) return;
        Calculate();
    }
    private void Calculate()
    {
        if (!serialized)
        {
            if (buffKind == "")
            {
                int i = Random.Range(0, 101);
                if (i <= 10)
                {
                    buffKind = "Attack Speed";
                }
                if (i <= 30 && i > 10)
                {
                    buffKind = "Defence";
                }
                if (i <= 60 && i > 30)
                {
                    buffKind = "Strength";
                }
                if (i <= 100 && i > 60)
                {
                    isBuffTaken = true;
                }
            }
        }
        if (isDissolveGateOpen)
        {
            dissolveGate.SetActive(false);
        }
        if (IsDestroyPortalOpen)
        {
            destroyPortal.SetActive(true);
            portalSelf.SetActive(false);
        }
    }
    public void addCertainBuff()
    {
        DrinkableStatPotionManager DSPM = PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>();
        switch (buffKind)
        {
            case "Attack Speed":
                DSPM.AddNewStatPotion(38, Random.Range(120, 1800));
                break;
            case "Defence":
                DSPM.AddNewStatPotion(25, Random.Range(120, 1800));
                break;
            case "Strength":
                DSPM.AddNewStatPotion(29, Random.Range(120, 1800));
                break;
        }
    }
    public void OpenGate()
    {
        int random = Random.Range(0, 101);
        if (random <= 60)
        {
            Inventory inv = PostProcessingManager.instance.gameObject.GetComponent<Inventory>();
            foreach (GameObject Gslot in inv.Slots)
            {
                Slot slot = Gslot.GetComponent<Slot>();
                slot.CleanSlot();
                UISfxManager.instance.PlayWearSfx();
            }
        }
        isPortalDestroyed = false;
        isDestroyPortalOpen = false;
        isDissolveGateOpen = true;
        PSDissolve.Play();
        dissolveGate.SetActive(false);
        UISfxManager.instance.PlayRockHitSound();
    }
    public IEnumerator DestroyAltar()
    {
        //destroy items and rotate destroyedaltar section
        yield return new WaitForSecondsRealtime(0.6f);
        FPSController.instance.KnockbackImpact((-FPSController.instance.transform.forward * 2) + (FPSController.instance.transform.up / 2), 700);
        isPortalDestroyed = true;
        isDestroyPortalOpen = true;
        isDissolveGateOpen = false;
        portalSelf.SetActive(false);
        PSPortal.Play();
        destroyPortal.SetActive(true);
        UISfxManager.instance.PlayRockHitSound();
        yield return null;
    }

    #region Serializing Stuff
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= SerializeMe;
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        isPortalDestroyed = SIC.serialBoolList[0];
        isDestroyPortalOpen = SIC.serialBoolList[1];
        isDissolveGateOpen = SIC.serialBoolList[2];
        isBuffTaken = SIC.serialBoolList[3];
        buffKind = SIC.serialStringList[0];
        Calculate();
    }
    public void SerializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= SerializeMe;
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
        SIC.dontDeleteThis = false;
        SIC.generatedBySomething = false;
        SIC.serialBoolList.Add(isPortalDestroyed);
        SIC.serialBoolList.Add(isDestroyPortalOpen);
        SIC.serialBoolList.Add(isDissolveGateOpen);
        SIC.serialBoolList.Add(isBuffTaken);
        SIC.serialStringList.Add(buffKind);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    #endregion
}

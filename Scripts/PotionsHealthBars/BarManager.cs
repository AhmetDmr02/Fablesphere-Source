using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    [Header("Bar Side")]
    public Image HpBar;
    public Image StamBar;
    public Image ManaBar;
    [Header("Stat Side")]
    public float HPMax;
    public float HP;
    public float StamMax;
    public float Stam;
    public float ManaMax;
    public float Mana;
    public float StamFillRate;
    public bool GuardPos,Died;
    public int maxMercy = 1;
    public Animation cameraAnimObj;
    private float HPRate,StamRate,ManaRate;

    void Start()
    {
        HP = HPMax;
        Stam = StamMax;
        Mana = ManaMax;
    }
    void Update()
    {
        if (PostProcessingManager.instance.isPoisoned)
        {
            HpBar.color = Color.green;
            if (HP > HPMax / 2)
            {
                HP = HPMax / 2;
            }
        }
        else
        {
            HpBar.color = Color.white;
        }
        HPRate = HP / HPMax;
        StamRate = Stam / StamMax;
        ManaRate = Mana / ManaMax;
        HpBar.fillAmount = HPRate;
        StamBar.fillAmount = StamRate;
        ManaBar.fillAmount = ManaRate;
        if (HP > HPMax)
        {
            HP = HPMax;
        }
        if (Stam > StamMax)
        {
            Stam = StamMax;
        }
        if (Mana > ManaMax)
        {
            Mana = ManaMax;
        }
        if (Stam != StamMax)
        {
            Stam += StamFillRate * Time.deltaTime;
        }
        if (Stam <= 0)
        {
            Stam = 0;
        }
        if (HP <= 0)
        {
            if(!Died)
            Die();
        }
    }
    public void GiveHealByTime(float HpAmount, float PotionFillRate)
    {
        BarIncreaser BI = gameObject.GetComponent<BarIncreaser>();
        if (BI.GiveHp)
        {
            float targetHp = BI.TargetHp;
            targetHp += HpAmount;
            if (targetHp > HPMax)
            {
                targetHp = HPMax;
            }
            BI.HpRate = PotionFillRate;
            BI.TargetHp = targetHp;
            Debug.Log(targetHp);
            BI.HpFillAmount = HpAmount;
            BI.GiveHp = true;
            return;
        }
        float TargetHp = HP;
        TargetHp += HpAmount;
        if (TargetHp > HPMax)
        {
            TargetHp = HPMax;
        }
        BI.HpRate = PotionFillRate;
        BI.TargetHp = TargetHp;
        Debug.Log(TargetHp);
        BI.HpFillAmount = HpAmount;
        BI.GiveHp = true;
    }
    public void GiveManaByTime(float ManaAmount, float PotionFillRate)
    {
        BarIncreaser BI = gameObject.GetComponent<BarIncreaser>();
        if (BI.GiveMana)
        {
            float targetMana = BI.TargetMana;
            targetMana += ManaAmount;
            if (targetMana > ManaMax)
            {
                targetMana = ManaMax;
            }
            BI.ManaRate = PotionFillRate;
            BI.TargetMana = targetMana;
            Debug.Log(targetMana);
            BI.ManafillAmount = ManaAmount;
            BI.GiveMana = true;
            return;
        }
        float TargetHp = Mana;
        TargetHp += ManaAmount;
        if (TargetHp > ManaMax)
        {
            TargetHp = ManaMax;
        }
        BI.ManaRate = PotionFillRate;
        BI.TargetMana = TargetHp;
        Debug.Log(TargetHp);
        BI.ManafillAmount = ManaAmount;
        BI.GiveMana = true;
    }
    public void GiveStaminaByTime(float StamAmount)
    {
        Stam += StamAmount;
    }
    public void Die()
    {
        Died = true;
        if (maxMercy > 0)
        {
            if (ActivatorManager.instance.SomeoneActive)
            {
                if (ActivatorManager.instance.ActiveObject == ActivatorManager.instance.InventoryCanvas.gameObject)
                {
                    ActivatorManager.instance.SomeoneActive = false;
                    ActivatorManager.instance.ActiveObject = null;
                    ActivatorManager.instance.InventoryCanvas.enabled = false;
                }
            }
            PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = true;
            PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = PostProcessingManager.instance.gameObject;
            maxMercy -= 1;
            this.gameObject.GetComponent<Currency>().recalculateMercy();
            cameraAnimObj.Play();
            cameraAnimObj.transform.parent.GetComponent<CameraLook>().enabled = false;
            Invoke("openCameraLook", 3f);
            EffectManager.instance.CreateBlackoutEffect(0.5f, 3, false, 1);
            List<Slot> notEmptySlots = new List<Slot>();
            foreach (Slot s in Inventory.instance.AllSlots)
            {
                if (s.Id != 0)
                {
                    notEmptySlots.Add(s);
                }
            }
            if (notEmptySlots.Count == 1)
            {
                this.gameObject.GetComponent<GoneManager>().InstantiateGone(notEmptySlots[0].Id, notEmptySlots[0].Quantity);
                notEmptySlots[0].CleanSlot();
                UISfxManager.instance.PlayWearSfx();
            }
            else if (notEmptySlots.Count > 1)
            {
                HelperOfDmr.UtilitesOfDmr.Shuffle(notEmptySlots);
                this.gameObject.GetComponent<GoneManager>().InstantiateGone(notEmptySlots[0].Id, notEmptySlots[0].Quantity);
                notEmptySlots[0].CleanSlot();
                UISfxManager.instance.PlayWearSfx();
            }
            this.gameObject.GetComponent<StatData>().CheckStatsAgainBecauseOfPotions();
            //Reload Recent Portal And Delete Random Not Empty Slot
        }
        else
        {
            //Load Losing Screen
            if (ActivatorManager.instance.SomeoneActive)
            {
                if (ActivatorManager.instance.ActiveObject == ActivatorManager.instance.InventoryCanvas.gameObject)
                {
                    ActivatorManager.instance.SomeoneActive = false;
                    ActivatorManager.instance.ActiveObject = null;
                    ActivatorManager.instance.InventoryCanvas.enabled = false;
                }
            }
            PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = true;
            PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = PostProcessingManager.instance.gameObject;
            cameraAnimObj.Play();
            cameraAnimObj.transform.parent.GetComponent<CameraLook>().enabled = false;
            Invoke("loadLosingSide", 3f);
            EffectManager.instance.CreateBlackoutEffect(0.5f, 3, false, 1);
        }
    }
    public void openCameraLook()
    {
        cameraAnimObj.transform.parent.GetComponent<CameraLook>().enabled = true;
        //Load Scene;
        LoadingManager.instance.reverseComing = true;
        if (ProceduralModuleGenerator.instance.currentIndex != 0)
        {
            ProceduralModuleGenerator.instance.StartCoroutine(ProceduralModuleGenerator.instance.delayedLoadModuleFromIndex(ProceduralModuleGenerator.instance.currentIndex - 1, 0));
        }
        else
        {
            ProceduralModuleGenerator.instance.StartCoroutine(ProceduralModuleGenerator.instance.delayedLoadModuleFromIndex(0, 0));
        }
        HP = HPMax;
        Died = false;
    }
    public void loadLosingSide()
    {
        Application.LoadLevel("Scenes/Losing");
    }
}

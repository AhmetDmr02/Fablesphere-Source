using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkableStatPotionManager : MonoBehaviour
{
    public GameObject MainObject,MainScriptObject, ParentObject;
    public DataScriptPotion[] ScriptPotionBehaviours;
    public List<GameObject> PotionBufferContainer = new List<GameObject>();
    public void AddNewStatPotion(int ItemId, float Duration)
    {
        if (PotionBufferContainer.Count >= 1)
        {
            bool Found = false;
            for (int i = 0; i < PotionBufferContainer.Count;i++) {
                if (PotionBufferContainer[i].GetComponent<ScriptPotion>() == null)
                {
                    if (PotionBufferContainer[i].GetComponent<StatPotion>().Id == ItemId)
                    {
                        PotionBufferContainer[i].GetComponent<StatPotion>().Timee = Duration;
                        Found = true;
                        SlotStatCalculator SSC = gameObject.GetComponent<SlotStatCalculator>();
                        SSC.ReadStats();
                        break;
                    }
                }
            }
                
            if (!Found)
            {
                GameObject go = Instantiate(MainObject, ParentObject.transform);
                StatPotion SP = go.GetComponent<StatPotion>();
                Image img = go.gameObject.transform.GetChild(0).GetComponent<Image>();//FindImg;
                Text NameText = go.gameObject.transform.GetChild(1).GetComponent<Text>();//FindName
                Inventory inv = gameObject.GetComponent<Inventory>();
                StatData statReader = gameObject.GetComponent<StatData>();
                for (int i = 0; i < inv.DataItems.Length; i++)
                {
                    //Setting Sprites And Names
                    if (inv.DataItems[i].Id == ItemId)
                    {
                        img.sprite = inv.DataItems[i].Photo;
                        NameText.text = inv.DataItems[i].ItemName;
                        break;
                    }
                }
                int[] WeaponStats = statReader.GetPureStatArrayFromID(ItemId, "Armour");
                SP.Str = WeaponStats[0];
                SP.PhysicalDef = WeaponStats[1];
                SP.MagicalDef = WeaponStats[2];
                SP.Speed = WeaponStats[3];
                SP.Timee = Duration;
                SP.Id = ItemId;
                SP.started = true;
                SlotStatCalculator SSC = gameObject.GetComponent<SlotStatCalculator>();
                SSC.ReadStats();
            }
        }
        else
        {
            GameObject go = Instantiate(MainObject, ParentObject.transform);
            StatPotion SP = go.GetComponent<StatPotion>();
            Image img = go.gameObject.transform.GetChild(0).GetComponent<Image>();//FindImg;
            Text NameText = go.gameObject.transform.GetChild(1).GetComponent<Text>();//FindName
            Inventory inv = gameObject.GetComponent<Inventory>();
            StatData statReader = gameObject.GetComponent<StatData>();
            for (int i = 0; i < inv.DataItems.Length; i++)
            {
                //Setting Sprites And Names
                if (inv.DataItems[i].Id == ItemId)
                {
                    img.sprite = inv.DataItems[i].Photo;
                    NameText.text = inv.DataItems[i].ItemName;
                    break;
                }
            }
            int[] WeaponStats = statReader.GetPureStatArrayFromID(ItemId, "Armour");
            SP.Str = WeaponStats[0];
            SP.PhysicalDef = WeaponStats[1];
            SP.MagicalDef = WeaponStats[2];
            SP.Speed = WeaponStats[3];
            SP.Timee = Duration;
            SP.Id = ItemId;
            SP.started = true;
            SlotStatCalculator SSC = gameObject.GetComponent<SlotStatCalculator>();
            SSC.ReadStats();
        }
    }
    public void AddNewScriptPotion(int ItemId, float Duration)
    {
        Inventory inv = gameObject.GetComponent<Inventory>();
        string selectedBehName = "";
        if (PotionBufferContainer.Count >= 1)
        {
            bool Found = false;
            for (int i = 0; i < PotionBufferContainer.Count; i++)
            {
                if (PotionBufferContainer[i].GetComponent<StatPotion>() == null)
                {
                    if (PotionBufferContainer[i].GetComponent<ScriptPotion>().Id == ItemId)
                    {
                        PotionBufferContainer[i].GetComponent<ScriptPotion>().Timee = Duration;
                        Found = true;
                        break;
                    }
                }
            }
            if (!Found)
            {
                //foundproblemonhere
                foreach (DataScriptPotion DSP in ScriptPotionBehaviours)
                {
                    if (DSP.mainItem.Id == ItemId)
                    {
                        foreach (DataItem DI in inv.DataItems)
                        {
                            if (DI.Id == ItemId)
                            {
                                selectedBehName = DSP.assignedScriptName;
                            }
                        }
                    }
                }
                GameObject go = Instantiate(MainScriptObject, ParentObject.transform);
                go.AddComponent(System.Type.GetType(selectedBehName));
                ScriptPotion SD = go.GetComponent<ScriptPotion>();
                Image img = go.gameObject.transform.GetChild(0).GetComponent<Image>();//FindImg;
                Text NameText = go.gameObject.transform.GetChild(1).GetComponent<Text>();//FindName
                for (int i = 0; i < inv.DataItems.Length; i++)
                {
                    //Setting Sprites And Names
                    if (inv.DataItems[i].Id == ItemId)
                    {
                        img.sprite = inv.DataItems[i].Photo;
                        NameText.text = inv.DataItems[i].ItemName;
                        break;
                    }
                }
                SD.Timee = Duration;
                SD.Id = ItemId;
                SD.started = true;
            }
        }
        else
        {
            foreach (DataScriptPotion DSP in ScriptPotionBehaviours)
            {
                if (DSP.mainItem.Id == ItemId)
                {
                    foreach (DataItem DI in inv.DataItems)
                    {
                        if (DI.Id == ItemId)
                        {
                            selectedBehName = DSP.assignedScriptName;
                        }
                    }
                }
            }
            GameObject go = Instantiate(MainScriptObject, ParentObject.transform);
            go.AddComponent(System.Type.GetType(selectedBehName));
            ScriptPotion SP = go.GetComponent<ScriptPotion>();
            Image img = go.gameObject.transform.GetChild(0).GetComponent<Image>();//FindImg;
            Text NameText = go.gameObject.transform.GetChild(1).GetComponent<Text>();//FindName
            for (int i = 0; i < inv.DataItems.Length; i++)
            {
                //Setting Sprites And Names
                if (inv.DataItems[i].Id == ItemId)
                {
                    img.sprite = inv.DataItems[i].Photo;
                    NameText.text = inv.DataItems[i].ItemName;
                    break;
                }
            }
            SP.Timee = Duration;
            SP.Id = ItemId;
            SP.started = true;
        }
    }
}

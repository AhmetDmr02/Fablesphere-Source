using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatData : MonoBehaviour
{
    private int MagicRes;
    private int PhysicalDef;
    private int Str;
    private float Speed;
    public DataArmour[] Armors;
    public DataWeapons[] Weapons;
    public SlotStatCalculator SSC;
    public string GetStats(int InventoryId,string Category)
    {
        if (Category == "Armour" || Category == "Helmet")
        {
            for (int i = 0; i < Armors.Length; i++)
            {
                if (InventoryId == Armors[i].Item.Id)
                {
                   string StatString = "Str:" + Armors[i].Str + " / Physical Defence:" + Armors[i].PhysicalDef + " / Magical Defence:" + Armors[i].MagicalDef;
                   return StatString;
                   //Make Colors And Helmets
                }
            }
        }
        if (Category == "Weapon")
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (InventoryId == Weapons[i].Item.Id)
                {
                    string StatString = "Str:" + Weapons[i].Str + " / Speed:" + Weapons[i].Speed + " / Magical Defence:" + Weapons[i].MagicResistance;
                    return StatString;
                }
            }
        }
        return null;
    }
    public Color GetColor(int InventoryId, string Category)
    {
        if (Category == "Armour" || Category == "Helmet")
        {
            for (int i = 0; i < Armors.Length; i++)
            {
                if (InventoryId == Armors[i].Item.Id)
                {
                    if (Armors[i].Item.Rarity == DataItem.RarityRnum.Legendary)
                    {
                        Color ReturnColor = Color.yellow;
                        return ReturnColor;
                    }
                    if (Armors[i].Item.Rarity == DataItem.RarityRnum.Epic)
                    {
                        Color ReturnColor = Color.magenta;
                        return ReturnColor;
                    }
                    if (Armors[i].Item.Rarity == DataItem.RarityRnum.Rare)
                    {
                        Color ReturnColor = Color.blue;
                        return ReturnColor;
                    }
                    if (Armors[i].Item.Rarity == DataItem.RarityRnum.Common)
                    {
                        Color ReturnColor = Color.gray;
                        return ReturnColor;
                    }
                    //Make Colors And Helmets
                }
            }
        }
        if (Category == "Weapon")
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (InventoryId == Weapons[i].Item.Id)
                {
                    if (Weapons[i].Item.Rarity == DataItem.RarityRnum.Legendary)
                    {
                        Color ReturnColor = Color.yellow;
                        return ReturnColor;
                    }
                    if (Weapons[i].Item.Rarity == DataItem.RarityRnum.Epic)
                    {
                        Color ReturnColor = Color.magenta;
                        return ReturnColor;
                    }
                    if (Weapons[i].Item.Rarity == DataItem.RarityRnum.Rare)
                    {
                        Color ReturnColor = Color.blue;
                        return ReturnColor;
                    }
                    if (Weapons[i].Item.Rarity == DataItem.RarityRnum.Common)
                    {
                        Color ReturnColor = Color.gray;
                        return ReturnColor;
                    }
                }
            }
        }
        return Color.black;
    }
    public string GetOverAllStatsFromSlots(Slot[] slots)
    {
        Debug.Log("getoverallstatscalledthistime");
        MagicRes = 0;
        PhysicalDef = 0;
        Str = 0;
        Speed = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Category == "Helmet" || slots[i].Category == "Armour")
            {
                for (int x = 0; x < Armors.Length; x++)
                {
                    if (slots[i].Id == Armors[x].Item.Id)
                    {
                        Str += Armors[x].Str;
                        PhysicalDef += Armors[x].PhysicalDef;
                        MagicRes += Armors[x].MagicalDef;
                    }
                }
            }
            if (slots[i].Category == "Weapon")
            {
                for (int x = 0; x < Weapons.Length; x++)
                {
                    if (slots[i].Id == Weapons[x].Item.Id)
                    {
                        Str += Weapons[x].Str;
                        MagicRes += Weapons[x].MagicResistance;
                        Speed += Weapons[x].Speed;
                    }
                }
            }
        }
        List<GameObject> PotionStats = PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>().PotionBufferContainer;
        if (PotionStats.Count >= 1)
        {
            foreach (GameObject Stats in PotionStats)
            {
                if (Stats.GetComponent<ScriptPotion>() == null)
                {
                    StatPotion SP = Stats.GetComponent<StatPotion>();
                    Str += SP.Str;
                    MagicRes += SP.MagicalDef;
                    PhysicalDef += SP.PhysicalDef;
                    Speed += SP.Speed;
                }
            }
        }
        string SendString = "Strength:" + Str + "\n" + "Melee Speed:" + Speed + "\n" + "Physical Defence:" + PhysicalDef + "\n" + "Magical Resistance:" + MagicRes;
        return SendString;
    }
    public float[] GetPureStatArrayFromSlots(Slot[] slots)
    {
        float[] Array = {0 , 0, 0 ,0};
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Category == "Helmet" || slots[i].Category == "Armour")
            {
                for (int x = 0; x < Armors.Length; x++)
                {
                    if (slots[i].Id == Armors[x].Item.Id)
                    {
                        Array[0] += Armors[x].Str;
                        Array[1] += Armors[x].PhysicalDef;
                        Array[2] += Armors[x].MagicalDef;
                    }
                }
            }
            if (slots[i].Category == "Weapon")
            {
                for (int x = 0; x < Weapons.Length; x++)
                {
                    if (slots[i].Id == Weapons[x].Item.Id)
                    {
                        Array[0] += Weapons[x].Str;
                        Array[2] += Weapons[x].MagicResistance;
                        Array[3] += Weapons[x].Speed;
                    }
                }
            }
        }
        List<GameObject> PotionStats = PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>().PotionBufferContainer;
        if (PotionStats.Count >= 1)
        {
            foreach (GameObject Stats in PotionStats)
            {
                if (Stats.GetComponent<ScriptPotion>() == null)
                {
                    StatPotion SP = Stats.GetComponent<StatPotion>();
                    Array[0] += SP.Str;
                    Array[2] += SP.MagicalDef;
                    Array[1] += SP.PhysicalDef;
                    Array[3] += SP.Speed;
                }
            }
        }
        return Array;
    }
    public int[] GetPureStatArrayFromID(int InventoryId,string Category)
    {
        if (Category == "Armour" || Category == "Helmet")
        {
            for (int i = 0; i < Armors.Length; i++)
            {
                if (InventoryId == Armors[i].Item.Id)
                {
                    int[] ArrayedAnswer = { 1, 2, 3,4 };
                    ArrayedAnswer[0] = Armors[i].Str;
                    ArrayedAnswer[1] = Armors[i].PhysicalDef;
                    ArrayedAnswer[2] = Armors[i].MagicalDef;
                    ArrayedAnswer[3] = Armors[i].Speed;
                    return ArrayedAnswer;
                }
            }
        }
        if (Category == "Weapon")
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (InventoryId == Weapons[i].Item.Id)
                {
                    int[] ArrayedAnswer = { 1, 2, 3 };
                    ArrayedAnswer[0] = Weapons[i].Str;
                    ArrayedAnswer[1] = Weapons[i].Speed;
                    ArrayedAnswer[2] = Weapons[i].MagicResistance;
                    return ArrayedAnswer;
                }
            }
        }
        return null;
    }
    public void CheckStatsAgainBecauseOfPotions()
    {
        Debug.Log("Stat Calculated Successfuly");
        SlotStatCalculator SSC = gameObject.GetComponent<SlotStatCalculator>();
        SSC.ReadStats();
    }
    public float GetHitDamages(float PureDmg,float MagicalDmg,float PhysicalDmg)
    {
        SSC = gameObject.GetComponent<SlotStatCalculator>();
        float[] arrayedAnswer = GetPureStatArrayFromSlots(SSC.StatSlots);
        float percentAge = (arrayedAnswer[1] / 1000) * 100; //PhysicalPercent
        float percentAgeMagic = (arrayedAnswer[2] / 1000) * 100; //MagicalPercent
        float DecreaseNewMagicalDmg = (MagicalDmg * percentAgeMagic) / 100;
        float DecreaseNewPhysicalDmg = (PhysicalDmg * percentAge) / 100;
        float NewMagicalDmg = MagicalDmg - DecreaseNewMagicalDmg;
        float NewPhysicalDmg = PhysicalDmg - DecreaseNewPhysicalDmg;
        float TotalDmg = PureDmg + NewPhysicalDmg + NewMagicalDmg;
        Debug.Log("Total Dmg:" + TotalDmg + " New Magical Dmg = (Pure = " + MagicalDmg + ")" + " New Magical Dmg = (Percent%" + percentAgeMagic + " New =" + NewMagicalDmg + ")");
        return TotalDmg;
    }
    public float CalculatePlayerHit(float EnemyDef,bool Critical)
    {
        if (!Critical)
        {
            SSC = gameObject.GetComponent<SlotStatCalculator>();
            float[] Stats = GetPureStatArrayFromSlots(SSC.StatSlots);
            float Strength_Pure = Stats[0];
            Strength_Pure += Random.Range(-Strength_Pure/4, Strength_Pure/6 );
            float HitPercentage = (EnemyDef / 100) * 100;
            float DecreaseSize = (Strength_Pure * HitPercentage) / 100;
            float FinalDmg = Strength_Pure - DecreaseSize;
            return FinalDmg;
        }
        else
        {
            SSC = gameObject.GetComponent<SlotStatCalculator>();
            float[] Stats = GetPureStatArrayFromSlots(SSC.StatSlots);
            float Strength_Pure = Stats[0];
            Strength_Pure += Random.Range(Strength_Pure / 4,Strength_Pure / 2);
            float HitPercentage = (EnemyDef / 100) * 100;
            float DecreaseSize = (Strength_Pure * HitPercentage) / 100;
            float FinalDmg = Strength_Pure - DecreaseSize;
            return FinalDmg;
        }
    }
}

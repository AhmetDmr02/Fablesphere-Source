using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseGunBehaivour : MonoBehaviour
{
    public SwordIdHold[] Swords;
    public BarManager bm;
    public Slot WeaponSlot;
    Inventory inv;
    private int privId;
    private bool diedOnce,diedCalcualted;
    void Start()
    {
        if (!WeaponSlot.isFull)
        {
            privId = 0;
        }
        inv = gameObject.GetComponent<Inventory>();
    }

    void Update()
    {
        if (bm.Died && !diedCalcualted)
        {
            foreach (SwordIdHold SIH in Swords)
            {
                SIH.gameObject.SetActive(false);
            }
            diedCalcualted = true;
            diedOnce = true;
            privId = -2654654; //Random Number
        }
        if (!bm.Died && diedOnce)
        {
            diedCalcualted = false;
        }
        if (bm.Died) return;
        if (WeaponSlot.Id != privId)
        {
            privId = WeaponSlot.Id;
            if (WeaponSlot.isFull)
            {
                for (int x = 0; x < inv.DataItems.Length; x++)
                {
                    if (inv.DataItems[x].Id == WeaponSlot.Id)
                    {
                        for (int i = 0; i < Swords.Length; i++)
                        {
                            if (Swords[i].MainItem.Id == WeaponSlot.Id)
                            {
                                Swords[i].gameObject.SetActive(true);
                            }
                            else
                            {
                                Swords[i].gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (SwordIdHold SIH in Swords)
                {
                    SIH.gameObject.SetActive(false);
                }
            }
        }
    }
}

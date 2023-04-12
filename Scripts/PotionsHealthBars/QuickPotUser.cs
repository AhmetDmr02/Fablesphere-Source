using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickPotUser : MonoBehaviour
{
    Inventory inv;
    GameObject[] CheckSlots;

    private void Start()
    {
        inv = gameObject.GetComponent<Inventory>();
        CheckSlots = inv.Slots;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < CheckSlots.Length; i++)
            {
                Slot slot = CheckSlots[i].GetComponent<Slot>();
                if (slot.Id == 26) //Health Potion Id
                {
                    UISfxManager USFX = gameObject.GetComponent<UISfxManager>();
                    BarManager Bar = gameObject.GetComponent<BarManager>();
                    GoneManager GM = gameObject.GetComponent<GoneManager>();
                    if (slot.Quantity > 1)
                    {
                        slot.Quantity -= 1;
                        USFX.PlayDrinkSfx();
                        Bar.GiveHealByTime(60, 6);
                        GM.InstantiateGone(26, 1);
                        break;
                    }
                    else if(slot.Quantity == 1)
                    {
                        slot.CleanSlot();
                        USFX.PlayDrinkSfx();
                        Bar.GiveHealByTime(60, 6);
                        GM.InstantiateGone(26, 1);
                        break;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < CheckSlots.Length; i++)
            {
                Slot slot = CheckSlots[i].GetComponent<Slot>();
                if (slot.Id == 27) //Mana Potion Id
                {
                    UISfxManager USFX = gameObject.GetComponent<UISfxManager>();
                    BarManager Bar = gameObject.GetComponent<BarManager>();
                    GoneManager GM = gameObject.GetComponent<GoneManager>();
                    if (slot.Quantity > 1)
                    {
                        slot.Quantity -= 1;
                        USFX.PlayDrinkSfx();
                        Bar.GiveManaByTime(100, 8);
                        GM.InstantiateGone(27, 1);
                        break;
                    }
                    else if (slot.Quantity == 1)
                    {
                        slot.CleanSlot();
                        USFX.PlayDrinkSfx();
                        Bar.GiveManaByTime(100, 8);
                        GM.InstantiateGone(27, 1);
                        break;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            for (int i = 0; i < CheckSlots.Length; i++)
            {
                Slot slot = CheckSlots[i].GetComponent<Slot>();
                if (slot.Id == 28) //Stamina Potion Id
                {
                    UISfxManager USFX = gameObject.GetComponent<UISfxManager>();
                    BarManager Bar = gameObject.GetComponent<BarManager>();
                    GoneManager GM = gameObject.GetComponent<GoneManager>();
                    if (slot.Quantity > 1)
                    {
                        slot.Quantity -= 1;
                        USFX.PlayDrinkSfx();
                        Bar.GiveStaminaByTime(100);
                        GM.InstantiateGone(28, 1);
                        break;
                    }
                    else if (slot.Quantity == 1)
                    {
                        slot.CleanSlot();
                        USFX.PlayDrinkSfx();
                        Bar.GiveStaminaByTime(100);
                        GM.InstantiateGone(28, 1);
                        break;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemUsing : MonoBehaviour
{
    Inventory inv;
    public GameObject SelectedSlot;
    ActivatorManager act;
    private void Start()
    {
        act = gameObject.GetComponent<ActivatorManager>();
    }
    void Update()
    {
        if (!act.SomeoneActive)
        {
            SelectedSlot = null;
        }
        if (SelectedSlot != null && Input.GetKeyDown(KeyCode.E))
        {
            Slot slot = SelectedSlot.GetComponent<Slot>();
            UISfxManager USFX = gameObject.GetComponent<UISfxManager>();
            GoneManager GM = gameObject.GetComponent<GoneManager>();
            if (slot.isFull)
            {
                if (slot.Category == "Potion")
                {
                    if (slot.Id == 26) //Health Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            BarManager Bar = gameObject.GetComponent<BarManager>();
                            Bar.GiveHealByTime(60, 6);
                            GM.InstantiateGone(26, 1);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            BarManager Bar = gameObject.GetComponent<BarManager>();
                            Bar.GiveHealByTime(60, 6);
                            GM.InstantiateGone(26, 1);
                        }
                    }
                    if (slot.Id == 27) //Mana Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            BarManager Bar = gameObject.GetComponent<BarManager>();
                            Bar.GiveManaByTime(100, 8);
                            GM.InstantiateGone(27, 1);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            BarManager Bar = gameObject.GetComponent<BarManager>();
                            Bar.GiveManaByTime(100, 8);
                            GM.InstantiateGone(27, 1);
                        }
                    }
                    if (slot.Id == 28) //Stamina Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            BarManager Bar = gameObject.GetComponent<BarManager>();
                            Bar.GiveStaminaByTime(100);
                            GM.InstantiateGone(28, 1);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            BarManager Bar = gameObject.GetComponent<BarManager>();
                            Bar.GiveStaminaByTime(100);
                            GM.InstantiateGone(28, 1);
                        }
                    }
                    if (slot.Id == 25) //Def Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            GM.InstantiateGone(25, 1);
                            DSPM.AddNewStatPotion(25, 300);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewStatPotion(25, 300);
                            GM.InstantiateGone(25, 1);
                        }
                    }
                    if (slot.Id == 29) //Str Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            GM.InstantiateGone(29, 1);
                            DSPM.AddNewStatPotion(29, 300);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewStatPotion(29,300);
                            GM.InstantiateGone(29, 1);
                        }
                    }
                    if (slot.Id == 38) //Melee Speed Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            GM.InstantiateGone(38, 1);
                            DSPM.AddNewStatPotion(38, 300);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewStatPotion(38, 300);
                            GM.InstantiateGone(38, 1);
                        }
                    }
                    if (slot.Id == 39) //Magical Defence Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            GM.InstantiateGone(39, 1);
                            DSPM.AddNewStatPotion(39, 200);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewStatPotion(39, 200);
                            GM.InstantiateGone(39, 1);
                        }
                    }
                    if (slot.Id == 40) //Physical Defence Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            GM.InstantiateGone(40, 1);
                            DSPM.AddNewStatPotion(40, 200);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewStatPotion(40, 200);
                            GM.InstantiateGone(40, 1);
                        }
                    }
                    if (slot.Id == 41) //Poison bottle Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewScriptPotion(41, 600);
                            GM.InstantiateGone(41, 1);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewScriptPotion(41, 600);
                            GM.InstantiateGone(41, 1);
                        }
                    }
                    if (slot.Id == 42) //Antidote Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            GM.InstantiateGone(42, 1);
                            List<GameObject> Stats = PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>().PotionBufferContainer;
                            for (int i = 0; i < Stats.Count; i++)
                            {
                                if (Stats[i].GetComponent<StatPotion>() == null)
                                {
                                    if (Stats[i].GetComponent<ScriptPotion>().Id == 41)
                                    {
                                        Stats[i].GetComponent<ScriptPotion>().Timee = 0;
                                    }
                                }
                            }
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            GM.InstantiateGone(42, 1);
                            List<GameObject> Stats = PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>().PotionBufferContainer;
                            for (int i = 0; i < Stats.Count; i++)
                            {
                                if (Stats[i].GetComponent<StatPotion>() == null)
                                {
                                    if (Stats[i].GetComponent<ScriptPotion>().Id == 41)
                                    {
                                        Stats[i].GetComponent<ScriptPotion>().Timee = 0;
                                    }
                                }
                            }
                        }
                    }
                    if (slot.Id == 43) //Strong poison immunity Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewScriptPotion(43, 1200);
                            GM.InstantiateGone(43, 1);
                            PostProcessingManager.instance.isPoisoned = false;
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewScriptPotion(43, 1200);
                            GM.InstantiateGone(43, 1);
                            PostProcessingManager.instance.isPoisoned = false;
                        }
                    }
                    if (slot.Id == 44) //Critical Hit Potion
                    {
                        if (slot.Quantity > 1)
                        {
                            slot.Quantity -= 1;
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewScriptPotion(44, 30);
                            GM.InstantiateGone(44, 1);
                        }
                        else if (slot.Quantity == 1)
                        {
                            slot.CleanSlot();
                            USFX.PlayDrinkSfx();
                            DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                            DSPM.AddNewScriptPotion(44, 30);
                            GM.InstantiateGone(44, 1);
                        }
                    }
                }
                if (slot.Id == 45) //Ancient Stone
                {
                    if (slot.Quantity > 1)
                    {
                        slot.Quantity -= 1;
                        PostProcessingManager.instance.GetComponent<Currency>().IncreaseGem(2);
                        GM.InstantiateGone(45, 1);
                        USFX.PlayButtonClick();
                    }
                    else if (slot.Quantity == 1)
                    {
                        slot.CleanSlot();
                        PostProcessingManager.instance.GetComponent<Currency>().IncreaseGem(2);
                        GM.InstantiateGone(45, 1);
                        USFX.PlayButtonClick();
                    }
                }
                if (slot.Id == 46) //Philter Of Reality
                {
                    if (slot.Quantity > 1)
                    {
                        slot.Quantity -= 1;
                        USFX.PlayDrinkSfx();
                        DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                        DSPM.AddNewScriptPotion(46, 500);
                        GM.InstantiateGone(46, 1);
                    }
                    else if (slot.Quantity == 1)
                    {
                        slot.CleanSlot();
                        USFX.PlayDrinkSfx();
                        DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                        DSPM.AddNewScriptPotion(46, 500);
                        GM.InstantiateGone(46, 1);
                    }
                }
                if (slot.Id == 47) //Mercy Mixture
                {
                    if (slot.Quantity > 1)
                    {
                        slot.Quantity -= 1;
                        USFX.PlayDrinkSfx();
                        USFX.PlayHealSound();
                        BarManager DSPM = Inventory.instance.gameObject.GetComponent<BarManager>();
                        DSPM.maxMercy += 1;
                        DSPM.gameObject.GetComponent<Currency>().recalculateMercy();
                        GM.InstantiateGone(47, 1);
                    }
                    else if (slot.Quantity == 1)
                    {
                        slot.CleanSlot();
                        USFX.PlayDrinkSfx();
                        USFX.PlayHealSound();
                        BarManager DSPM = Inventory.instance.gameObject.GetComponent<BarManager>();
                        DSPM.maxMercy += 1;
                        DSPM.gameObject.GetComponent<Currency>().recalculateMercy();
                        GM.InstantiateGone(47, 1);
                    }
                }
                if (slot.Id == 49) //God's Prism
                {
                    if (slot.Quantity > 1)
                    {
                        slot.Quantity -= 1;
                          if (MainDatabaseManager.instance != null)
                        {
                            if (!MainDatabaseManager.instance.closeDATABASE)
                            {
                                MainDatabaseManager.instance.WinGame(MainDatabaseManager.instance.databasePiercer.playerID);
                            }
                        }
                        if (EndingSceneMan.instance == null) return;
                        EndingSceneMan.instance.endingSeq();
                        GM.InstantiateGone(49, 1);  
                    }
                    else if (slot.Quantity == 1)
                    {
                        slot.CleanSlot();
                        if (MainDatabaseManager.instance != null)
                        {
                            if (!MainDatabaseManager.instance.closeDATABASE)
                            {
                                MainDatabaseManager.instance.WinGame(MainDatabaseManager.instance.databasePiercer.playerID);
                            }
                        }
                        if (EndingSceneMan.instance == null) return;
                        EndingSceneMan.instance.endingSeq();
                        GM.InstantiateGone(49, 1);    
                    }
                }
                if (slot.Id == 53)
                {
                    int i = Random.Range(0, 101);
                    if (slot.Quantity > 1)
                    {
                        slot.Quantity -= 1;
                        if (i < 5)
                        {
                            USFX.PlayButtonClick();
                            USFX.PlayHealSound();
                            BarManager DSPM = Inventory.instance.gameObject.GetComponent<BarManager>();
                            DSPM.maxMercy += 1;
                            DSPM.gameObject.GetComponent<Currency>().recalculateMercy();
                            FadingTextCreator.instance.CreateFadeText("Book gave you extra one more mercy", 15, 5, null, Color.red);
                            GM.InstantiateGone(53, 1);
                        }else
                        {
                            USFX.PlayButtonClick();
                            FadingTextCreator.instance.CreateFadeText("Book gave you nothing", 1, 5, null, Color.blue);
                        }
                    }
                    else if (slot.Quantity == 1)
                    {
                        slot.CleanSlot();
                        if (i < 5)
                        {
                            USFX.PlayButtonClick();
                            USFX.PlayHealSound();
                            BarManager DSPM = Inventory.instance.gameObject.GetComponent<BarManager>();
                            DSPM.maxMercy += 1;
                            DSPM.gameObject.GetComponent<Currency>().recalculateMercy();
                            FadingTextCreator.instance.CreateFadeText("Book gave you extra one more mercy", 15, 5, null, Color.red);
                            GM.InstantiateGone(53, 1);
                        }
                        else
                        {
                            USFX.PlayButtonClick();
                            FadingTextCreator.instance.CreateFadeText("Book gave you nothing", 1, 5, null, Color.blue);
                        }
                        GM.InstantiateGone(53, 1);
                    }
                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelperOfDmr;

public class RaycastCenter : MonoBehaviour
{
    public Text PopUpText;
    public Camera cam;
    ActivatorManager act;
    RaycastHit hit;
    public Slot ChestSlot;

    //ItemPart
    public static bool LookingItem;
    public static GameObject LookingItemObject;
    private GameObject _lastDissolveObject;
    private string _dissolveNeededMaterials;
    void Update()
    {
        if (act == null)
        {
            act = this.gameObject.GetComponent<ActivatorManager>();
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3f))
        {
            if (hit.transform.tag == "Chest")
            {
                Chest chestt = hit.transform.GetComponent<Chest>();
                PopUpText.enabled = true;
                if (chestt.Openable)
                {
                    PopUpText.text = "[F] - Open";
                    PopUpText.color = Color.yellow;
                }
                else
                {
                    PopUpText.text = "Locked By Some Power";
                    PopUpText.color = Color.red;
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Chest chest = hit.transform.GetComponent<Chest>();
                    if (chest.Openable)
                    {
                        if (!chest.OpenState)
                        {
                            chest.Opening = true;
                        }
                        else if (chest.OpenStatDone)
                        {
                            chest.SetSlotItemAndOpenSlot(ChestSlot, act);
                            SlotToTestTracker STTT = gameObject.GetComponent<SlotToTestTracker>();
                            STTT.chestObj = chest;
                        }
                    }
                }
            }
            if (hit.transform.tag == "PickableObject")
            {
                if (hit.transform.GetComponent<PickableItem>() != null) hit.transform.GetComponent<PickableItem>().UpdateAllSpritesAndText();
                PopUpText.enabled = true;
                PopUpText.text = "[F] - Pick Up";
                PopUpText.color = Color.blue;
                LookingItem = true;
                LookingItemObject = hit.transform.gameObject;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Inventory inv = gameObject.GetComponent<Inventory>();
                    inv.CalculateSlotCanHandle(hit.transform.GetComponent<PickableItem>().Id);
                    if (hit.transform.GetComponent<PickableItem>().Count <= inv.AllSlotsCanHandleForLastAddedItem)
                    {
                        inv.AddItem(hit.transform.GetComponent<PickableItem>().Id, hit.transform.GetComponent<PickableItem>().Count);
                        hit.transform.GetComponent<PickableItem>().Claimed = true;
                        UISfxManager UISM = gameObject.GetComponent<UISfxManager>();
                        UISM.PlayWearSfx();
                    }
                    else
                    {
                        UISfxManager UISM = gameObject.GetComponent<UISfxManager>();
                        UISM.PlayErrorSfx();
                    }
                }
            }
            if (hit.transform.tag == "EnemyMob")
            {
                if (hit.transform.GetComponent<MobMrChest>() != null)
                {
                    LookingItemObject = hit.transform.gameObject;
                }
            }
            if (hit.transform.tag == "DissolveObject")
            {
                if (_lastDissolveObject != hit.transform.gameObject)
                {
                    if (hit.transform.gameObject.GetComponent<DissolveDefinements>() == null)
                    {
                        _lastDissolveObject = hit.transform.gameObject;
                        _dissolveNeededMaterials = "";
                    }
                    else
                    {
                        DissolveDefinements DF = hit.transform.gameObject.GetComponent<DissolveDefinements>();
                        if (!DF.DoesHaveDissolveItem && !DF.DoesHaveDissolveItem) { _dissolveNeededMaterials = ""; }
                         if (DF.DoesHaveDissolveRune) { _dissolveNeededMaterials = "Needed: Rune " + DF.DissolveCostRune + "\n"; }
                         if (DF.DoesHaveDissolveItem && DF.DoesHaveDissolveRune) { _dissolveNeededMaterials += "Item : " + DF.DissolveCostItem.ItemName + " " + DF.DissolveCostItemQuantity; } else if (DF.DoesHaveDissolveItem) { _dissolveNeededMaterials = "Item : " + DF.DissolveCostItem.ItemName + " " + DF.DissolveCostItemQuantity; }
                        _lastDissolveObject = hit.transform.gameObject;
                    }
                }
                PopUpText.enabled = true;
                PopUpText.text = "[F] To Dissolve Magic Barrier \n " + _dissolveNeededMaterials;
                if (PopUpText.color != Color.blue) PopUpText.color = Color.blue;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    DissolveDefinements DF = _lastDissolveObject.GetComponent<DissolveDefinements>();
                    if (DF.DoesHaveDissolveRune) 
                    {
                        int CurrentRune = PostProcessingManager.instance.GetComponent<Currency>().GetGem();
                        if (CurrentRune < DF.DissolveCostRune)
                        {
                            PostProcessingManager.instance.GetComponent<UISfxManager>().PlayErrorSfx();
                            return;
                        }
                    } 
                    if (DF.DoesHaveDissolveItem)
                    {
                        int CurrentItemQuantity = PostProcessingManager.instance.GetComponent<Inventory>().howManyItemInInventory(DF.DissolveCostItem.Id);
                        bool doWeHaveItem = PostProcessingManager.instance.GetComponent<Inventory>().isItemInInventory(DF.DissolveCostItem.Id);
                        if (!doWeHaveItem)
                        {
                            PostProcessingManager.instance.GetComponent<UISfxManager>().PlayErrorSfx();
                            return;
                        }
                        if (CurrentItemQuantity < DF.DissolveCostItemQuantity)
                        {
                            PostProcessingManager.instance.GetComponent<UISfxManager>().PlayErrorSfx();
                            return;
                        }
                    }
                    if (DF.DoesHaveDissolveItem || DF.DoesHaveDissolveRune)
                    {
                        if (DF.DoesHaveDissolveRune && DF.DissolveCostConsumeRune) PostProcessingManager.instance.GetComponent<Currency>().DecreaseGem(DF.DissolveCostRune);
                        if (DF.DoesHaveDissolveItem && DF.DissolveCostItem)
                        {
                            if (DF.DissolveCostConsumeItem)
                            {
                                PostProcessingManager.instance.GetComponent<Inventory>().destroyCertainItemInInventory(DF.DissolveCostItem.Id, DF.DissolveCostItemQuantity);
                            }
                        }
                    }
                    hit.transform.GetComponent<U10PS_DissolveOverTime>().enabled = true;
                    DF.serializeMe();
                    StartCoroutine(CloseDissolveObjectOnLavaGolem(hit.transform.gameObject));
                }
            }
            if (hit.transform.tag == "TradeStone")
            {
                if (!act.SomeoneActive)
                {
                    PopUpText.enabled = true;
                    PopUpText.text = "[F] To Open Trade Rune";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        hit.transform.GetComponent<TradeStoneMain>().openPanel();
                        hit.transform.GetComponent<TradeStoneMain>().RecalculateSellStuff();
                    }
                }
            }
            if (hit.transform.tag == "SacrificeShrine")
            {
                if (!act.SomeoneActive)
                {
                    //DO STUFF
                    Inventory inv = PostProcessingManager.instance.GetComponent<Inventory>();
                    bool foundLegendary = false;
                    DataItem legendaryItem = null;
                    if (FrostIslandCustom.instance.ShrineSealed)
                    {
                        PopUpText.enabled = false;
                    }
                    else
                    {
                        PopUpText.enabled = true;
                        PopUpText.text = "[F] Sacrifice Legendary Item";
                        PopUpText.color = Color.yellow;
                    }
                    if (Input.GetKeyDown(KeyCode.F) && !FrostIslandCustom.instance.ShrineSealed)
                    {
                        List<DataItem> DIList = new List<DataItem>();
                        foreach (GameObject sl in inv.Slots)
                        {
                            Slot slot = sl.GetComponent<Slot>();
                            if (slot.Rarity != "Legendary") continue;
                            foreach (DataItem di in inv.DataItems)
                            {
                                if (slot.Id == di.Id)
                                {
                                    DIList.Add(di);
                                    continue;
                                }
                            }
                        }
                        DIList.Shuffle();
                        if(DIList.Count > 0) legendaryItem = DIList[0];
                        if (legendaryItem != null) foundLegendary = true;
                        if (foundLegendary)
                        {
                            UISfxManager.instance.PlayHealSound();
                            inv.destroyCertainItemInInventory(legendaryItem.Id, 1);
                            FrostIslandCustom.instance.openSeal();
                            //Access Static 
                        }
                        else
                        {
                            UISfxManager.instance.PlayErrorSfx();
                        }
                    }
                }
            }
            if (hit.transform.tag == "DrinkableObject")
            {
                if (!act.SomeoneActive)
                {
                    if (hit.transform.GetComponent<CauldronRandom>().thisKind == "Empty") return;
                    PopUpText.enabled = true;
                    PopUpText.text = "[F] To Drink Random Mix";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        UISfxManager USFX = gameObject.GetComponent<UISfxManager>();
                        DrinkableStatPotionManager DSPM = gameObject.GetComponent<DrinkableStatPotionManager>();
                        USFX.PlayDrinkSfx();
                        string kind = hit.transform.GetComponent<CauldronRandom>().thisKind;
                        switch (kind)
                        {
                            case "Poison":
                                DSPM.AddNewScriptPotion(41, Random.Range(120,900));
                                hit.transform.GetComponent<CauldronRandom>().thisKind = "Empty";
                                break;
                            case "Antipoison":
                                DSPM.AddNewScriptPotion(43, Random.Range(120, 1800));
                                hit.transform.GetComponent<CauldronRandom>().thisKind = "Empty";
                                break;
                            case "Def":
                                DSPM.AddNewStatPotion(25, Random.Range(120, 900));
                                hit.transform.GetComponent<CauldronRandom>().thisKind = "Empty";
                                break;
                            case "Str":
                                DSPM.AddNewStatPotion(29, Random.Range(120, 900));
                                hit.transform.GetComponent<CauldronRandom>().thisKind = "Empty";
                                break;
                        }
                        hit.transform.GetComponent<CauldronRandom>().switchGoopObj();
                        //Add Effect 
                    }
                }
            }
            if (hit.transform.tag == "TakeBuff")
            {
                if (!act.SomeoneActive)
                {
                    BrigSpecialScript BSS = BrigSpecialScript.instance;
                    if (BSS.IsBuffTaken) {PopUpText.enabled = false; return;}
                    PopUpText.enabled = true;
                    PopUpText.text = "[F] To Get " + BSS.BuffKind;
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        UISfxManager.instance.PlayManaSound();
                        BSS.IsBuffTaken = true;
                        BSS.addCertainBuff();
                    }
                }
            }
            if (hit.transform.tag == "SacrificeBlob")
            {
                if (!act.SomeoneActive)
                {
                    BrigSpecialScript BSS = BrigSpecialScript.instance;
                    if (BSS.IsDissolveGateOpen || BSS.IsDestroyPortalOpen) return;
                    PopUpText.enabled = true;
                    PopUpText.text = "[F] To Accept Offer (%60 Losing All Of The Items) Or [G] To Destroy Altar";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        BSS.OpenGate();
                        UISfxManager.instance.PlayManaSound();
                    }
                    else if (Input.GetKeyDown(KeyCode.G))
                    {
                        UISfxManager.instance.PlayGolemWhooshSound();
                        BSS.StartCoroutine(BSS.DestroyAltar());
                    }
                }
            }
            if (hit.transform.tag == "Interactable")
            {
                if (act.SomeoneActive) return;
                if (hit.transform.GetComponent<IInteractable>() == null) return;
                IInteractable objInterface = hit.transform.GetComponent<IInteractable>();
                PopUpText.enabled = true;
                PopUpText.color = objInterface.GetTextColor();
                PopUpText.text = objInterface.GetLookAtDescription();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    objInterface.OnInteract();
                }
                if (hit.transform.GetComponent<IInteractableV2>() == null) return;
                IInteractableV2 objInterface2 = hit.transform.GetComponent<IInteractableV2>();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    objInterface2.OnInteractV2();
                }
            }
        }
        else
        {
            PopUpText.enabled = false;
            LookingItem = false;
            LookingItemObject = null;
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {

        }
        else
        {
            PopUpText.enabled = false;
        }
    }

    IEnumerator CloseDissolveObjectOnLavaGolem(GameObject gobject)
    {
        yield return new WaitForSeconds(3.5f);
        gobject.SetActive(false);
        StopCoroutine(CloseDissolveObjectOnLavaGolem(gobject));
        yield return null;
    }
}

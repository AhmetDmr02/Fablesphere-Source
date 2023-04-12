using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class SlotSelecter : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
    Item item;
    public GameObject SplitBar;
    public Text Rarity_text,name_Text, description_text,Cost_Text,Category_text;
    private bool Splitting = false;
    private bool DisableSplitting;
    public bool EnableSfx;
   // public GameObject Coin;
    Inventory inventory;
    ItemUsing ItemUse;
    //Writed By ADmr0
    void Start()
    {
        SplitBar.SetActive(false);
        item = GameObject.FindGameObjectWithTag("Preview").GetComponent<Item>();
        ItemUse = GameObject.FindGameObjectWithTag("Manager").GetComponent<ItemUsing>();
    }
    void Update()
    {
      //  Coin = GameObject.FindGameObjectWithTag("Coin");
    }
    public void OnPointerEnter(PointerEventData eventdata)
    {
        inventory = PostProcessingManager.instance.gameObject.GetComponent<Inventory>();
        if (eventdata.pointerEnter.gameObject.GetComponent<Slot>() != null)
        {
            ItemUse.SelectedSlot = eventdata.pointerEnter.gameObject;
        }
        for (int x = 0; x < inventory.DataItems.Length; x++)
        {
            if (inventory.DataItems[x].Id == gameObject.GetComponent<Slot>().Id && gameObject.GetComponent<Slot>().isFull)
            {
                name_Text.text = inventory.DataItems[x].ItemName;
                description_text.text = inventory.DataItems[x].ItemDescription;
                Rarity_text.text = inventory.DataItems[x].Rarity.ToString();
                Category_text.text = "Category: " + inventory.DataItems[x].Category;
                if (inventory.DataItems[x].Rarity == DataItem.RarityRnum.Common)
                {
                    Rarity_text.color = Color.gray;
                    Category_text.color = Color.gray;
                    name_Text.color = Color.gray;
                }
                if (inventory.DataItems[x].Rarity == DataItem.RarityRnum.Rare)
                {
                    Rarity_text.color = Color.blue;
                    Category_text.color = Color.blue;
                    name_Text.color = Color.blue;
                }
                if (inventory.DataItems[x].Rarity == DataItem.RarityRnum.Epic)
                {
                    Rarity_text.color = Color.magenta;
                    Category_text.color = Color.magenta;
                    name_Text.color = Color.magenta;
                }
                if (inventory.DataItems[x].Rarity == DataItem.RarityRnum.Legendary)
                {
                    Rarity_text.color = Color.yellow;
                    Category_text.color = Color.yellow;
                    name_Text.color = Color.yellow;
                }
                string CostText = "Selling Cost: " + inventory.DataItems[x].SellCost + " Trade Rune";
                Cost_Text.text = CostText;
            }
        }   
    }
    public void OnPointerExit(PointerEventData eventdata)
    {
       ItemUse.SelectedSlot = null;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && Input.GetKey(KeyCode.LeftShift) && !DisableSplitting)
        {
            if (!item.Dragging && this.gameObject.GetComponent<Slot>().Stackable && this.gameObject.GetComponent<Slot>().isFull && !Splitting && this.gameObject.GetComponent<Slot>().Stackable)
            {
                SplitBar.SetActive(true);
                Slot slot;
                slot = this.gameObject.GetComponent<Slot>();
                Splitting = true;
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Left && !item.Dragging)
        {
            if (!item.Dragging && this.gameObject.GetComponent<Slot>().isFull && !Splitting)
            {
                Slot slot;
                slot = this.gameObject.GetComponent<Slot>();
                item.isFull = slot.isFull;
                item.Id = slot.Id;
                item.Quantity = slot.Quantity;
                item.MaxStack = slot.MaxStack;
                item.ItemSprite = slot.ItemSprite;
                item.Stackable = slot.Stackable;
                item.Category = slot.Category;
                item.Cost = slot.Cost;
                item.Sellcost = slot.SellCost;
                item.Rarity = slot.Rarity;
                item.Dragging = true;
                slot.CleanSlot();
               
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right && item.Dragging)
        {
            if (item.Dragging && this.gameObject.GetComponent<Slot>().Id == item.Id && !Splitting)
            {
                Slot slot;
                slot = this.gameObject.GetComponent<Slot>();
                if (slot.CategoryFilter == "" || slot.CategoryFilter == null)
                {
                    if (slot.Quantity < slot.MaxStack && item.Quantity > 1)
                    {
                        item.Quantity -= 1;
                        slot.Quantity += 1;
                    }
                    else if (slot.Quantity < slot.MaxStack && item.Quantity <= 1)
                    {
                        slot.Quantity += 1;
                        item.CleanAllInfo();
                    }
                }
                else
                {
                    if (slot.CategoryFilter == item.Category)
                    {
                        if (slot.Quantity < slot.MaxStack && item.Quantity > 1)
                        {
                            item.Quantity -= 1;
                            slot.Quantity += 1;
                        }
                        else if (slot.Quantity < slot.MaxStack && item.Quantity <= 1)
                        {
                            slot.Quantity += 1;
                            item.CleanAllInfo();
                        }
                    }
                    else
                    {
                        if (EnableSfx)
                        {
                            UISfxManager UISfx = GameObject.FindGameObjectWithTag("Manager").GetComponent<UISfxManager>();
                            UISfx.PlayErrorSfx();
                            //PlayErrorSFX
                        }
                    }
                }
            }
            if (item.Dragging && !this.gameObject.GetComponent<Slot>().isFull && !Splitting)
            {
                Slot slot;
                slot = this.gameObject.GetComponent<Slot>();
                if (slot.CategoryFilter == "" || slot.CategoryFilter == null)
                {
                    if (slot.Id == 0 && item.Quantity > 1)
                    {
                        slot.Id = item.Id;
                        slot.isFull = item.isFull;
                        slot.MaxStack = item.MaxStack;
                        item.Quantity -= 1;
                        slot.Quantity += 1;
                        slot.ItemSprite = item.ItemSprite;
                        slot.Stackable = item.Stackable;
                        slot.Cost = item.Cost;
                        slot.SellCost = item.Sellcost;
                        slot.Category = item.Category;
                        slot.Rarity = item.Rarity;
                    }
                    else if (slot.Id == 0 && item.Quantity <= 1)
                    {
                        slot.Id = item.Id;
                        slot.isFull = item.isFull;
                        slot.Quantity = item.Quantity;
                        slot.MaxStack = item.MaxStack;
                        slot.ItemSprite = item.ItemSprite;
                        slot.Stackable = item.Stackable;
                        slot.Cost = item.Cost;
                        slot.SellCost = item.Sellcost;
                        slot.Category = item.Category;
                        slot.Rarity = item.Rarity;
                        item.Dragging = false;
                        item.CleanAllInfo();
                    }
                }
                else if (slot.CategoryFilter == item.Category)
                {
                    if (slot.Id == 0 && item.Quantity > 1)
                    {
                        slot.Id = item.Id;
                        slot.isFull = item.isFull;
                        slot.MaxStack = item.MaxStack;
                        item.Quantity -= 1;
                        slot.Quantity += 1;
                        slot.ItemSprite = item.ItemSprite;
                        slot.Stackable = item.Stackable;
                        slot.Cost = item.Cost;
                        slot.SellCost = item.Sellcost;
                        slot.Category = item.Category;
                        slot.Rarity = item.Rarity;
                    }
                    else if (slot.Id == 0 && item.Quantity <= 1)
                    {
                        slot.Id = item.Id;
                        slot.isFull = item.isFull;
                        slot.Quantity = item.Quantity;
                        slot.MaxStack = item.MaxStack;
                        slot.ItemSprite = item.ItemSprite;
                        slot.Stackable = item.Stackable;
                        slot.Cost = item.Cost;
                        slot.SellCost = item.Sellcost;
                        slot.Category = item.Category;
                        slot.Rarity = item.Rarity;
                        item.Dragging = false;
                        item.CleanAllInfo();
                    }
                }
                else
                {
                    if (EnableSfx)
                    {
                        UISfxManager UISfx = GameObject.FindGameObjectWithTag("Manager").GetComponent<UISfxManager>();
                        UISfx.PlayErrorSfx();
                        //PlayErrorSFX
                    }
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left && !Splitting && item.Dragging)
        {
            item = GameObject.FindGameObjectWithTag("Preview").GetComponent<Item>();
            if (item.Dragging && !this.gameObject.GetComponent<Slot>().isFull && this.gameObject.GetComponent<Slot>().Id != item.Id)
            {          
                Slot slot;
                slot = this.gameObject.GetComponent<Slot>();
                if (slot.CategoryFilter == "" || slot.CategoryFilter == null || slot.CategoryFilter == item.Category)
                {
                    slot.Id = item.Id;
                    slot.isFull = item.isFull;
                    slot.Quantity = item.Quantity;
                    slot.MaxStack = item.MaxStack;
                    slot.ItemSprite = item.ItemSprite;
                    slot.Stackable = item.Stackable;
                    slot.Cost = item.Cost;
                    slot.SellCost = item.Sellcost;
                    slot.Category = item.Category;
                    slot.Rarity = item.Rarity;
                    item.Dragging = false;
                    item.CleanAllInfo();
                }
                else
                {
                    if (EnableSfx)
                    {
                        UISfxManager UISfx = GameObject.FindGameObjectWithTag("Manager").GetComponent<UISfxManager>();
                        UISfx.PlayErrorSfx();
                        //PlayErrorSFX
                    }
                }
            }
            if (item.Dragging && this.gameObject.GetComponent<Slot>().isFull && this.gameObject.GetComponent<Slot>().Id != item.Id)
            {
                Slot slot;
                slot = this.gameObject.GetComponent<Slot>();
                if (slot.CategoryFilter == "" || slot.CategoryFilter == null || slot.CategoryFilter == item.Category)
                {
                    item.Id2 = slot.Id;
                    item.isFull2 = slot.isFull;
                    item.Quantity2 = slot.Quantity;
                    item.MaxStack2 = slot.MaxStack;
                    item.ItemSprite2 = slot.ItemSprite;
                    item.Stackable2 = slot.Stackable;
                    item.Cost2 = slot.Cost;
                    item.Sellcost2 = slot.SellCost;
                    item.Category2 = slot.Category;
                    item.Rarity2 = slot.Rarity;
                    slot.Id = item.Id;
                    slot.isFull = item.isFull;
                    slot.Quantity = item.Quantity;
                    slot.Category = item.Category;
                    slot.Cost = item.Cost;
                    slot.SellCost = item.Sellcost;
                    slot.MaxStack = item.MaxStack;
                    slot.ItemSprite = item.ItemSprite;
                    slot.Stackable = item.Stackable;
                    slot.Rarity = item.Rarity;
                    item.Calculate2To1();
                }
                else
                {
                    if (EnableSfx)
                    {
                        UISfxManager UISfx = GameObject.FindGameObjectWithTag("Manager").GetComponent<UISfxManager>();
                        UISfx.PlayErrorSfx();
                        //PlayErrorSFX
                    }
                }
            }
            if (item.Dragging && this.gameObject.GetComponent<Slot>().Id == item.Id)
            {
                Slot slot;
                slot = this.gameObject.GetComponent<Slot>();
                if (slot.CategoryFilter == "" || slot.CategoryFilter == null || slot.CategoryFilter == item.Category)
                {
                    int HowMuchCanClaim = slot.MaxStack; //How much slot can take
                    HowMuchCanClaim -= slot.Quantity;
                    if (item.Quantity > HowMuchCanClaim)
                    {
                        slot.Quantity += HowMuchCanClaim;
                        item.Quantity -= HowMuchCanClaim;
                    }
                    else if (item.Quantity == HowMuchCanClaim)
                    {
                        slot.Quantity += HowMuchCanClaim;
                        item.CleanAllInfo();
                    }
                    else if (item.Quantity < HowMuchCanClaim)
                    {
                        slot.Quantity += item.Quantity;
                        item.CleanAllInfo();
                    }
                }
                else
                {
                    if (EnableSfx)
                    {
                        UISfxManager UISfx = GameObject.FindGameObjectWithTag("Manager").GetComponent<UISfxManager>();
                        UISfx.PlayErrorSfx();
                        //PlayErrorSFX
                    }
                }
            }
        }
    }
    public void Split()
    {
        Slot slot;
        slot = this.gameObject.GetComponent<Slot>();
        int number;
        if (int.TryParse(SplitBar.GetComponent<InputField>().text, out number))
        {
            int i = int.Parse(SplitBar.GetComponent<InputField>().text);
            if (!item.Dragging)
            {
                if (i < slot.Quantity && i > 0)
                {
                    item.Dragging = true;
                    slot.Quantity -= i;
                    item.Id = slot.Id;
                    item.Quantity += i;
                    item.MaxStack = slot.MaxStack;
                    item.isFull = slot.isFull;
                    item.ItemSprite = slot.ItemSprite;
                    item.Cost = slot.Cost;
                    item.Sellcost = slot.SellCost;
                    item.Stackable = slot.Stackable;
                    item.Category = slot.Category;
                    Splitting = false;
                    SplitBar.SetActive(false);
                }
                else if (i == slot.Quantity)
                {
                    item.Dragging = true;
                    item.Id = slot.Id;
                    item.Quantity += slot.Quantity;
                    item.MaxStack = slot.MaxStack;
                    item.isFull = slot.isFull;
                    item.ItemSprite = slot.ItemSprite;
                    item.Cost = slot.Cost;
                    item.Sellcost = slot.SellCost;
                    item.Category = slot.Category;
                    item.Stackable = slot.Stackable;
                    Splitting = false;
                    slot.CleanSlot();
                    SplitBar.SetActive(false);
                }
            }
        }
        else
        {
            return;
        }
    }
}
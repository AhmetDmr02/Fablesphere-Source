using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SlotToTestTracker : MonoBehaviour
{
    public Slot slot;
    public Chest chestObj;
    ActivatorManager act;
    private void Update()
    {
        if (act == null) 
        {
            act = gameObject.GetComponent<ActivatorManager>();
        }
        if (act.ChestPoolActive)
        {
            chestObj.MySlot.isFull = slot.isFull;
            chestObj.MySlot.Id = slot.Id;
            chestObj.MySlot.Stackable = slot.Stackable;
            chestObj.MySlot.ItemSprite = slot.ItemSprite;
            chestObj.MySlot.MaxStack = slot.MaxStack;
            chestObj.MySlot.Quantity = slot.Quantity;
            chestObj.MySlot.Category = slot.Category;
            chestObj.MySlot.Cost = slot.Cost;
            chestObj.MySlot.SellCost = slot.SellCost;
            chestObj.MySlot.Rarity = slot.Rarity;
        }
    }
}
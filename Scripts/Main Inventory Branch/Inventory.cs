using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public DataItem[] DataItems;
    public GameObject[] Slots;
    public Slot[] AllSlots;
    public InputField FieldId, CountId;
    public int SetId, Count;
    [HideInInspector]
    public int PrivateTracker;
    //Writed By Admr0 For Test -And Successfully Completed
    public int AllSlotsCanHandleForLastAddedItem;
    private int number;
    public static Inventory instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(this.gameObject);
        }
    }

    public void AddItem(int itemId, int Count)
    {
        {
            bool Finished = false;
            bool Added = false;
            int RemainCount = 0;
            for (int y = 0; y < Slots.Length; y++)
            {
                int HowMuchWeCanClaimCount = 0;
                if (Slots[y].GetComponent<Slot>().Id == itemId)
                {
                    if (Slots[y].GetComponent<Slot>().Quantity != Slots[y].GetComponent<Slot>().MaxStack && Count <= 1 && Count <= AllSlotsCanHandleForLastAddedItem)
                    {
                        Slots[y].GetComponent<Slot>().Quantity += 1;
                        Added = true;
                        break;
                    }
                    else if (Count > 1 && Slots[y].GetComponent<Slot>().Quantity != Slots[y].GetComponent<Slot>().MaxStack && Count <= AllSlotsCanHandleForLastAddedItem)
                    {
                        HowMuchWeCanClaimCount = Slots[y].GetComponent<Slot>().MaxStack;
                        HowMuchWeCanClaimCount -= Slots[y].GetComponent<Slot>().Quantity;
                        if (HowMuchWeCanClaimCount >= Count)
                        {
                            Slots[y].GetComponent<Slot>().Quantity += Count;
                            Added = true;
                            break;
                        }
                        else if (HowMuchWeCanClaimCount < Count)
                        {
                            if (y == Slots.Length)
                            {
                                Slots[y].GetComponent<Slot>().Quantity += HowMuchWeCanClaimCount;
                                Count -= HowMuchWeCanClaimCount;
                                RemainCount = Count;
                                Added = true;
                                AddNew(itemId, RemainCount);
                                break;
                            }
                            else if (y < Slots.Length)
                            {
                                Slots[y].GetComponent<Slot>().Quantity += HowMuchWeCanClaimCount;
                                Count -= HowMuchWeCanClaimCount;
                                RemainCount = Count;
                            }
                        }
                    }
                }
            }
            Finished = true;
            if (Finished && !Added)
            {
                AddNew(itemId, Count);
            }
        }
    }
    public void AddNew(int itemId, int RemainCount)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (RemainCount == 0)
            {
                if (!Slots[i].GetComponent<Slot>().isFull)
                {
                    Slots[i].GetComponent<Slot>().isFull = true;
                    Slots[i].GetComponent<Slot>().Id = itemId;
                    for (int x = 0; x < DataItems.Length; x++)
                    {
                        if (DataItems[x].Id == itemId)
                        {
                            Slots[i].GetComponent<Slot>().Stackable = DataItems[x].Stackable;
                            Slots[i].GetComponent<Slot>().ItemSprite = DataItems[x].Photo;
                            Slots[i].GetComponent<Slot>().MaxStack = DataItems[x].MaxStack;
                            Slots[i].GetComponent<Slot>().Category = DataItems[x].Category;
                            Slots[i].GetComponent<Slot>().Cost = DataItems[x].Cost;
                            Slots[i].GetComponent<Slot>().SellCost = DataItems[x].SellCost;
                            Slots[i].GetComponent<Slot>().Rarity = DataItems[x].Rarity.ToString();
                            break;
                        }
                    }
                    break;
                }
            }
            else if (RemainCount > 0 && RemainCount <= AllSlotsCanHandleForLastAddedItem)
            {
                if (!Slots[i].GetComponent<Slot>().isFull)
                {
                    Slots[i].GetComponent<Slot>().isFull = true;
                    Slots[i].GetComponent<Slot>().Id = itemId;
                    for (int x = 0; x < DataItems.Length; x++)
                    {
                        if (DataItems[x].Id == itemId)
                        {
                            if (RemainCount <= DataItems[x].MaxStack)
                            {
                                Slots[i].GetComponent<Slot>().Stackable = DataItems[x].Stackable;
                                Slots[i].GetComponent<Slot>().ItemSprite = DataItems[x].Photo;
                                Slots[i].GetComponent<Slot>().MaxStack = DataItems[x].MaxStack;
                                Slots[i].GetComponent<Slot>().Category = DataItems[x].Category;
                                Slots[i].GetComponent<Slot>().Cost = DataItems[x].Cost;
                                Slots[i].GetComponent<Slot>().SellCost = DataItems[x].SellCost;
                                Slots[i].GetComponent<Slot>().Rarity = DataItems[x].Rarity.ToString();
                                Slots[i].GetComponent<Slot>().Quantity += RemainCount;
                                break;
                            }
                            else
                            {
                                Slots[i].GetComponent<Slot>().Stackable = DataItems[x].Stackable;
                                Slots[i].GetComponent<Slot>().ItemSprite = DataItems[x].Photo;
                                Slots[i].GetComponent<Slot>().MaxStack = DataItems[x].MaxStack;
                                Slots[i].GetComponent<Slot>().Quantity = DataItems[x].MaxStack;
                                Slots[i].GetComponent<Slot>().Category = DataItems[x].Category;
                                Slots[i].GetComponent<Slot>().Cost = DataItems[x].Cost;
                                Slots[i].GetComponent<Slot>().SellCost = DataItems[x].SellCost;
                                Slots[i].GetComponent<Slot>().Rarity = DataItems[x].Rarity.ToString();
                                RemainCount -= DataItems[x].MaxStack;
                                AddNew(itemId, RemainCount);
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }
    }
    public void CalculateSlotCanHandle(int itemIdForHowMuchEmpty)
    {
        AllSlotsCanHandleForLastAddedItem = 0;
        for (int z = 0; z < Slots.Length; z++)
        {
            Slot slot = Slots[z].GetComponent<Slot>();
            if (slot.Id == itemIdForHowMuchEmpty && slot.Quantity != slot.MaxStack)
            {
                AllSlotsCanHandleForLastAddedItem += slot.MaxStack;
                AllSlotsCanHandleForLastAddedItem -= slot.Quantity;
            }
            else if (!slot.isFull)
            {
                for (int x = 0; x < DataItems.Length; x++)
                {
                    if (DataItems[x].Id == itemIdForHowMuchEmpty)
                    {
                        AllSlotsCanHandleForLastAddedItem += DataItems[x].MaxStack;
                    }
                }
            }
        }
    }
    public void AddItemButton()
    {
        if (int.TryParse(FieldId.text, out number))
        {
            SetId = int.Parse(FieldId.text);
        }
        else
        {
            return;
        }
        if (int.TryParse(CountId.text, out number))
        {
            Count = int.Parse(CountId.text);
        }
        else
        {
            return;
        }
        CalculateSlotCanHandle(SetId);
        AddItem(SetId, Count);
    }
    public bool isItemInInventory(int itemId)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].GetComponent<Slot>().Id == itemId)
            {
                Debug.Log("Item Was: " + "True");
                return true;
                break;
            }
        }
        Debug.Log("Item Was: " + "False");
        return false;
    }
    public int howManyItemInInventory(int itemId)
    {
        int Quantity = 0;
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].GetComponent<Slot>().Id == itemId)
            {
                Quantity += Slots[i].GetComponent<Slot>().Quantity;
            }
        }
        Debug.Log("Item Count Was: " + Quantity);
        return Quantity;
    }
    public void destroyCertainItemInInventory(int itemId, int Count)
    {
        int Vcount = Count;
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].GetComponent<Slot>().Id == itemId)
            {
                if (Slots[i].GetComponent<Slot>().Quantity > Vcount)
                {
                    Slots[i].GetComponent<Slot>().Quantity -= Vcount;
                    Debug.Log("Item Cleared Succesfully");
                    break;
                } 
                else
                {
                    Vcount -= Slots[i].GetComponent<Slot>().Quantity;
                    Slots[i].GetComponent<Slot>().CleanSlot();
                }
            }
        }
        if (Vcount <= 0)
        {
            Debug.Log("Item Cleared Succesfully");
        }
        else
        {
            Debug.LogError("Critical Remove Procedure Failed");
        }
    }
}
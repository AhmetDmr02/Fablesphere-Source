using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    public bool isFull;
    public int Id, Quantity,Cost,SellCost;
    public Image ItemPhoto;
    public Text QuantityText;
    public bool Stackable;
    public string Category,CategoryFilter;
    public Sprite ItemSprite;
    public int MaxStack;
    private bool ReadyForDebug,TestOnce;
    //Writed By Admr0 For Test -And Successfully Completed
    public DataItem[] MainItems;
    public string Rarity;
    private bool SaveRecoveryBool;
    void Update()
    {
        if (MainItems.Length == 0)
        {
            MainItems = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>().DataItems;
        }

        ItemPhoto.sprite = ItemSprite;
        QuantityText.text = Quantity.ToString();
        if (Quantity == 0 && isFull)
        {
            Quantity = 1;
        } //If item exists its needs to be above than 0 
        if (Id == 0)
        {
            isFull = false;
            gameObject.GetComponent<Outline>().effectColor = Color.black; //Setting Rarity Color To Black
        }//If item id equals 0 slot will be empty
        if (!isFull)
        {
            ItemPhoto.color = new Color(0, 0, 0, 0); //Making Transparent
            QuantityText.enabled = false;
        }//If slot empty image will be transparent
        else
        {
            ItemPhoto.color = new Color(255, 255, 255, 255); //Making White
            // ItemPhoto.sprite = Resources.Load<Sprite>(Id.ToString()); //Automated Finding Item Sprite
        }
        if (Stackable)
        {
            QuantityText.enabled = true;
        }
        else
        {
            MaxStack = 1;
        }
        if (Id != 0)
        {
            ReadyForDebug = true;
            isFull = true;
            if (!TestOnce && ReadyForDebug)
            {
                TestOnce = true;
                DebugUnavaiableID();              
            }
            if (Rarity == "Common")
            {
                GetComponent<Outline>().effectColor = Color.gray;
            }
            if (Rarity == "Rare")
            {
                GetComponent<Outline>().effectColor = Color.blue;
            }
            if (Rarity == "Epic")
            {
                GetComponent<Outline>().effectColor = Color.magenta;
            }
            if (Rarity == "Legendary")
            {
                GetComponent<Outline>().effectColor = Color.yellow;
            }
        }
    }
    public void DebugUnavaiableID()
    {
        for (int i = 0; i <= MainItems.Length; i++)
        {
            if (i == MainItems.Length)
            {
                TestForDebug(i);
                break;
            }
        }
    }
    public void TestForDebug(int MaxItemId)
    {
        if (MaxItemId < Id)
        {
            print("Found Bugged Slot Cleaning Now..");
            CleanSlot();
        }
    }
    public void CleanSlot()
    {
        Id = 0;
        isFull = false;
        Id = 0;
        Quantity = 0;
        Rarity = string.Empty;
        Stackable = false;
        ItemSprite = null;
        MaxStack = 0;
        ReadyForDebug = false;
        TestOnce = false;
        Cost = 0;
        SellCost = 0;
        Category = string.Empty;
    }
}

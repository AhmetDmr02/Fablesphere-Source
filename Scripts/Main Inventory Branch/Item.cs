using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    //Writed By Admr0 For Test -And Successfully Completed
    public bool isFull;
    public int Id, Quantity,Cost,Sellcost;
    public bool Stackable;
    public Sprite ItemSprite;
    public string Category;
    public int MaxStack;
    public string Rarity;
    ////////////////////First Item
    public bool isFull2;
    public int Id2, Quantity2,Cost2,Sellcost2;
    public bool Stackable2;
    public string Category2;
    public Sprite ItemSprite2;
    public int MaxStack2;
    public string Rarity2;
    ///////////////////
    public bool Dragging;
    public Text Quantitytext;

    void Update()
    {
        Quantitytext.text = Quantity.ToString();
        if (Dragging)
        {
            gameObject.GetComponent<Image>().enabled = true;
            Quantitytext.enabled = true;
            gameObject.GetComponent<Image>().sprite = ItemSprite;
            this.transform.position = Input.mousePosition;
        }
        else
        {
            Quantitytext.enabled = false;
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.GetComponent<Image>().sprite = null;
        }
                
    }
  
    public void CleanAllInfo()
    {
        Id = 0;
        isFull = false;
        Quantity = 0;
        MaxStack = 0;
        Stackable = false;
        ItemSprite = null;
        Sellcost = 0;
        Cost = 0;
        Category = string.Empty;
        Rarity = string.Empty;
        Dragging = false;
        Id2 = 0;
        isFull2 = false;
        Quantity2 = 0;
        MaxStack2 = 0;
        Sellcost2 = 0;
        ItemSprite2 = null;
        Stackable2 = false;
        Cost2 = 0;
        Rarity2 = string.Empty;
        Category2 = string.Empty;
    }
    public void Calculate2To1()
    {
        Id = Id2;
        isFull = isFull2;
        Category = Category2;
        Rarity = Rarity2;
        Quantity = Quantity2;
        MaxStack = MaxStack2;
        Cost = Cost2;
        Sellcost = Sellcost2;
        Stackable = Stackable2;
        ItemSprite = ItemSprite2;
        Id2 = 0;
        ItemSprite2 = null;
        isFull2 = false;
        Quantity2 = 0;
        Rarity2 = string.Empty;
        Sellcost2 = 0;  
        MaxStack2 = 0;
        Stackable2 = false;
        Cost2 = 0;  
        Category2 = string.Empty;
    }
}   

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class DataItem : ScriptableObject
{
    //Writed By Admr0 For Test -And Successfully Completed
    public new int Id, MaxStack, Cost, SellCost;
    public new bool Stackable,BlockOnSpawnChest,BlockSpawnOnMarket,BlockSpawnOnMobs;
    [TextArea]
    public new string ItemDescription;
    public new string ItemName, Category;
    public new Sprite Photo;
    public new enum RarityRnum {Legendary,Epic,Rare,Common}
    public new RarityRnum Rarity;

    public void OnValidate()
    {
        if (MaxStack < 0) MaxStack = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armour Stat", menuName = "Armour Stat")]
public class DataArmour : ScriptableObject
{
    public new DataItem Item;
    public new int Str;
    public new int PhysicalDef,MagicalDef;
    public new int Speed;
}

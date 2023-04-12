using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon Stat",menuName = "Weapon Stat")]
public class DataWeapons : ScriptableObject
{
    public new DataItem Item;
    public new int Str;
    public new int Speed;
    public new int MagicResistance;
}

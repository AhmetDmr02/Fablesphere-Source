using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell Info")]
public class DataSpells : ScriptableObject
{
    public new DataWeapons MainWeapon;
    public new Sprite Spell_Sprite;
    public new string Spell_Name;
    public new string Description;
    public new float Cooldown;
    public new float Mana_Cost;
}

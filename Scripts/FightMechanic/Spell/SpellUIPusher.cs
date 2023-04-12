using UnityEngine;

public class SpellUIPusher : MonoBehaviour
{
    public Slot WeaponSlot;
    public DataSpells[] Spells;
    public GameObject SpellUI;
    public bool SpellWeaponActive;
    public Canvas worldCanvas;
    SpellManager SM;

    public int PreviousInt;
    private void Start()
    {
        SM = gameObject.GetComponent<SpellManager>();
    }
    private void Update()
    {
        if (SpellWeaponActive)
        {
            SpellUI.SetActive(true);
        } 
        else 
        {
            SpellUI.SetActive(false);
        }
        if (WeaponSlot.Id != PreviousInt)
        {
            PreviousInt = WeaponSlot.Id;
            bool IsThisSpellWeapon = CheckIsThisSpellWeapon(Spells,WeaponSlot);
            if (IsThisSpellWeapon)
            {
                DataSpells GetSpell = GetSpellWeapon(Spells, WeaponSlot);
                SpellWeaponActive = true;
                SM.CurrentData = GetSpell;
            }
            else
            {
                SpellWeaponActive = false;
                SM.CurrentData = null;
            }
        }
    }
    private bool CheckIsThisSpellWeapon(DataSpells[] spellData,Slot CurrentSlot) 
    {
        foreach (DataSpells spells in spellData)
        {

            if (spells.MainWeapon.Item.Id == CurrentSlot.Id)
            {
                return true;
            }
        }
        return false;
    }
    private DataSpells GetSpellWeapon(DataSpells[] spellData, Slot CurrentSlot)
    {
        foreach (DataSpells spells in spellData)
        {
            if (spells.MainWeapon.Item.Id == CurrentSlot.Id)
            {
                return spells;
            }
        }
        return null;
    }
}

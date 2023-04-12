using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellManager : MonoBehaviour
{
    UISfxManager USM;
    EffectManager EM;
    SpellCooldown SC;
    [Header("GUI")]
    public Image SpriteSheet;
    public Text Name_Text;
    public Text DescText;
    public Text CooldownText;
    public DataSpells CurrentData;
    UseGunBehaivour UGB;

    private void Start()
    {
        EM = gameObject.GetComponent<EffectManager>();  
        USM = gameObject.GetComponent<UISfxManager>();
        SC = gameObject.GetComponent<SpellCooldown>();
        UGB = gameObject.GetComponent<UseGunBehaivour>();
    }
    void Update()
    {
        if (CurrentData != null)
        {
            SpriteSheet.sprite = CurrentData.Spell_Sprite;
            Name_Text.text = CurrentData.Spell_Name;
            DescText.text = CurrentData.Description;
            CooldownText.text = "Cooldown = " + CurrentData.Cooldown;
        }
        if (CurrentData != null)
        {
            BarManager BM = gameObject.GetComponent<BarManager>();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!SC.OnCooldown && BM.Mana >= CurrentData.Mana_Cost)
                {
                    if (CurrentData.MainWeapon.Item.Id == 31) //Mana Gods
                    {
                        SC.CreateCooldown(CurrentData.Cooldown);
                        BM.Mana -= CurrentData.Mana_Cost;
                        ManaGodEffect();
                        //PlayEffects
                    }
                    if (CurrentData.MainWeapon.Item.Id == 30) //Healer Gods
                    {
                        SC.CreateCooldown(CurrentData.Cooldown);
                        BM.Mana -= CurrentData.Mana_Cost;
                        HealerEffect();
                    }
                    if (CurrentData.MainWeapon.Item.Id == 32) //Allowed Runes Stun
                    {
                        SC.CreateCooldown(CurrentData.Cooldown);
                        BM.Mana -= CurrentData.Mana_Cost;
                        StunEffect();
                    }
                    if (CurrentData.MainWeapon.Item.Id == 35) //Dark Knight Stun
                    {
                        SC.CreateCooldown(CurrentData.Cooldown);
                        BM.Mana -= CurrentData.Mana_Cost;
                        StunEffect();
                    }
                    if (CurrentData.MainWeapon.Item.Id == 36) //Sword Of Law Stun
                    {
                        SC.CreateCooldown(CurrentData.Cooldown);
                        BM.Mana -= CurrentData.Mana_Cost;
                        StunEffect();
                    }
                }
                else
                {
                    USM.PlayErrorSfx();
                }
                //UseSpell
            }
        }
    }


    //HealerSwordSide
    void HealerEffect()
    {
        BarManager BM = gameObject.GetComponent<BarManager>();
        BM.HP = BM.HPMax;
        EM.CreateHealEffect(FPSController.PlayerObject.transform,Color.red);
        USM.PlayHealSound();
    }
    //ManaSwordSide
    void ManaGodEffect()
    {
        //Find Active Sword Shot
        List<SwordsShot> SS = new List<SwordsShot>();
        SS.Clear();
        foreach (SwordIdHold SIH in UGB.Swords)
        {
            SS.Add(SIH.transform.gameObject.GetComponent<SwordsShot>());
        }
        SwordsShot Selected = FindEnableSword(SS);
        Selected.ManaGodActive = true;
        StartCoroutine(CloseManaGodEffect(Selected, 30f));
        USM.PlayManaSound();
        EM.CreateManaEffect(GameObject.FindGameObjectWithTag("Player").transform);
    }

    void StunEffect()
    {
        List<SwordsShot> SS = new List<SwordsShot>();
        SS.Clear();
        foreach (SwordIdHold SIH in UGB.Swords)
        {
            SS.Add(SIH.transform.gameObject.GetComponent<SwordsShot>());
        }
        SwordsShot Selected = FindEnableSword(SS);
        Selected.CritActive = true;
        USM.PlayHealSound();
        EM.CreateHealEffect(FPSController.PlayerObject.transform, Color.yellow);
    }
    SwordsShot FindEnableSword(List<SwordsShot> SwordList) {
        foreach (SwordsShot SwordShotScripts in SwordList)
        {
            if (SwordShotScripts.transform.gameObject.activeInHierarchy == true)
            {
                //Found
                return SwordShotScripts;
            }
        }
        return null;
    }
    IEnumerator CloseManaGodEffect(SwordsShot SS,float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        SS.ManaGodActive = false;
    }
}

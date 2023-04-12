using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarIncreaser : MonoBehaviour
{
    public float TargetHp,HpRate,HpFillAmount;
    public bool GiveHp;
    public float TargetMana, ManaRate, ManafillAmount;
    public bool GiveMana;
    BarManager BM;
    private void Start()
    {
        BM = gameObject.GetComponent<BarManager>();
    }
    void Update()
    {
        if (GiveHp)
        {
            if (BM.HP < TargetHp)
            {
                BM.HP += HpRate * Time.deltaTime;
            }
            else
            {
                GiveHp = false;
            }
        }
        if (GiveMana)
        {
            if (BM.Mana < TargetMana)
            {
                BM.Mana += ManaRate * Time.deltaTime;
            }
            else
            {
                GiveMana = false;
            }
        }
    }
}

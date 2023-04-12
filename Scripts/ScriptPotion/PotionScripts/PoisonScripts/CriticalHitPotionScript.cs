using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHitPotionScript : MonoBehaviour,IScriptPotion
{
    public void doPotionStuff()
    {
        PostProcessingManager.instance.CritPotionActive = true;
    }
    public void removePotionBuffs()
    {
        PostProcessingManager.instance.CritPotionActive = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PosionPotionScript : MonoBehaviour, IScriptPotion
{
    PostProcessingManager myManager;
    public void doPotionStuff()
    {
        if (myManager == null) myManager = PostProcessingManager.instance;
        if (myManager.isPoisoned) myManager.isPoisoned = false;
        if (!myManager.hasPoisonProtection) myManager.hasPoisonProtection = true;
    }
    public void removePotionBuffs()
    {
        myManager.hasPoisonProtection = false;
    }
}

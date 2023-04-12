using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBottleScript : MonoBehaviour, IScriptPotion
{
    PostProcessingManager myManager;
    public void doPotionStuff()
    {
        if (myManager == null) myManager = PostProcessingManager.instance;
        if (myManager.hasPoisonProtection)
        {
            Destroy(this.gameObject);
            myManager.isPoisoned = false;
            return;
        }
        if(!myManager.isPoisoned)
        {
            myManager.isPoisoned = true;
        }

    }

    public void removePotionBuffs()
    {
        myManager.isPoisoned = false;
    }
}

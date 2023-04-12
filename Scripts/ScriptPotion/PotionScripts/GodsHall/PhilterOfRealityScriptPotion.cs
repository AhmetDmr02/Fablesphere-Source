using UnityEngine;

public class PhilterOfRealityScriptPotion : MonoBehaviour, IScriptPotion
{
    public void doPotionStuff()
    {
        if (GodsHallMain.instance == null) return;
        GodsHallMain.instance.isPhilterActive = true;

    }

    public void removePotionBuffs()
    {
        if (GodsHallMain.instance == null) return;
        GodsHallMain.instance.isPhilterActive = false;
    }
}

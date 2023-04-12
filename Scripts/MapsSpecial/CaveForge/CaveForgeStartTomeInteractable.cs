using UnityEngine;

public class CaveForgeStartTomeInteractable : MonoBehaviour,IInteractable
{
    public ForgeSystemMain FSM;
    public Currency cur;

    public string GetLookAtDescription()
    {
        if (FSM.usedOnce || FSM.animStarted)
        {
            return "";
        }
        else
        {
            return "4 Rune To Create Random Equipment [F]";
        }
    }

    public Color GetTextColor()
    {
        if (cur == null) cur = Inventory.instance.gameObject.GetComponent<Currency>();
        if(cur.GetGem() <= 4) return Color.red;
        if(cur.GetGem() >= 4) return Color.green;
        return Color.blue;
    }

    public void OnInteract()
    {
        if (cur == null) cur = Inventory.instance.gameObject.GetComponent<Currency>();
        if (FSM.usedOnce || FSM.animStarted) return;
        if (cur.GetGem() < 4) return;
        cur.DecreaseGem(4);
        FSM.animStarted = true;
        FSM.StartInvokeToSpawn();
        FSM.gameObject.GetComponent<Animation>().Play();
    }
}

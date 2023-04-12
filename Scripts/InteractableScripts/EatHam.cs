using UnityEngine;
using HelperOfDmr;
public class EatHam : MonoBehaviour, IInteractable,IMapGetSerializer
{
    public string GetLookAtDescription()
    {
        return "[F] To Eat";
    }

    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        if (SIC.serialBoolList[0])
        {
            Destroy(this.gameObject);
        }
    }
    public void serializeMe()
    {
        SerializationInfoClass SIC = UtilitesOfDmr.CreateDefaultSIC(this.gameObject,this);
        SIC.serialBoolList.Add(true);
        SIC.dontDeleteThis = true;
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
        Destroy(this.gameObject);
    }

    public Color GetTextColor()
    {
        return Color.red;
    }

    public void OnInteract()
    {
        UISfxManager.instance.PlayCriticalShot();
        UISfxManager.instance.GetComponent<BarManager>().HP = UISfxManager.instance.GetComponent<BarManager>().HPMax;
        serializeMe();
    }
}

using UnityEngine;

public class RepairPortal : MonoBehaviour, IInteractable,IMapGetSerializer
{
    [SerializeField] private Animator _portalAnimator;
    [SerializeField] private AudioSource _ignitePortal, _portalInit;
    [SerializeField] private ParticleSystem _ignitePartice;
    private bool portalOpened;


    public string GetLookAtDescription()
    {
        if (portalOpened)
        {
            return "";
        }
        else
        {
            return "[F] To Fix Broken Portal \n Costs: 6 Rune";
        }
    }

    public Color GetTextColor()
    {
        return Color.green;
    }

    public void OnInteract()
    {
        if (portalOpened) return;
        Currency currency = PostProcessingManager.instance.GetComponent<Currency>();
        if (currency.GetGem() >= 6)
        {
            currency.DecreaseGem(6);
            portalOpened = true;
            _portalAnimator.Play("PortalBrokenToNormal");
            serializeMe();
        }else
        {
            UISfxManager.instance.PlayErrorSfx();
        }
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        if (SIC.serialBoolList[0])
        {
            _portalAnimator.Play("PortalBrokenToNormal");
            portalOpened = SIC.serialBoolList[0];
        }
    }
    public void serializeMe()
    {
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
        SIC.dontDeleteThis = true;
        SIC.generatedBySomething = false;
        SIC.serialBoolList.Add(portalOpened);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void FirePortalInitSound()
    {
        _portalInit.Play();
    }
    public void FirePortalIgniteSound()
    {
        _ignitePortal.Play();
    }
    public void FirePortalParticle()
    {
        _ignitePartice.Play();
    }
}

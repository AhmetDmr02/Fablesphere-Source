using UnityEngine;

public class FrostIslandCustom : MonoBehaviour, IMapGetSerializer
{
    private bool serialized;

    private bool isVaultOpen;
    private bool shrineSealed;

    public bool ShrineSealed => shrineSealed;
    [SerializeField] GameObject _vaultSecretObj;
    [SerializeField] Chest[] sealedChests;

    [SerializeField] private ParticleSystem PS;

    public static FrostIslandCustom instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += SerializeMe;
        if (serialized) return;
        Calculate();
        if (isVaultOpen) return;
    } 
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        isVaultOpen = SIC.serialBoolList[0];
        shrineSealed = SIC.serialBoolList[1];
        Calculate();
    }
    private void SerializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= SerializeMe;
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
        SIC.dontDeleteThis = false;
        SIC.generatedBySomething = false;
        SIC.serialBoolList.Add(isVaultOpen);
        SIC.serialBoolList.Add(shrineSealed);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    private void Calculate()
    {
        if (!serialized)
        {
            int i = Random.Range(0, 101);
            if (i <= 40)
            {
                isVaultOpen = true;
                _vaultSecretObj.SetActive(isVaultOpen);
            }
        }
        else
        {
            _vaultSecretObj.SetActive(isVaultOpen);
        }
    }
    public void openSeal()
    {
        PS.Play();
        foreach (Chest chest in sealedChests)
        {
            chest.Openable = true;
        }
        shrineSealed = true;
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= SerializeMe;
    }
}

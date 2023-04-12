using HelperOfDmr;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GargoyleStatue : MonoBehaviour,IMapGetSerializer
{
    [SerializeField] private float _hp = 300;
    [SerializeField] private int _percentAge = 25;
    [SerializeField] private Material _particleMaterial;
    [SerializeField] private DataItem containedItem;
    private bool isItDestroyed;

    private Vector3 particleSpawnPoint;
    private bool Critical;
    PlaySwordSwing PSS;
    public void WarmDamage(Vector3 particleSpawnPointt, PlaySwordSwing PS, bool critical)
    {
        PSS = PS;
        PS.gargoylePlay = true;
        Critical = critical;
        particleSpawnPoint = particleSpawnPointt;
        PS.warmDone += TakeDamage;
    }
    public void TakeDamage()
    {
        PSS.warmDone -= TakeDamage;
        EffectManager EM = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
        EM.CreateEnemyHit(particleSpawnPoint, Critical, _particleMaterial, _particleMaterial);
        StatData SD = PostProcessingManager.instance.gameObject.GetComponent<StatData>();
        float _decrease = SD.CalculatePlayerHit(10, Critical);
        _hp -= _decrease;
        if (_hp > 0)
        {
            UISfxManager.instance.PlayRockHitSound();
            if (Critical)
            {
                this.transform.localScale -= new Vector3(0.8f, 0.8f, 0.8f);
            }
            else
            {
                this.transform.localScale -= new Vector3(0.2f, 0.2f,0.2f);
            }
        }
        else if (_hp <= 0)
        {
            UISfxManager.instance.PlayRockHitSound();
            EffectManager.instance.CreateGargoyleDestroyEffect(this.transform);
            CreateItem();
        }
    }
    public void CreateItem()
    {
        int i = Random.Range(0, 101);
        if (i <= _percentAge)
        {
            PickableItemCreator.instance.CreatePickUpAbleItem(containedItem.Id, 1, this.transform, 0);
        }
        isItDestroyed = true;
        //Create Particles
        serializeMe(0);
    }

    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        isItDestroyed = SIC.serialBoolList[0];
        if (isItDestroyed) Destroy(this.gameObject);
    }
    public void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.dontDeleteThis = true;
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
        SIC.generatedBySomething = false;
        SIC.serialBoolList.Add(isItDestroyed);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
        Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        if(PSS != null)
        PSS.warmDone -= TakeDamage;
    }
}

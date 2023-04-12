using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyMainStat : MonoBehaviour
{
    public enum EnemyMat {Clothy,RockOrMud};
    [Header("Effect Side Things")]
    public EnemyMat MaterialOfEnemy;
    public Material ParticleMaterial;
    public Material TrailMaterial;
    public Image HealthBar;
    public TextMeshProUGUI EnemyNameText,HPText;
    public float BarSize = 1f;
    public Transform BarLocation;
    [SerializeField]
    private GameObject CreateBar;
    [Header("Main Stats")]
    public float MobDefence;
    public float MobHP;
    public float MobMaxHP;
    public float BaseScaleSize = 0.25f;
    public string MobName;
    private float MobHPRate;
    public float LevelScaleRate;
    public int Level = 1;
    UISfxManager USM;
    public EnemyHitCenter EHC;
    EffectManager EM;
    StatData SD;

    private GameObject CreatedObject;
    private void Awake()
    {
        Level = SectorManager.instance.currentSector + 1;
        float increaseVar = Level * LevelScaleRate;
        MobDefence += increaseVar * MobDefence;
        MobHP += increaseVar * MobHP;
        MobMaxHP = MobHP;   
        EHC.PhysicalDmg += increaseVar * EHC.PhysicalDmg;
        EHC.MagicalDmg += increaseVar * EHC.MagicalDmg;
        EHC.PureDmg += increaseVar * EHC.PureDmg;
    }
    private void Start()
    {
        if (CreateBar == null)
        {
            CreateBar = Resources.Load<GameObject>("BarHolder");
        }
        if (HealthBar == null)
        {
            GameObject WorldCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas");
            GameObject GO = Instantiate(CreateBar, WorldCanvas.transform);
            GO.transform.position = BarLocation.position;
            Vector3 Size = new Vector3(BarSize, BarSize, BarSize);
            GO.transform.localScale = Size;
            HealthBar = GO.transform.GetChild(0).GetComponent<Image>();
            HPText = GO.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            EnemyNameText = GO.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            CreatedObject = GO;
        }
        EnemyNameText.text = MobName;
    }
    void Update()
    {
        if (CreatedObject != null) CreatedObject.transform.position = BarLocation.position;
        int DesiredHp = (int)MobHP;
        HPText.text = DesiredHp.ToString();
        MobHPRate = MobHP / MobMaxHP;
        HealthBar.fillAmount = MobHPRate;
        if (SD == null)
            SD = PostProcessingManager.instance.gameObject.GetComponent<StatData>();
        if (USM == null)
            USM = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
        if (EM == null)
            EM = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
    }
    public bool emsCritic;
    public void PlayerHit(Vector3 EffectSpawnLocation, PlaySwordSwing PSS,bool Critical)
    {
        emsCritic = Critical;
        PSS.GetVariables(EffectSpawnLocation, TrailMaterial, ParticleMaterial,this.gameObject.GetComponent<EnemyMainStat>(),Critical);
        if (MaterialOfEnemy == EnemyMat.RockOrMud)
        {
            PSS.WTP = PlaySwordSwing.WhatToPlay.RockAndMud;
            PSS.CanPlay = true;
         //   USM.PlayRockHitSound();
        }
        else if(MaterialOfEnemy == EnemyMat.Clothy)
        {
            PSS.WTP = PlaySwordSwing.WhatToPlay.ClothSound;
            PSS.CanPlay = true;
            //USM.PlayClothHitSound();
        }
    }
    private void OnDestroy()
    {
        Destroy(this.CreatedObject);
    }
    public void DestroyBar()
    {
       this.CreatedObject.AddComponent<DestroyMe>().DestroyTime = 0f;
    }
}

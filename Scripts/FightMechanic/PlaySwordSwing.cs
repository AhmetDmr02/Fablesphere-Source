using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using System;

public class PlaySwordSwing : MonoBehaviour
{
    public bool PlayOneShot;
    public bool Playing;
    public bool Playing2;
    public bool CutPoint;
    public bool CanPlay;
    public bool CreateParticles;
    [SerializeField] private float[] PlayShotMS, CutPointMS;
    [SerializeField] Animator animManager;
    public enum WhatToPlay { ClothSound, RockAndMud };
    [HideInInspector]
    public WhatToPlay WTP;
    [HideInInspector]
    public bool CreateParticlesPrivate;
    [SerializeField]private float speedStat;
    public event Action warmDone;
    public float[] TempPlayShotMS = new float[4];
    public float[] TempCutPointMS = new float[4];
    public bool gargoylePlay;
    void Update()
    {
        if (animManager == null)
        {
            animManager = this.GetComponentInParent<Animator>();
        }
        if (CreateParticles && !CreateParticlesPrivate)
        {
            CreateParticles = false;
            CreateParticlesPrivate = true;
            //CreateSparks();
            //NO NEED ANYMORE
        }

        /*
        if (animManager.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack1"))
        {
            if (SwordStates[0])
            {
                SwordStates[0] = false;
                Debug.Log("calling sword1");
                StartCoroutine(OpenSwingSound(TempPlayShotMS[0]));
                StartCoroutine(OpenCutSound(TempCutPointMS[0]));
                //Invoke("OpenSwingSound", TempPlayShotMS[0]);
                //Invoke("OpenCutSound", TempCutPointMS[0]);
            }

        }
        if (animManager.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack2"))
        {
            if (SwordStates[1])
            {
                SwordStates[1] = false;
                Debug.Log("calling sword2");
                StartCoroutine(OpenSwingSound(TempPlayShotMS[1]));
                StartCoroutine(OpenCutSound(TempCutPointMS[1]));
                //Invoke("OpenSwingSound", TempPlayShotMS[1]);
                //Invoke("OpenCutSound", TempCutPointMS[1]);
            }
        }
        if (animManager.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack3"))
        {
            if (SwordStates[2])
            {
                SwordStates[2] = false;
                Debug.Log("calling sword3");
                StartCoroutine(OpenSwingSound(TempPlayShotMS[2]));
                StartCoroutine(OpenCutSound(TempCutPointMS[2]));
                //Invoke("OpenSwingSound", TempPlayShotMS[2]);
                //Invoke("OpenCutSound", TempCutPointMS[2]);
            }
        }
        if (animManager.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack4"))
        {
            if (SwordStates[3])
            {
                SwordStates[3] = false;
                Debug.Log("calling sword4");
                StartCoroutine(OpenSwingSound(TempPlayShotMS[3]));
                StartCoroutine(OpenCutSound(TempCutPointMS[3]));
                //Invoke("OpenSwingSound", TempPlayShotMS[3]);
                //Invoke("OpenCutSound", TempCutPointMS[3]);
            }
        }
        */
        if (!Playing && PlayOneShot)
        {
            Playing = true;
            PlayOneShot = false;
            UISfxManager USM = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
            USM.PlaySwordSwingSound();
        }
        if (!Playing2 && CutPoint)
        {
            Playing2 = true;
            CutPoint = false;
            UISfxManager USM = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
            if (CanPlay)
            {
                CanPlay = false;
                if (!Critical)
                {
                    if (WTP == WhatToPlay.ClothSound)
                        USM.PlayClothHitSound();
                    if (WTP == WhatToPlay.RockAndMud)
                        USM.PlayRockHitSound();
                    CameraShaker.Instance.ShakeOnce(1.75f, 1.75f * 1.5f, 0f, 1f);
                }
                else
                {
                    CameraShaker.Instance.ShakeOnce(2.3f, 2.3f * 1.5f, 0f, 1f);
                    USM.PlayCriticalShot();
                }
                EffectManager EM;
                EM = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
                EM.CreateEnemyHit(EffectSpawnLocation, Critical, TrailMaterial, ParticleMaterial);
                DecreaseHP();
                warmDone?.Invoke();
            }
            if (gargoylePlay)
            {
                Playing2 = true;
                CutPoint = false;
                if (gargoylePlay)
                {
                    gargoylePlay = false;
                    warmDone?.Invoke();
                }
            }
        }
    }
    private Vector3 EffectSpawnLocation;
    private Material TrailMaterial,ParticleMaterial;
    private EnemyMainStat EMS;
    private bool Critical;
    public void CreateSparks()
    {
      //SwordsShot SS = this.transform.parent.GetComponent<SwordsShot>();
    }
    public void GetVariables(Vector3 EffectSpawnLocation_, Material TrailMaterial_, Material ParticleMaterial_,EnemyMainStat EMS_,bool Critical_)
    {
        TrailMaterial = TrailMaterial_;
        ParticleMaterial = ParticleMaterial_;
        EffectSpawnLocation = EffectSpawnLocation_;
        EMS = EMS_;
        Critical = Critical_;
    }
    public void DecreaseHP()
    {
        StatData SD = PostProcessingManager.instance.gameObject.GetComponent<StatData>();
        float DecraseHp_ = SD.CalculatePlayerHit(EMS.MobDefence,Critical);
        EffectManager EM = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
        EM.CreateDmgHit(Critical, DecraseHp_, EffectSpawnLocation);
        EMS.MobHP -= DecraseHp_;
        //shrinkMob
        if (Critical)
        {
            Vector3 Shrink = new Vector3(EMS.gameObject.transform.localScale.x - EMS.BaseScaleSize * 2, EMS.gameObject.transform.localScale.y - EMS.BaseScaleSize * 2, EMS.gameObject.transform.localScale.z - EMS.BaseScaleSize * 2);
            EMS.gameObject.transform.localScale = Shrink;
        }
        else
        {
            Vector3 Shrink = new Vector3(EMS.gameObject.transform.localScale.x - EMS.BaseScaleSize, EMS.gameObject.transform.localScale.y - EMS.BaseScaleSize, EMS.gameObject.transform.localScale.z - EMS.BaseScaleSize);
            EMS.gameObject.transform.localScale = Shrink;
        }
    }

    public IEnumerator OpenSwingSound(float secondWait)
    {
        yield return new WaitForSeconds(secondWait);
        PlayOneShot = true;
        yield return null;
    }
    public IEnumerator OpenCutSound(float secondWait)
    {
        yield return new WaitForSeconds(secondWait);
        CutPoint = true;
        yield return null;
    }
    public void setSpeedStat(float SpeedVar)
    {
        speedStat = SpeedVar;
    }
    public void CalculateVariables()
    {
        Debug.Log("calculating variables");
        for (int i = 0; i < PlayShotMS.Length; i++)
        {
            float TempEdit = PlayShotMS[i];
            TempEdit = TempEdit / speedStat;
            TempPlayShotMS[i] = TempEdit;
        }
        for (int i = 0; i < CutPointMS.Length; i++)
        {
            float TempEdit = CutPointMS[i];
            TempEdit = TempEdit / speedStat;
            TempCutPointMS[i] = TempEdit;
        }
    }
}

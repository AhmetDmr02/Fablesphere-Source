using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class EnemyHitCenter : MonoBehaviour
{
    public float PureDmg, MagicalDmg, PhysicalDmg;
    public float CameraShakeSize;
    public float ParryStunTimeMin, ParryStunTimeMax;
    public bool StunnWhenParried;
    public bool Parried;
    public bool ParryWindow;
    public bool invulnerable = false;
    StatData SD;
    BarManager BM;
    private void Start()
    {
        SD = PostProcessingManager.instance.gameObject.GetComponent<StatData>();
        BM = PostProcessingManager.instance.gameObject.GetComponent<BarManager>();
    }
    public void HitPlayer()
    {
        if (BM.GuardPos)
        {
            float TotalHit = SD.GetHitDamages(PureDmg, MagicalDmg, PhysicalDmg);
            float random = Random.Range(1.0f, 2.0f);
            BM.HP -= TotalHit / random;
            Debug.Log(random);
            EffectManager ES = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
            ES.CreateBlood(FPSController.instance.gameObject.GetComponent<Transform>());
            UISfxManager SFX = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
            SFX.PlayClothHitSound();
            PostProcessingManager.instance.hurtFlash();
            CameraShaker.Instance.ShakeOnce(CameraShakeSize, CameraShakeSize * 1.5f, 0f, 1f);
        }
        else
        {
            float TotalHit = SD.GetHitDamages(PureDmg, MagicalDmg, PhysicalDmg);
            BM.HP -= TotalHit;
            EffectManager ES = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
            ES.CreateBlood(FPSController.instance.gameObject.GetComponent<Transform>());
            UISfxManager SFX = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
            SFX.PlayClothHitSound();
            PostProcessingManager.instance.hurtFlash();
            CameraShaker.Instance.ShakeOnce(CameraShakeSize, CameraShakeSize * 1.5f, 0f, 1f);
        }
    }
}

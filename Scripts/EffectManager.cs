using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject Blood;
    public GameObject EnemyHit;
    public GameObject HealEffect;
    public GameObject ManaEffect;
    public GameObject FloatingNumber;
    public GameObject SparkEffect;
    public GameObject WorldSpaceCanvas;
    public GameObject BlackoutEffect;
    public GameObject GargoyleEffect;
    public float x, y, z;
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
    public void CreateBlood(Transform spawnLocation)
    {
        Instantiate(Blood,new Vector3(spawnLocation.position.x,spawnLocation.position.y + 0.25f,spawnLocation.position.z), Blood.transform.rotation);
    }
    public void CreateHealEffect(Transform spawnLocation,Color color_)
    {
      GameObject GO = Instantiate(HealEffect, new Vector3(spawnLocation.position.x, spawnLocation.position.y, spawnLocation.position.z), HealEffect.transform.rotation);
        ParticleSystem PS = GO.GetComponent<ParticleSystem>();
        ParticleSystemRenderer PSR = PS.GetComponent<ParticleSystemRenderer>();
        if (color_ != Color.red)
        {
            PSR.material.color = color_;
            PSR.trailMaterial.color = color_;
            PSR.material.SetColor("_EmissionColor", color_);
            PSR.trailMaterial.SetColor("_EmissionColor", color_);
        }
    }
    public void CreateManaEffect(Transform spawnLocation)
    {
        Instantiate(ManaEffect, new Vector3(spawnLocation.position.x, spawnLocation.position.y - 0.25f, spawnLocation.position.z), ManaEffect.transform.rotation);
    }
    public void CreateEnemyHit(Vector3 spawnLoc,bool Crit,Material TrailMat,Material ParticleMat)
    {
        if (Crit)
        {
            GameObject PSObject = Instantiate(EnemyHit,spawnLoc, EnemyHit.transform.rotation);
            ParticleSystem PS = PSObject.GetComponent<ParticleSystem>();
            ParticleSystemRenderer PSR = PS.GetComponent<ParticleSystemRenderer>();
            PSR.material = ParticleMat;
            PSR.trailMaterial = TrailMat;
            var emission = PS.emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);
            var main = PS.main;
            bursts[0].count = 45;
            emission.SetBursts(bursts);
            PS.startSize = 0.2f;
            PS.Play();
        }
        else
        {
            GameObject PSObject = Instantiate(EnemyHit, spawnLoc, EnemyHit.transform.rotation);
            ParticleSystem PS = PSObject.GetComponent<ParticleSystem>();
            ParticleSystemRenderer PSR = PS.GetComponent<ParticleSystemRenderer>();
            PSR.material = ParticleMat;
            PSR.trailMaterial = TrailMat;
            var emission = PS.emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);
            var main = PS.main;
            bursts[0].count = 10;
            PS.startSize = 0.1f;
            emission.SetBursts(bursts);
            PS.Play();
        }
    }
    public void CreateDmgHit(bool Crit, float Dmg,Vector3 hitPoint)
    {
        GameObject GO = Instantiate(FloatingNumber, hitPoint,FloatingNumber.transform.rotation);
        TextMeshProUGUI TMPU = GO.GetComponent<TextMeshProUGUI>();
        int NewDmg = (int)Dmg;
        TMPU.text = NewDmg.ToString();
        if (Crit)
        {
            TMPU.color = Color.red;
        }
        GO.transform.SetParent(WorldSpaceCanvas.transform);
    }
    public void CreateSparkEffect(Transform spawnLoc)
    {
        Instantiate(SparkEffect, new Vector3(spawnLoc.position.x + x, spawnLoc.position.y + y, spawnLoc.position.z + z), SparkEffect.transform.rotation);
    }
    public void CreateGargoyleDestroyEffect(Transform spawnLoc)
    {
        Instantiate(GargoyleEffect, new Vector3(spawnLoc.position.x + x, spawnLoc.position.y + y, spawnLoc.position.z + z), GargoyleEffect.transform.rotation);
    }
    public void CreateBlackoutEffect(float Speed,float BlackDuration,bool DontCloseCutsceneAfter,float CloseDelay)
    {
        if (DontCloseCutsceneAfter)
        {
            gameObject.GetComponent<ActivatorManager>().CutsceneActive = true;
        }
        else
        {
            gameObject.GetComponent<ActivatorManager>().CutsceneActive = true;
            Invoke("CloseCutsceneBlackout", CloseDelay);
        }
        var GO = Instantiate(BlackoutEffect);
        GO.GetComponent<BlackoutManager>().Cnv.alpha = 0;
        GO.GetComponent<BlackoutManager>().Speed = Speed;
        GO.GetComponent<BlackoutManager>().Stayblack = BlackDuration;
        GO.GetComponent<BlackoutManager>().Launch();
    }
    public void CloseCutsceneBlackout()
    {
        gameObject.GetComponent<ActivatorManager>().CutsceneActive = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BeholderGuard : MonoBehaviour,IMobSerializer,IMapGetSerializer
{
    EnemyMainStat EMS;
    public float PureDmg, MagicalDmg, PhysicalDmg;
    private Animator AnimManager;
    public LayerMask WhatIsPlayer, WhatIsGround;
    public NavMeshAgent NMA;
    public float StunTime;
    public float Stunned;
    public float AttackDuration;
    public float AttackCooldown;
    public float OurHP;
    private GameObject Target;
    public List<Transform> PatrolLocations = new List<Transform>();
    private Transform OldTransform;
    public ParticleSystem Beam;
    public AudioSource MonsterSound;
    [Header("SFX")]
    public AudioClip[] General;
    public AudioClip[] Die_Sound;
    public AudioClip[] Spot;
    [Header("Detection")]
    public float AttackRange;
    public Transform FovTransform;
    public float Radius;
    [HideInInspector]
    public bool CheckingForPlayer;
    public bool CanSeePlayer;
    [Range(0,360)]
    public float Angle;
    public float YAxisBoost;
    public GameObject playerRef;
    private bool serialized;
    public enum States
    {
        Patrolling,
        Chasing,
        Attacking,
        Died
    }
    public States enemyState;
    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
        AnimManager = gameObject.GetComponent<Animator>();
        EMS = this.gameObject.GetComponent<EnemyMainStat>();
        OurHP = EMS.MobHP;
        NMA.stoppingDistance = AttackRange;
        Invoke("PlayRandomSound", 5f);
        StartCoroutine(FOVRoutine());
    }
    private bool CallToday;
    private bool StopAttackingBool;
    private float AttackRate;
    private bool DiedBool;
    private bool OpenBeamWhenSee;
    public void Update()
    {
        if (AttackRate > 0)
        {
            AttackRate -= 1 * Time.deltaTime;
        }
        if (this.playerRef == null)
        {
            playerRef = FPSController.PlayerObject;
        }
        if (this.enemyState != States.Died)
        {
            if (OurHP != EMS.MobHP)
            {
                OurHP = EMS.MobHP;
                TakeDmg(EMS.emsCritic);
            }
            if (Stunned > 0)
            {
                Stunned -= 1 * Time.deltaTime;
                AnimManager.SetBool("Stunned", true);
                Beam.Stop();
            }
            else
            {
                Stunned = 0;
                AnimManager.SetBool("Stunned", false);
            }
            if (OurHP <= 0)
            {
                this.enemyState = States.Died;
                if (!DiedBool)
                {
                    DiedBool = true;
                    Die();
                }
            }
        }
        if (this.enemyState == States.Patrolling)
        {
            if (OldTransform == null)
            {
                if (PatrolLocations.Count > 0)
                {
                    OldTransform = PatrolLocations[0]; //for call void one time
                    SetTransformLocation();
                }
            }
            else
            {
                if (!CallToday)
                {
                    CallToday = true;
                    int RandomWait = Random.Range(3, 15);
                    Invoke("SetTransformLocation", (float)RandomWait);
                    Debug.Log("settransformlocation");
                   //Transform RealT = SetTransformLocation();
                   // OldTransform = RealT;
                     //get transform with delay
                }
            }
            
        }
        if (this.enemyState == States.Chasing)
        {
            StopCoroutine(FOVRoutine());
            NMA.SetDestination(Target.transform.position);
            var lookPos = Target.transform.position - this.transform.position;
            lookPos.y = 0;
            var _rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 5f);
            if (NMA.remainingDistance < AttackRange && this.enemyState != States.Attacking && NMA.remainingDistance != 0)
            {
                if (this.enemyState != States.Attacking)
                {
                    this.enemyState = States.Attacking;
                    AttackPlayer();
                    
                }
                 NMA.ResetPath();
            }
          //  Debug.Log(NMA.remainingDistance);
        }
        if (this.enemyState == States.Attacking)
        {
            if (CanSeePlayer && Stunned <= 0)
            {
                if (OpenBeamWhenSee)
                {
                    this.OpenBeamWhenSee = false;
                    this.Beam.Play();
                }
            }
            else
            {
                this.Beam.Stop();
                this.OpenBeamWhenSee = true;
            }
            var lookPos = Target.transform.position - this.transform.position;
            lookPos.y = 0;
            var _rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 15f);
            if (ReadyToBeamBool)
            {
                if (CanSeePlayer)
                {
                    if (AttackRate <= 0)
                    {
                        AttackRate += 0.1f;
                        if (!StopAttackingBool)
                        {
                            StopAttackingBool = true;
                            Invoke("StopAttacking", AttackDuration);
                        }
                        if (this.EMS.EHC.invulnerable) return;
                        EMS.EHC.HitPlayer();
                    }
                }else
                {
                    if (AttackRate <= 0)
                    {
                        AttackRate += 0.1f;
                        if (!StopAttackingBool)
                        {
                            StopAttackingBool = true;
                            Invoke("StopAttacking", AttackDuration);
                        }
                    }
                }
            }
        }
        if (this.Stunned > 0)
        {
            NMA.speed = 0;
        }
        else
        {
            NMA.speed = 4f;
        }

        if (Target == null && this.enemyState != States.Died && Stunned <= 0 && this.enemyState != States.Attacking)
        {
            enemyState = States.Patrolling;
        }
        else if(Stunned <= 0 && this.enemyState != States.Died && this.enemyState != States.Attacking && CanSeePlayer)
        {   
            this.enemyState = States.Chasing;
        }
    }
    public void SetTransformLocation()
    {
        if (PatrolLocations.Count <= 0)
        {
            CallToday = false;
            return;
        }
        Transform T = PatrolLocations[Random.Range(0, PatrolLocations.Count)];
        while (OldTransform == T)
        {
            T = PatrolLocations[Random.Range(0, PatrolLocations.Count)];
        }
        if (enemyState == States.Patrolling)
        {
            NMA.SetDestination(T.position);
            OldTransform = T;
            Debug.Log("Set T");
        }
        CallToday = false;
    }
    public void TakeDmg(bool Critical)
    {
        if (Critical)
        {
            Stunned = StunTime;
            if (this.enemyState == States.Attacking)
            {
                Target = null;
                this.enemyState = States.Patrolling;
                AnimManager.Play("Dizzy");
            }
        }
        else
        {
            if (Stunned <= 0)
            {
                if (this.enemyState != States.Attacking)
                {
                    AnimManager.Play("GetHit");
                    this.enemyState = States.Chasing;
                }
            }
        }
        this.Target = playerRef.gameObject;

    }
    public void Die()
    {
        Debug.Log("Died");
        AnimManager.Play("Die");
        this.gameObject.AddComponent<DestroyMe>().DestroyTime = 60f;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        this.Beam.gameObject.SetActive(false);
        EMS.DestroyBar();
        MonsterSound.PlayOneShot(Die_Sound[Random.Range(0, Die_Sound.Length)], 1);
    }
    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Vector3 NewVector3 = new Vector3(this.FovTransform.transform.position.x, this.FovTransform.transform.position.y + YAxisBoost, this.FovTransform.transform.position.z);
        Collider[] rangeChecks = Physics.OverlapSphere(NewVector3, Radius, WhatIsPlayer);
        if (rangeChecks.Length != 0)
        {
            for (int i = 0; i < rangeChecks.Length; i++)
            {
                if (rangeChecks[i].GetComponent<FPSController>() != null)
                {
                    Transform target = rangeChecks[i].transform;
                    Vector3 directionToTarget = (target.position - FovTransform.transform.position).normalized;
                    if (Vector3.Angle(FovTransform.transform.forward, directionToTarget) < Angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(FovTransform.transform.position, target.position);
                        if (!Physics.Raycast(NewVector3, directionToTarget, distanceToTarget, WhatIsGround))
                        {
                            CanSeePlayer = true;
                            Target = rangeChecks[i].gameObject;
                        }
                        else
                            CanSeePlayer = false;

                    }
                    else
                    {
                        CanSeePlayer = false;
                    }
                }
            } 
        }
        else if (CanSeePlayer)
            CanSeePlayer = false;
    }
    public void AttackPlayer()
    {
        MonsterSound.PlayOneShot(Spot[Random.Range(0, General.Length)], 1);
        AnimManager.Play("Attack02ST");
        Invoke("ReadyToBeam", 0.25f);
    }
    private bool ReadyToBeamBool;
    public void ReadyToBeam()
    {
        ReadyToBeamBool = true;
        this.Beam.Play();
    }
    public void StopAttacking()
    {
        AnimManager.SetBool("AttackDone", true);
        Stunned += AttackCooldown;
        this.enemyState = States.Patrolling;
        Beam.Stop();
        StopAttackingBool = false;
        ReadyToBeamBool = false;
    }
    public void PlayRandomSound()
    {
        if (this.enemyState != States.Died)
        {
            Invoke("PlayRandomSound", Random.Range(10, 50));
            MonsterSound.PlayOneShot(General[Random.Range(0, General.Length)], 1);
        }
    }
    public void setPatrolLocation(Transform[] t)
    {
        foreach (Transform Tr in t)
        {
            PatrolLocations.Add(Tr);
        }
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        AnimManager = gameObject.GetComponent<Animator>();
        EMS = this.gameObject.GetComponent<EnemyMainStat>();
        this.EMS.MobDefence = SIC.serialFloatList[0];
        this.EMS.MobHP = SIC.serialFloatList[1];
        this.EMS.MobMaxHP = SIC.serialFloatList[2];
        this.EMS.BaseScaleSize = SIC.serialFloatList[3];
        this.EMS.LevelScaleRate = SIC.serialFloatList[4];
        this.EMS.Level = SIC.serialIntList[0];
        this.EMS.EHC.PureDmg = SIC.serialFloatList[5];
        this.EMS.EHC.MagicalDmg = SIC.serialFloatList[6];
        this.EMS.EHC.PhysicalDmg = SIC.serialFloatList[7];
        this.EMS.EHC.CameraShakeSize = SIC.serialFloatList[8];
        this.EMS.EHC.ParryStunTimeMin = SIC.serialFloatList[9];
        this.EMS.EHC.ParryStunTimeMax = SIC.serialFloatList[10];
        this.StunTime = SIC.serialFloatList[11];
        this.AttackRange = SIC.serialFloatList[12];
        this.Radius = SIC.serialFloatList[13];
        this.Angle = SIC.serialFloatList[14];
        this.AttackCooldown = SIC.serialFloatList[15];
        this.EMS.MobName = SIC.serialStringList[0];
        this.EMS.EHC.StunnWhenParried = SIC.serialBoolList[0];
        foreach (Vector3 vec in SIC.serialVec3List)
        {
            GameObject G = new GameObject();
            G.transform.position = vec;
            PatrolLocations.Add(G.transform);
        }
    }
    public void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        if (this.enemyState == States.Died) return;
        this.EMS.EHC.invulnerable = true;
        SerializationInfoClass SIC = new SerializationInfoClass();
        if (this.gameObject.GetComponent<GeneratedObject>() != null)
        {
            SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
            SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
            SIC.dontDeleteThis = false;
            SIC.generatedBySomething = true;
            SIC.serialQuaternionSingle = this.transform.rotation;
            SIC.serialVec3Single = this.transform.position;
            SIC.serialFloatList.Add(this.EMS.MobDefence);
            SIC.serialFloatList.Add(this.EMS.MobHP);
            SIC.serialFloatList.Add(this.EMS.MobMaxHP);
            SIC.serialFloatList.Add(this.EMS.BaseScaleSize);
            SIC.serialFloatList.Add(this.EMS.LevelScaleRate);
            SIC.serialIntList.Add(this.EMS.Level);
            SIC.serialFloatList.Add(this.EMS.EHC.PureDmg);
            SIC.serialFloatList.Add(this.EMS.EHC.MagicalDmg);
            SIC.serialFloatList.Add(this.EMS.EHC.PhysicalDmg);
            SIC.serialFloatList.Add(this.EMS.EHC.CameraShakeSize);
            SIC.serialFloatList.Add(this.EMS.EHC.ParryStunTimeMin);
            SIC.serialFloatList.Add(this.EMS.EHC.ParryStunTimeMax);
            SIC.serialFloatList.Add(this.StunTime);
            SIC.serialFloatList.Add(this.AttackRange);
            SIC.serialFloatList.Add(this.Radius);
            SIC.serialFloatList.Add(this.Angle);
            SIC.serialFloatList.Add(this.AttackCooldown);
            SIC.serialStringList.Add(this.EMS.MobName);
            SIC.serialBoolList.Add(this.EMS.EHC.StunnWhenParried);
            foreach (Transform t in PatrolLocations)
            {
                SIC.serialVec3List.Add(t.position);
            }
            SIC.serialAssetGameObject.Add(this.gameObject.GetComponent<GeneratedObject>()._objInAssets);
        }
        else
        {
            SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
            SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
            SIC.dontDeleteThis = false;
            SIC.generatedBySomething = false;
            SIC.serialFloatList.Add(this.EMS.MobDefence);
            SIC.serialFloatList.Add(this.EMS.MobHP);
            SIC.serialFloatList.Add(this.EMS.MobMaxHP);
            SIC.serialFloatList.Add(this.EMS.BaseScaleSize);
            SIC.serialFloatList.Add(this.EMS.LevelScaleRate);
            SIC.serialFloatList.Add(this.EMS.Level);
            SIC.serialFloatList.Add(this.EMS.EHC.PureDmg);
            SIC.serialFloatList.Add(this.EMS.EHC.MagicalDmg);
            SIC.serialFloatList.Add(this.EMS.EHC.PhysicalDmg);
            SIC.serialFloatList.Add(this.EMS.EHC.CameraShakeSize);
            SIC.serialFloatList.Add(this.EMS.EHC.ParryStunTimeMin);
            SIC.serialFloatList.Add(this.EMS.EHC.ParryStunTimeMax);
            SIC.serialFloatList.Add(this.StunTime);
            SIC.serialFloatList.Add(this.AttackRange);
            SIC.serialFloatList.Add(this.Radius);
            SIC.serialFloatList.Add(this.Angle);
            SIC.serialFloatList.Add(this.AttackCooldown);
            SIC.serialStringList.Add(this.EMS.MobName);
            SIC.serialBoolList.Add(this.EMS.EHC.StunnWhenParried);
            foreach (Transform t in PatrolLocations)
            {
                SIC.serialVec3List.Add(t.position);
            }
        }
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
}

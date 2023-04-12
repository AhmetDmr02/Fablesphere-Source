using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using EZCameraShake;

public class DemonBullBeh : MonoBehaviour,IMapGetSerializer
{
    EnemyMainStat EMS;
    private Animator AnimManager;
    public LayerMask WhatIsPlayer, WhatIsGround;
    public UnityEngine.AI.NavMeshAgent NMA;
    public float StunTime;
    public float Stunned;
    public float OurHP;
    private GameObject Target;
    public List<Transform> PatrolLocations = new List<Transform>();
    public AudioSource MonsterSound;
    [Header("SFX")]
    public AudioClip[] General;
    public AudioClip[] Die_Sound;
    public AudioClip[] Spot;
    public AudioClip[] BiteSound;
    public AudioClip[] Footsteps;
    public AudioSource[] FootstepSources;
    public float FootStepInvertal;
    [Header("Detection")]
    public float AttackRange;
    public Transform FovTransform;
    public float Radius;
    [HideInInspector]
    public bool CheckingForPlayer;
    public bool CanSeePlayer;
    [Range(0, 360)]
    public float Angle;
    public float YAxisBoost;
    public GameObject playerRef;
    DataItem item;
    private Transform OldTransform;
    private bool serialized;
    [SerializeField] private GameObject footstepSound;
    public enum States
    {
        Patrolling,
        Chasing,
        Attacking,
        Stunned,
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
    public float AttackCooldown;
    private float attack_Cooldown;
    private bool DiedBool;
    private bool LocalParried;
    private bool AttackParried;
    public void Update()
    {
        if (EMS.EHC.Parried != LocalParried)
        {
            if (!AttackParried) AttackParried = EMS.EHC.Parried;
            EMS.EHC.Parried = false;
            float randomStun = (float)Random.Range((int)EMS.EHC.ParryStunTimeMin, (int)EMS.EHC.ParryStunTimeMax + 1);
            Stunned = randomStun;
            Target = null;
            AnimManager.Play("SleepStart");
            AnimManager.SetBool("Stunned", true);
            this.enemyState = States.Stunned;
            CancelInvoke("AttackPlayer");
            CancelInvoke("ClearAnim");
        }
        if (attack_Cooldown > 0)
        {
            attack_Cooldown -= 1 * Time.deltaTime;
            ReadyToAttackBool = false;
        }
        else if (attack_Cooldown <= 0)
        {
            ReadyToAttackBool = true;
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
                if (NMA.remainingDistance < 0.300f && !CallToday)
                {
                    CallToday = true;
                    int RandomWait = Random.Range(3, 15);
                    Invoke("SetTransformLocation", (float)RandomWait);
                    //Transform RealT = SetTransformLocation();
                    // OldTransform = RealT;
                    //get transform with delay
                }
            }

        }
        if (this.enemyState == States.Chasing)
        {
            if (NMA.remainingDistance < AttackRange)
            {
                AnimManager.SetBool("Chasing", false);
            }
            else
            {
                AnimManager.SetBool("Chasing", true);
            }
            StopCoroutine(FOVRoutine());
            NMA.SetDestination(Target.transform.position);
            var lookPos = Target.transform.position - this.transform.position;
            lookPos.y = 0;
            var _rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 5f);
            if (NMA.remainingDistance < AttackRange && this.enemyState != States.Attacking && NMA.remainingDistance != 0)
            {
                if (this.enemyState != States.Attacking && ReadyToAttackBool)
                {
                    this.enemyState = States.Attacking;
                }
                NMA.ResetPath();
            }
            //  Debug.Log(NMA.remainingDistance);
        }
        else
        {
            AnimManager.SetBool("Chasing", false);
        }
        if (this.enemyState == States.Attacking)
        {
            var lookPos = Target.transform.position - this.transform.position;
            lookPos.y = 0;
            var _rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 15f);
            if (ReadyToAttackBool)
            {
                if (CanSeePlayer)
                {
                    if (attack_Cooldown <= 0)
                    {
                        AttackPlayer();
                        attack_Cooldown += AttackCooldown;
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
            NMA.speed = 7f;
        }
        if (Target == null && this.enemyState != States.Died && Stunned <= 0 && this.enemyState != States.Attacking)
        {
            enemyState = States.Patrolling;
        }
        else if (Stunned <= 0 && this.enemyState != States.Died && this.enemyState != States.Attacking && CanSeePlayer)
        {
            this.enemyState = States.Chasing;
        }
    }
    public void SetTransformLocation()
    {
        Debug.Log("Setting Transform");
        if (PatrolLocations.Count <= 0)
        {
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
        }
        CallToday = false;
    }
    bool WokeUp;
    public void TakeDmg(bool Critical)
    {
        if (Critical)
        {
            Stunned = StunTime;
            if (this.enemyState == States.Attacking)
            {
                Target = null;
                this.enemyState = States.Patrolling;
                //Golem is resistive aganist stunning
            }
        }
        else
        {
            if (Stunned <= 0)
            {
                if (this.enemyState != States.Attacking)
                {
                    this.enemyState = States.Chasing;
                }
            }
        }
        if(this.enemyState != States.Attacking && this.enemyState != States.Stunned) AnimManager.Play("Damage",-1,0);
        this.Target = playerRef.gameObject;
    }
    public void Die()
    {
        int i = Random.Range(0, 101);
        if (i <= 20)
        {
            PickableItemCreator.instance.CreatePickUpAbleItem(44, 1, this.transform, 1);
        }else if (i > 20 && i <= 40)
        {
            PickableItemCreator.instance.CreatePickUpAbleItem(38, 1, this.transform, 1);
        }
        Debug.Log("Died");
        AnimManager.SetBool("Died",true);
        AnimManager.Play("Die");
        this.gameObject.AddComponent<DestroyMe>().DestroyTime = 60f;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        EMS.DestroyBar();
        if (Die_Sound.Length >= 1)
        {
            MonsterSound.PlayOneShot(Die_Sound[Random.Range(0, Die_Sound.Length)], 1);
        }
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
        AttackParried = false;
        attack_Cooldown += AttackCooldown;
        AnimManager.Play("Hit");
        Invoke("ClearAnim", 0.5f);
        try
        {
            MonsterSound.PlayOneShot(BiteSound[Random.Range(0, BiteSound.Length)], 1);
        }
        catch (System.Exception err)
        {
            print(err);
        }

    }
    private void ClearAnim()
    {
        if (AttackParried) return;
        if (this.EMS.EHC.invulnerable) return;
        if (Spot.Length > 1)
        {
            MonsterSound.PlayOneShot(Spot[Random.Range(0, General.Length)], 1);
        }
        EMS.EHC.HitPlayer();
        this.enemyState = States.Chasing;
    }
    private bool ReadyToAttackBool;
    private void OpenParryWindow()
    {
        EMS.EHC.ParryWindow = true;
    }
    private void CloseParryWindow()
    {
        EMS.EHC.ParryWindow = false;
    }
    public void PlayRandomSound()
    {
        if (this.enemyState != States.Died)
        {
            Invoke("PlayRandomSound", Random.Range(10, 50));
            if (General.Length >= 1)
            {
                MonsterSound.PlayOneShot(General[Random.Range(0, General.Length)], 0.35f);
            }
        }
    }
    public void CreateShakeForRunning()
    {
        CameraShaker.Instance.ShakeOnce(8, 8, 0, 0.5f);
        GameObject GO = Instantiate(footstepSound, this.transform.position, this.transform.rotation);
        GO.transform.parent = this.transform;
        //Shaking And Sound
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
        this.item = new DataItem();
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

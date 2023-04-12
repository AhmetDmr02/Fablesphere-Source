using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using HelperOfDmr;
using TMPro;
using EZCameraShake;
public class LavaGolemMain : MonoBehaviour
{
    EnemyMainStat EMS;
    UISfxManager USM;
    public GameObject ModuleObject;
    public float PureDmg, MagicalDmg, PhysicalDmg;
    private Animator AnimManager;
    public LayerMask WhatIsPlayer, WhatIsGround;
    public NavMeshAgent NMA;
    public float StunTime;
    public float Stunned;
    public float OurHP;
    [SerializeField] float maxPatternCooldown;
    private float PatternCooldown;
    private GameObject Target;
    public Transform[] PatrolLocations;
    private Transform OldTransform;
    public AudioSource MonsterSound;
    [SerializeField] private ParticleSystem TeleportParticle;
    [Header("SFX")]
    public AudioClip[] General;
    public AudioClip[] Die_Sound;
    [SerializeField] private AudioSource TeleportSound;
    [SerializeField] private Transform particleSpawner;
    [SerializeField] private GameObject LavaGolemSpawner, LavaGolemRock, LavaGolemMeteorSound;
    [SerializeField] private GameObject LavaBeam;
    [Header("Detection")]
    public float AttackRange;
    public GameObject playerRef;
    public enum States
    {
        WaitingPlayer,
        Teleporting,
        Stunned,
        DetermineAttack,
        Attacking_Spelling,
        Preattacking_Chasing,
        Attacking_Chasing,
        Died
    }
    public States enemyState;
    private void Start()
    {
        AnimManager = gameObject.GetComponent<Animator>();
        EMS = this.gameObject.GetComponent<EnemyMainStat>();
        OurHP = EMS.MobHP;
        NMA.stoppingDistance = AttackRange;
    }
    private bool CallToday;
    public float AttackCooldown;
    private float attack_Cooldown;
    private bool DiedBool;

    [SerializeField] private bool Determined;
    private int PunchCounter = 3; 
    public void Update()
    {

        #region definitionOfPlayer
        if (this.playerRef == null)
        {
            playerRef = FPSController.PlayerObject;
        }
        if (Target == null)
        {
            Target = playerRef;
        }
        #endregion

        #region Cooldown Stuff

        if (USM == null)
        {
            USM = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
        }
        if (PatternCooldown > 0)
        {
            PatternCooldown -= 1 * Time.deltaTime;
        }
        if (attack_Cooldown > 0)
        {
            attack_Cooldown -= 1 * Time.deltaTime;
        }
        #endregion


        if (this.enemyState == States.DetermineAttack)
        {
            var lookPos = Target.transform.position - this.transform.position;
            lookPos.y = 0;
            var _rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 2f);
            if (PatternCooldown <= 0)
            {
                if (!Determined)
                {
                    float distanceToPlayer = Vector3.Distance(this.transform.position, playerRef.transform.position);
                    Debug.Log(distanceToPlayer);
                    if (EMS.MobHP > EMS.MobMaxHP / 2)
                    {
                        //Mild Attacks
                        if (distanceToPlayer > 40)
                        {
                            //far away             
                            Determined = true;
                            int randomRotationInt = Random.Range(1, 3);
                            Debug.Log(randomRotationInt + "++");
                            int randomInt = Random.Range(10, 15);
                            StartCoroutine(MeteoriteHell(randomInt, 0.20f));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.22f, 1, 6, 0));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.24f, 1, -6, 0));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.26f, 1, 0, -6));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.20f, 1, 0, 6));
                            PatternCooldown += maxPatternCooldown;
                            PatternCooldown += randomInt * 0.26f;
                            if (randomRotationInt == 1)
                            {
                                lateTeleport((float)(randomInt * 0.26f + 1.2f));
                            }
                            randomRotationInt -= 1;
                            if (randomRotationInt == 0)
                            {
                                Invoke("ClearDetermination", (float)(randomInt * 0.26f + 1 + 5));
                            }
                            else
                            {
                                StartCoroutine(FirstRotativeAttack(randomRotationInt));
                            }
                        }
                        if (distanceToPlayer <= 40 && distanceToPlayer > 10)
                        {
                            Determined = true;
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.02f, 1.25f, 4, 0));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.12f, 1.25f, 4, 4));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.10f, 1.25f, -4, -4));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.08f, 1.25f, -4, 0));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.06f, 1.25f, 0, -4));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.04f, 1.25f, 0, 4));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.00f, 1.2f, 0, 0));
                            PatternCooldown += maxPatternCooldown;
                            PatternCooldown += 1 * 0.26f;
                            lateTeleport(1.1f);

                        }
                        if (distanceToPlayer <= 10)
                        {
                            this.enemyState = States.Attacking_Chasing;
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.00f, 1f, 0, 0));
                        }
                    }
                    else
                    {
                        if (distanceToPlayer > 40)
                        {
                            //far away 
                            Determined = true;
                            int randomInt = Random.Range(25, 55);
                            StartCoroutine(MeteoriteHellSpeed(randomInt, 0.20f,1.5f));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.22f, 0.7f, 6, 0));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.24f, 0.7f, -6, 0));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.26f, 0.7f, 0, -6));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.20f, 0.7f, 0, 6));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.20f, 0.5f, 0, 0));
                            PatternCooldown += maxPatternCooldown;
                            PatternCooldown += randomInt * 0.26f;
                            Debug.Log("calling determinate");
                            Invoke("ClearDetermination", (float)(randomInt * 0.26f + 1 + 5));
                        }
                        if (distanceToPlayer <= 40 && distanceToPlayer > 10)
                        {
                            Determined = true;
                            Debug.Log("below 40 spikemore and tel");
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.02f, 1.25f, 4, 0));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.12f, 1.25f, 4, 4));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.10f, 1.25f, -4, -4));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.08f, 1.25f, -4, 0));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.06f, 1.25f, 0, -4));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.04f, 1.25f, 0, 4));
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.00f, 1.2f, 0, 0));
                            PatternCooldown += maxPatternCooldown;
                            PatternCooldown += 1 * 0.26f;
                            lateTeleport(1.1f);

                        }
                        if (distanceToPlayer <= 10)
                        {
                            Debug.Log("below 10 chase");
                            this.enemyState = States.Attacking_Chasing;
                            StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.00f, 1f, 0, 0));
                        }
                    }
                }
            }
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


        #region Warming Attack
        if (this.enemyState == States.Preattacking_Chasing)
        {
            if (NMA.remainingDistance <= AttackRange)
            {
                AnimManager.SetBool("Chasing", false);
            }
            else
            {
                AnimManager.SetBool("Chasing", true);
                AnimManager.Play("RunNonMix");
            }
            NMA.SetDestination(Target.transform.position);
            var lookPos = Target.transform.position - this.transform.position;
            lookPos.y = 0;
            var _rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 2f);
        }
        #endregion

        #region if lava golem starts to chase player
        if (this.enemyState == States.Attacking_Chasing)
        {
            if (attack_Cooldown <= 0)
            {
                if (NMA.remainingDistance <= AttackRange)
                {
                    AnimManager.SetBool("Chasing", false);
                    if (PunchCounter == 0)
                    {
                        if (attack_Cooldown <= 0)
                        {
                            attack_Cooldown += AttackCooldown;
                            PunchCounter = 3;
                            this.enemyState = States.Attacking_Spelling;
                            AnimManager.Play("Standing Taunt Battlecry");
                            StartupKnockplayerBack();
                            Invoke("SetDetermination", 1.2f);
                            Invoke("ClearDetermination", 1.2f);
                        }
                        //make next attack other than chasing
                    }
                    else
                    {
                        if (attack_Cooldown <= 0)
                        {
                            AttackPlayer();
                            attack_Cooldown += AttackCooldown;
                            PunchCounter -= 1;
                        }
                    }
                }
                else
                {
                    AnimManager.SetBool("Chasing", true);
                    AnimManager.Play("RunNonMix");
                }
            }
            NMA.SetDestination(Target.transform.position);
            var lookPos = Target.transform.position - this.transform.position;
            lookPos.y = 0;
            var _rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 2f);
        }
        else if(this.enemyState != States.Preattacking_Chasing)
        {
            if (this.enemyState != States.Died)
            {
                AnimManager.SetBool("Chasing", false);
                NMA.ResetPath();
            }
        }
        #endregion

        #region if lava golem stunned
        if (this.Stunned > 0)
        {
            NMA.speed = 0;
            this.enemyState = States.Stunned;
        }
        else
        {
            NMA.speed = 6f;
            if (this.enemyState == States.Stunned)
            {
                this.enemyState = States.Attacking_Spelling;
                AnimManager.SetBool("Stunned", false);
                AnimManager.Play("Standing Taunt Battlecry");
                StartupKnockplayerBack();
                Invoke("SetDetermination", 1.2f);
                Invoke("ClearDetermination", 1.2f);
            }
        }
        #endregion

    }

    void testAttack()
    {
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.02f, 1.25f, 4, 0));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.12f, 1.25f, 4, 4));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.10f, 1.25f, -4, -4));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.08f, 1.25f, -4, 0));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.06f, 1.25f, 0, -4));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.04f, 1.25f, 0, 4));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(0.04f, 1.2f, 0, 0));
    }

    void lateTeleport(float delay)
    {
        Invoke("TeleportAttack", delay);
    }

    #region FastMeteoriteHell
    IEnumerator MeteoriteHellSpeed(int Count, float delayBetween,float SpeedTemp)
    {
        AnimManager.SetBool("SkillOneUsing", true);
        for (int i = 0; i < Count; i++)
        {
            SpeedTemp = LavaGolemRock.gameObject.GetComponent<LavaGolemRock>().Speed;
            SpeedTemp = SpeedTemp * 2;
            SpawnMeteor(SpeedTemp);
            yield return new WaitForSecondsRealtime(delayBetween);
        }
        yield return null;
    }
    #endregion

    #region Teleportation Attack
    public void TeleportAttack()
    {
        AnimManager.Play("Teleport");
        Invoke("Dissapear", 1.37f);
        AnimManager.SetBool("SkillOneUsing",false);
        AnimManager.SetBool("SkillTwoUsing", false);
    }

    public void Dissapear()
    {
        TeleportSound.Play();
        GameObject GO = Instantiate(TeleportParticle.gameObject, this.particleSpawner.position, TeleportParticle.transform.rotation).gameObject;
        GO.GetComponent<ParticleSystem>().Play();
        CameraShaker.Instance.ShakeOnce(2, 2, 0, 2);
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = false;
        Invoke("ReappearAtPlayerPos", 2f);
    }
    private Vector3 StartPos = new Vector3(359.14f, 26.14455f, 789.054f);
    private int FailureCalculator;
    public void ReappearAtPlayerPos()
    {
        Vector3 SetPosVec3 = new Vector3(0, 0, 0);
        int RandomI = Random.Range(0, 100);
        if (RandomI > 49)
        {
            //Right Or Left
            int RandomR = Random.Range(0, 100);
            if (RandomR > 49)
            {
                //Right
                SetPosVec3 = new Vector3(playerRef.transform.position.x, playerRef.transform.position.y + 15, playerRef.transform.position.z + 5);
            }
            else
            {
                //Left
                SetPosVec3 = new Vector3(playerRef.transform.position.x, playerRef.transform.position.y + 15, playerRef.transform.position.z - 5);
            }
        }
        else
        {
            //Forward Or Backward
            int RandomR = Random.Range(0, 100);
            if (RandomR > 49)
            {
                //Forward
                SetPosVec3 = new Vector3(playerRef.transform.position.x + 5, playerRef.transform.position.y + 15, playerRef.transform.position.z);
            }
            else
            {
                //Backward
                SetPosVec3 = new Vector3(playerRef.transform.position.x - 5, playerRef.transform.position.y + 15, playerRef.transform.position.z);
            }
        }
        RaycastHit hit;
        float distance = 100f;
        if (Physics.Raycast(SetPosVec3, Vector3.down, out hit, distance))
        {
            if (hit.transform.gameObject.layer != 7)
            {
                if (FailureCalculator < 6)
                {
                    Debug.Log("couldn't make it");
                    Invoke("ReappearAtPlayerPos", 1f);
                    FailureCalculator += 1;
                }
                else
                {
                    FailureCalculator = 0;
                    this.gameObject.transform.position = StartPos;
                    Debug.Log("task failed");
                    GameObject GO = Instantiate(TeleportParticle.gameObject, this.particleSpawner.position, TeleportParticle.transform.rotation).gameObject;
                    GO.GetComponent<ParticleSystem>().Play();
                    CameraShaker.Instance.ShakeOnce(2, 2, 0, 2);
                    this.gameObject.GetComponent<BoxCollider>().enabled = true;
                    this.gameObject.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = true;
                    this.enemyState = States.DetermineAttack;
                }

            }
            else
            {
                Vector3 FixedVectorForLava = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                this.gameObject.transform.position = FixedVectorForLava;
                Debug.Log("it's worked");
                TeleportSound.Play();
                GameObject GO = Instantiate(TeleportParticle.gameObject, this.particleSpawner.position, this.TeleportParticle.transform.rotation).gameObject;
                GO.GetComponent<ParticleSystem>().Play();
                CameraShaker.Instance.ShakeOnce(2, 2, 0, 2);
                this.gameObject.GetComponent<BoxCollider>().enabled = true;
                this.gameObject.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = true;
                WarmAttack();
                //Add 3 Punch Then Push Attack
            }
        }
        else
        {
            if (FailureCalculator < 6)
            {
                Debug.Log("couldn't make it");
                Invoke("ReappearAtPlayerPos", 1f);
                FailureCalculator += 1;
            }
            else
            {
                FailureCalculator = 0;
                this.gameObject.transform.position = StartPos;
                Debug.Log("task failed");
                GameObject GO = Instantiate(TeleportParticle.gameObject, this.particleSpawner.position, TeleportParticle.transform.rotation).gameObject;
                GO.GetComponent<ParticleSystem>().Play();
                CameraShaker.Instance.ShakeOnce(2, 2, 0, 2);
                this.gameObject.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = true;
                this.gameObject.GetComponent<BoxCollider>().enabled = true;
                this.enemyState = States.DetermineAttack;
            }
        }
    }
    #endregion

    #region meteoritepart
    IEnumerator MeteoriteHell(int Count,float delayBetween)
    {
        AnimManager.SetBool("SkillOneUsing", true);
        for (int i = 0; i < Count; i++)
        {
            SpawnMeteor(0);
            yield return new WaitForSecondsRealtime(delayBetween);
        }
        StopCoroutine(MeteoriteHell(Count,delayBetween));
    }

    #endregion

    #region Lava Spikes
    IEnumerator SpawnLavaSpikeAttackPlayerDynamic(float InvokeBeforeStart, float CooldownDelay, float PlayerShiftX,float PlayerShiftZ)
    {
        yield return new WaitForSecondsRealtime(InvokeBeforeStart);
        AnimManager.SetBool("SkillTwoUsing", true);
        AnimManager.SetBool("SkillOneUsing", false);
        if (CooldownDelay == 0)
        {
            Vector3 Pos = new Vector3(playerRef.transform.position.x + PlayerShiftX, LavaBeam.transform.position.y, playerRef.transform.position.z + PlayerShiftZ);
            Instantiate(LavaBeam, Pos, LavaBeam.transform.rotation);
            yield return null;
            AnimManager.SetBool("SkillTwoUsing", false);
        }
        else
        {
            Vector3 Pos = new Vector3(playerRef.transform.position.x + PlayerShiftX, LavaBeam.transform.position.y, playerRef.transform.position.z + PlayerShiftZ);
            GameObject GO = Instantiate(LavaBeam, Pos, LavaBeam.transform.rotation);
            GO.GetComponent<LavaSpike>().DelayForSpike = CooldownDelay;
            AnimManager.SetBool("SkillTwoUsing", false);
            yield return null;
        }
    }
    IEnumerator SpawnLavaSpikeAttackStatic(float InvokeBeforeStart,float CooldownDelay, Vector3 Pos)
    {
        yield return new WaitForSecondsRealtime(InvokeBeforeStart);
        AnimManager.SetBool("SkillTwoUsing", true);
        AnimManager.SetBool("SkillOneUsing", false);
        if (CooldownDelay == 0)
        {
            Instantiate(LavaBeam, Pos, LavaBeam.transform.rotation);
            yield return null;
        }
        else
        {
            GameObject GO = Instantiate(LavaBeam, Pos, LavaBeam.transform.rotation);
            GO.GetComponent<LavaSpike>().DelayForSpike = CooldownDelay;
            yield return null;
        }
    }

    #endregion

    #region 40 HP ROTATIVE
    IEnumerator FirstRotativeAttack(int loopCount)
    {
        if (PatternCooldown > 0)
            yield return new WaitForSeconds(PatternCooldown + 0.1f);
        int randomInt = Random.Range(10, 25);
        StartCoroutine(MeteoriteHell(randomInt, 0.20f));
        var lookPos = Target.transform.position - this.transform.position;
        lookPos.y = 0;
        var _rotation = Quaternion.LookRotation(lookPos);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 2f);
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.22f, 1, 6, 0));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.24f, 1, -6, 0));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.26f, 1, 0, -6));
        StartCoroutine(SpawnLavaSpikeAttackPlayerDynamic(randomInt * 0.20f, 1, 0, 6));
        PatternCooldown += maxPatternCooldown;
        PatternCooldown += randomInt * 0.26f;
        if (loopCount <= 1)
        {
            lateTeleport((float)(randomInt * 0.26f + 1.2f));
            Invoke("ClearDetermination", (float)(randomInt * 0.26f + 1 + 5));
            loopCount -= 1;
            yield return null;
        }
        else        
        {
            StartCoroutine(FirstRotativeAttack(loopCount));
            yield return null;
        }
        yield return null;
    }

    #endregion

    #region Knock back attack
    void StartupKnockplayerBack()
    {
        USM.PlayGolemWhooshSound();
        Invoke("KnockbackPlayerAttack", 0.6f);
    }

    void KnockbackPlayerAttack()
    {
        FPSController fpscont = FPSController.PlayerObject.GetComponent<FPSController>();
        fpscont.KnockbackImpact((-fpscont.transform.forward * 2) + (fpscont.transform.up / 2), 700);
    }

    #endregion

    #region Pre warming Attacking

    public void WarmAttack()
    {
        this.enemyState = States.Preattacking_Chasing;
        NMA.SetDestination(Target.transform.position);
        Invoke("MainAttack", 1);
    }
    public void MainAttack()
    {
        this.enemyState = States.Attacking_Chasing;
    }
    #endregion


    public void ClearDetermination()
    {
        Determined = false;
        Debug.Log("calling");
    }
    public void SetDetermination()
    {
        this.enemyState = States.DetermineAttack;
    }
    public void TakeDmg(bool Critical)
    {
        if (this.enemyState == States.Attacking_Chasing)
        {
            this.enemyState = States.Attacking_Spelling;
            AnimManager.SetBool("Chasing", false);
            AnimManager.Play("Standing Taunt Battlecry");
            StartupKnockplayerBack();
            Invoke("SetDetermination", 1.2f);
            Invoke("ClearDetermination", 1.2f);
        }
        if (Critical)
        {
            //Stunned = StunTime;
            //AnimManager.Play("StunnedLava");         
        }
        else
        {
            if (Stunned <= 0)
            {
                if (this.enemyState != States.Attacking_Spelling)
                {
                    AnimManager.Play("GetHit");
                }
            }
        }
        this.Target = playerRef.gameObject;
    }
    public void Die()
    {
        Debug.Log("Died");
        AnimManager.Play("DeathLava");
        this.gameObject.AddComponent<DestroyMe>().DestroyTime = 60f;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        EMS.DestroyBar();
        ModuleObject.GetComponent<LavaMainCont>().OurBossDied();
        this.gameObject.transform.GetChild(2).GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", Color.black);
        this.enemyState = States.Died;
        if (Die_Sound.Length >= 1)
        {
            MonsterSound.PlayOneShot(Die_Sound[Random.Range(0, Die_Sound.Length)],1);  
        }
    }
    public void AttackPlayer()
    {
        AnimManager.Play("Standing Melee Attack Horizontal");
        Invoke("OpenParryWindow", 0.32f);
        Invoke("CloseParryWindow", 0.53f);
        Invoke("ClearAnim", 0.56f);
    }
    private void ClearAnim()
    {
        if (!EMS.EHC.Parried)
        {
            EMS.EHC.HitPlayer();
        }
        else
        {
            Stunned += Random.Range(EMS.EHC.ParryStunTimeMin, EMS.EHC.ParryStunTimeMax + 1);
            EMS.EHC.Parried = false;
            AnimManager.Play("StunnedLava");
            AnimManager.SetBool("Stunned", true);
        }
    }
    private void OpenParryWindow()
    {
        EMS.EHC.ParryWindow = true;
    }
    private void CloseParryWindow()
    {
        EMS.EHC.ParryWindow = false;
    }
    public void SpawnMeteor(float speedVar)
    {
        if (speedVar == 0)
        {
            Instantiate(LavaGolemRock, LavaGolemSpawner.transform.position, LavaGolemRock.transform.rotation);
            Instantiate(LavaGolemMeteorSound, LavaGolemSpawner.transform.position, transform.rotation);
            CameraShaker.Instance.ShakeOnce(1, 1, 0, 1);
        }
        else
        {
            GameObject GO = Instantiate(LavaGolemRock, LavaGolemSpawner.transform.position, LavaGolemRock.transform.rotation);
            GO.GetComponent<LavaGolemRock>().Speed = speedVar;
            Instantiate(LavaGolemMeteorSound, LavaGolemSpawner.transform.position, transform.rotation);
            CameraShaker.Instance.ShakeOnce(1, 1, 0, 1);
        }
    }
    public void StartLavaGolem()
    {
        Invoke("SetDetermination", 1f);
        Invoke("ClearDetermination", 1f);
    }
}

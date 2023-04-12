using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using HelperOfDmr;
public class MobMrChest : MonoBehaviour,IMapGetSerializer
{
    EnemyMainStat EMS;
    public float PureDmg, MagicalDmg, PhysicalDmg;
    private Animator AnimManager;
    public LayerMask WhatIsPlayer, WhatIsGround;
    public NavMeshAgent NMA;
    public float StunTime;
    public float Stunned;
    public float OurHP;
    private GameObject Target;
    public List<Transform> PatrolLocations = new List<Transform>();
    private Transform OldTransform;
    public AudioSource MonsterSound;
    [Header("Contained Item")]
    private GameObject PrefabItem;
    private Image ItemImage;
    private TextMeshProUGUI CountText, ItemNameText;
    private int ItemCount;
    [HideInInspector]
    public GameObject PanelObject;
    [Header("SFX")]
    public AudioClip[] General;
    public AudioClip[] Die_Sound;
    public AudioClip[] Spot;
    public AudioClip[] BiteSound;
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
    private bool serialized;
    DataItem item;
    public enum States
    {
        Sleeping,
        Taunting,
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
        if (!serialized)
        {
            DetermineLootQuality();
        }
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
            AnimManager.Play("Dizzy");
            this.enemyState = States.Patrolling;
            CancelInvoke("AttackPlayer");
            CancelInvoke("ClearAnim");
        }
        if (RaycastCenter.LookingItemObject == this.gameObject)
        {

            if (this.enemyState != States.Sleeping)
            {
                PanelObject.SetActive(false);
            }
            else
            {
                PanelObject.SetActive(true);
            }
        }
        else
        {
            PanelObject.SetActive(false);
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
            NMA.speed = 8f;
        }
        if (Target == null && this.enemyState != States.Died && Stunned <= 0 && this.enemyState != States.Attacking && this.enemyState != States.Sleeping && this.enemyState != States.Taunting)
        {
            enemyState = States.Patrolling;
        }
        else if (Stunned <= 0 && this.enemyState != States.Died && this.enemyState != States.Attacking && CanSeePlayer && this.enemyState != States.Sleeping && this.enemyState != States.Taunting)
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
                AnimManager.Play("Dizzy");
            }
            if (this.enemyState != States.Attacking)
            {
                if (this.enemyState != States.Sleeping)
                {
                    AnimManager.Play("Dizzy");
                    this.enemyState = States.Patrolling;
                }else
                {
                    this.enemyState = States.Taunting;
                    if (!WokeUp)
                    {
                        WokeUp = true;
                        Invoke("StopTaunting", 1.6f);
                        AnimManager.Play("Taunting");
                        var lookPos = playerRef.transform.position - this.transform.position;
                        lookPos.y = 0;
                        var _rotation = Quaternion.LookRotation(lookPos);
                        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation,9000f);
                    }
                }
            }
        }
        else
        {
            if (Stunned <= 0)
            {
                if (this.enemyState != States.Attacking)
                {
                    if (this.enemyState == States.Sleeping)
                    {
                        this.enemyState = States.Taunting;
                        if (!WokeUp)
                        {
                            WokeUp = true;
                            Invoke("StopTaunting", 1.6f);
                            MonsterSound.PlayOneShot(Spot[0], 0.35f);
                            AnimManager.Play("Taunting");
                            var lookPos = playerRef.transform.position - this.transform.position;
                            lookPos.y = 0;
                            var _rotation = Quaternion.LookRotation(lookPos);
                            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, 9000f);
                        }
                    }
                    else
                    {
                        if (this.enemyState != States.Taunting && this.enemyState != States.Attacking)
                        {
                            AnimManager.Play("GetHit");
                        }
                        this.enemyState = States.Chasing;
                        var lookPos = Target.transform.position - this.transform.position;
                        lookPos.y = 0;
                        var _rotation = Quaternion.LookRotation(lookPos);
                        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * 15f);
                    }

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
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        EMS.DestroyBar();
        if (Die_Sound.Length >= 1)
        {
            MonsterSound.PlayOneShot(Die_Sound[Random.Range(0, Die_Sound.Length)], 1);
        }
        GameObject T = new GameObject();
        T.transform.position = this.transform.position;
        T.transform.position = new Vector3(T.transform.position.x, T.transform.position.y + 1f, T.transform.position.z);
        T.transform.rotation = RotationHelper.GiveRandomRotation(false);
        PickableItemCreator.instance.CreatePickUpAbleItem(item.Id, ItemCount, T.transform, 1);
        Destroy(T);
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
        AnimManager.Play("Attack01");
        Invoke("ClearAnim",0.25f);
        MonsterSound.PlayOneShot(BiteSound[Random.Range(0, BiteSound.Length)], 1);
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
    public void PlayRandomSound()
    {
        if (this.enemyState != States.Died && this.enemyState != States.Sleeping)
        {
            Invoke("PlayRandomSound", Random.Range(10, 50));
            if (General.Length >= 1)
            {
                MonsterSound.PlayOneShot(General[Random.Range(0, General.Length)], 0.35f);
            }
        }
    }
    public void StopTaunting()
    {
        this.enemyState = States.Patrolling;
    }
    public void SpawnContainedItem()
    {
        GameObject WorldSpaceCanvas = PostProcessingManager.instance.gameObject.GetComponent<SpellUIPusher>().worldCanvas.gameObject;
        this.PrefabItem = Resources.Load<GameObject>("ItemIconHolder");
        GameObject GO = Instantiate(this.PrefabItem, WorldSpaceCanvas.transform);
        GO.transform.position = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y +0.5f, this.gameObject.transform.position.z) ;
        this.ItemImage = GO.GetComponent<Image>();
        this.ItemImage.sprite = this.item.Photo;
        this.ItemNameText = GO.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        this.CountText = GO.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        this.CountText.enabled = false;
        GO.GetComponent<LookTheCam>().OnlyY = false;
        this.ItemNameText.text = this.item.ItemName;
        this.CountText.text = this.ItemCount.ToString();
        GO.transform.localScale = RotationHelper.GiveVec3WithOne(0.5f);
        PanelObject = GO;
    }
    public void DetermineLootQuality()
    {
        int RandomNes = Random.Range(0, 100);
        if (RandomNes >= 0 && RandomNes <= 7)
        {
            item = ReturnItem("Legendary");
        }
        if (RandomNes > 7 && RandomNes <= 20)
        {
            item = ReturnItem("Epic");
        }
        if (RandomNes > 20 && RandomNes <= 50)
        {
            item = ReturnItem("Rare");
        }
        if (RandomNes > 50 && RandomNes <= 100)
        {
            item = ReturnItem("Common");
        }
        ItemCount = GetCount(item);
        SpawnContainedItem();
    }
    public DataItem ReturnItem(string Rarity)
    {
        Inventory inv = PostProcessingManager.instance.gameObject.GetComponent<Inventory>();
        List<DataItem> items = new List<DataItem>();
        items.Clear();
        foreach (DataItem _item in inv.DataItems)
        {
            if (!_item.BlockSpawnOnMobs)
            {
                items.Add(_item);
            }
        }
        List<DataItem> NewItemList = new List<DataItem>();
        NewItemList.Clear();
        foreach (DataItem Items in items)
        {
            if (Rarity == "Legendary")
            {
                if (Items.Rarity == DataItem.RarityRnum.Legendary)
                {
                    NewItemList.Add(Items);
                }
            }
            if (Rarity == "Epic")
            {
                if (Items.Rarity == DataItem.RarityRnum.Epic)
                {
                    NewItemList.Add(Items);
                }
            }
            if (Rarity == "Rare")
            {
                if (Items.Rarity == DataItem.RarityRnum.Rare)
                {
                    NewItemList.Add(Items);
                }
            }
            if (Rarity == "Common")
            {
                if (Items.Rarity == DataItem.RarityRnum.Common)
                {
                    NewItemList.Add(Items);
                }
            }
        }
        int RandomIndex = Random.Range(0, NewItemList.Count);
        return NewItemList[RandomIndex];
        
    }
    public int GetCount(DataItem GetCountItem)
    {
        if (GetCountItem.Rarity == DataItem.RarityRnum.Rare || GetCountItem.Rarity == DataItem.RarityRnum.Common)
        {
            if (GetCountItem.MaxStack >= 10)
            {
                int ReturnInt = Random.Range(1, GetCountItem.MaxStack);
                return ReturnInt;
            }
        }
        return 1;
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
        this.item.Id = SIC.serialIntList[1];
        this.ItemCount = SIC.serialIntList[2];
        this.item.ItemName = SIC.serialStringList[1];
        this.item.Photo = SIC.serialSpriteList[0];
        foreach (Vector3 vec in SIC.serialVec3List)
        {
            GameObject G = new GameObject();
            G.transform.position = vec;
            PatrolLocations.Add(G.transform);
        }
        Inventory inv = PostProcessingManager.instance.GetComponent<Inventory>();
        foreach (DataItem item in inv.DataItems)
        {
            if (item.Id == this.item.Id)
            {
                this.item = item;
            }
        }
        GameObject WorldSpaceCanvas = PostProcessingManager.instance.gameObject.GetComponent<SpellUIPusher>().worldCanvas.gameObject;
        this.PrefabItem = Resources.Load<GameObject>("ItemIconHolder");
        GameObject GO = Instantiate(this.PrefabItem, WorldSpaceCanvas.transform);
        GO.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f, this.gameObject.transform.position.z);
        this.ItemImage = GO.GetComponent<Image>();
        this.ItemImage.sprite = this.item.Photo;
        this.ItemNameText = GO.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        this.CountText = GO.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        this.CountText.enabled = false;
        GO.GetComponent<LookTheCam>().OnlyY = false;
        this.ItemNameText.text = this.item.ItemName;
        this.CountText.text = this.ItemCount.ToString();
        GO.transform.localScale = RotationHelper.GiveVec3WithOne(0.5f);
        PanelObject = GO;
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
            SIC.serialIntList.Add(this.item.Id);
            SIC.serialIntList.Add(this.ItemCount);
            SIC.serialStringList.Add(this.item.ItemName);
            SIC.serialSpriteList.Add(this.item.Photo);
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
            SIC.serialIntList.Add(this.item.Id);
            SIC.serialIntList.Add(this.ItemCount);
            SIC.serialStringList.Add(this.item.ItemName);
            SIC.serialSpriteList.Add(this.item.Photo);
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

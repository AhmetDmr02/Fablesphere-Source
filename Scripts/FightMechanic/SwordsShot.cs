using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using HelperOfDmr;

public class SwordsShot : MonoBehaviour
{
    public Animator SwordAnimator;
    public SlotStatCalculator SSC;
    public StatData SD;
    private float Speed;
    public float Distance;
    private CooldownManager CM;
    private BarManager Bm;
    private ActivatorManager Act;
    public Camera cam;
    private int LastAnimation;
    public bool ManaGodActive,CritActive;
    RaycastHit hit;
    private void Update()
    {
        if (CM == null) 
            CM = GameObject.FindGameObjectWithTag("Manager").GetComponent<CooldownManager>();
        if (Act == null)
            Act = GameObject.FindGameObjectWithTag("Manager").GetComponent<ActivatorManager>();
        if (Bm == null)
            Bm = GameObject.FindGameObjectWithTag("Manager").GetComponent<BarManager>();
        if (SD == null)
            SD = GameObject.FindGameObjectWithTag("Manager").GetComponent<StatData>();
        if (SSC == null)
            SSC = GameObject.FindGameObjectWithTag("Manager").GetComponent<SlotStatCalculator>();
        if (Input.GetMouseButton(0) && !CM.OnCooldown && !Act.SomeoneActive && Bm.Stam >= 42 && !SwordAnimator.GetBool("Blocking"))
        {
            PlaySwordSwing PSSS = this.transform.GetChild(0).GetComponent<PlaySwordSwing>();
            int CritOrNot = Random.Range(0, 100);
            bool Critical = false;
            int AnimIndex = Random.Range(1, 5); // 1 or 4
            while (AnimIndex == LastAnimation) AnimIndex = Random.Range(1, 5);
            if (!PostProcessingManager.instance.CritPotionActive)
            {
                if (!CritActive)
                {
                    if (CritOrNot < 90)
                    {
                        Critical = false;
                    }
                    else
                    {
                        Critical = true;
                    }
                }
                else
                {
                    //CritActive = false;
                    Critical = true;
                }
            }
            else
            {
                Critical = true;
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Distance))
            {
                if (hit.transform.tag == "EnemyMob")
                {
                    float[] SwordWeapon = SD.GetPureStatArrayFromSlots(SSC.StatSlots);
                  
                    if (SwordWeapon[3] == 1)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 1;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(1);
                        if (!ManaGodActive)
                            Bm.Stam -= 42;
                        LastAnimation = AnimIndex;
                        EnemyMainStat EMS = hit.transform.GetComponent<EnemyMainStat>();
                        PlaySwordSwing PSS = this.gameObject.transform.GetChild(0).GetComponent<PlaySwordSwing>();
                        if (CritActive)
                        {
                            EMS.PlayerHit(hit.point, PSS, true);
                            CritActive = false;
                        }
                        else
                        {
                            EMS.PlayerHit(hit.point, PSS, Critical);
                        }
                    }
                    if (SwordWeapon[3] == 2)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 2;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(0.5f);
                        if (!ManaGodActive)
                            Bm.Stam -= 21;
                        LastAnimation = AnimIndex;
                        EnemyMainStat EMS = hit.transform.GetComponent<EnemyMainStat>();
                        PlaySwordSwing PSS = this.gameObject.transform.GetChild(0).GetComponent<PlaySwordSwing>();
                        if (CritActive)
                        {
                            EMS.PlayerHit(hit.point, PSS, true);
                            CritActive = false;
                        }
                        else
                        {
                            EMS.PlayerHit(hit.point, PSS, Critical);
                        }
                    }
                    if (SwordWeapon[3] == 3)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 3;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(0.33f);
                        if (!ManaGodActive)
                            Bm.Stam -= 11;
                        LastAnimation = AnimIndex;
                        EnemyMainStat EMS = hit.transform.GetComponent<EnemyMainStat>();
                        PlaySwordSwing PSS = this.gameObject.transform.GetChild(0).GetComponent<PlaySwordSwing>();
                        if (CritActive)
                        {
                            EMS.PlayerHit(hit.point, PSS, true);
                            CritActive = false;
                        }
                        else
                        {
                            EMS.PlayerHit(hit.point, PSS, Critical);
                        }
                    }
                } 
                else if(hit.transform.tag == "Gragoyle"){
                    float[] SwordWeapon = SD.GetPureStatArrayFromSlots(SSC.StatSlots);
                    if (SwordWeapon[3] == 1)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 1;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(1);
                        if (!ManaGodActive)
                            Bm.Stam -= 42;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                        LastAnimation = AnimIndex;
                        GargoyleStatue GS = hit.transform.GetComponent<GargoyleStatue>();
                        PlaySwordSwing PSS = this.gameObject.transform.GetChild(0).GetComponent<PlaySwordSwing>();
                        GS.WarmDamage(hit.point, PSS, Critical);
                    }
                    if (SwordWeapon[3] == 2)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 2;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(0.5f);
                        if (!ManaGodActive)
                            Bm.Stam -= 21;
                        LastAnimation = AnimIndex;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                        GargoyleStatue GS = hit.transform.GetComponent<GargoyleStatue>();
                        PlaySwordSwing PSS = this.gameObject.transform.GetChild(0).GetComponent<PlaySwordSwing>();
                        GS.WarmDamage(hit.point, PSS, Critical);
                    }
                    if (SwordWeapon[3] == 3)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 3;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(0.33f);
                        if (!ManaGodActive)
                            Bm.Stam -= 11;
                        LastAnimation = AnimIndex;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                        GargoyleStatue GS = hit.transform.GetComponent<GargoyleStatue>();
                        PlaySwordSwing PSS = this.gameObject.transform.GetChild(0).GetComponent<PlaySwordSwing>();
                        GS.WarmDamage(hit.point, PSS, Critical);
                    }
                }
                else
                {
                    float[] SwordWeapon = SD.GetPureStatArrayFromSlots(SSC.StatSlots);
                    if (SwordWeapon[3] == 1)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 1;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(1);
                        if (!ManaGodActive)
                            Bm.Stam -= 42;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                        LastAnimation = AnimIndex;
                    }
                    if (SwordWeapon[3] == 2)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 2;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(0.5f);
                        if (!ManaGodActive)
                            Bm.Stam -= 21;
                        LastAnimation = AnimIndex;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                    }
                    if (SwordWeapon[3] == 3)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 3;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                                PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                                break;
                        }
                        CM.CreateCooldown(0.33f);
                        if (!ManaGodActive)
                            Bm.Stam -= 11;
                        LastAnimation = AnimIndex;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                    }
                }
            }
            else
            {
                float[] SwordWeapon = SD.GetPureStatArrayFromSlots(SSC.StatSlots);
                if (SwordWeapon[3] == 1)
                {
                    //Give Cooldown 1 Sec And Set Animator Speed To One;
                    SwordAnimator.speed = 1;
                    switch (AnimIndex)
                    {
                        case 1:
                            SwordAnimator.Play("SwordAttack1");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                            break;
                        case 2:
                            SwordAnimator.Play("SwordAttack2");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                            break;
                        case 3:
                            SwordAnimator.Play("SwordAttack3");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                            break;
                        case 4:
                            SwordAnimator.Play("SwordAttack4");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                            break;
                    }
                    CM.CreateCooldown(1);
                    if (!ManaGodActive)
                        Bm.Stam -= 42;
                    CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                    LastAnimation = AnimIndex;
                }
                if (SwordWeapon[3] == 2)
                {
                    //Give Cooldown 1 Sec And Set Animator Speed To One;
                    SwordAnimator.speed = 2;
                    switch (AnimIndex)
                    {
                        case 1:
                            Debug.Log(AnimIndex + " AnimIndex vs " + 1);
                            SwordAnimator.Play("SwordAttack1");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                            break;
                        case 2:
                            Debug.Log(AnimIndex + " AnimIndex vs " + 2);
                            SwordAnimator.Play("SwordAttack2");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                            break;
                        case 3:
                            Debug.Log(AnimIndex + " AnimIndex vs " + 3);
                            SwordAnimator.Play("SwordAttack3");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                            break;
                        case 4:
                            Debug.Log(AnimIndex + " AnimIndex vs " + 4);
                            SwordAnimator.Play("SwordAttack4");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                            break;
                    }
                    CM.CreateCooldown(0.5f);
                    if (!ManaGodActive)
                        Bm.Stam -= 21;
                    LastAnimation = AnimIndex;
                    CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                }
                if (SwordWeapon[3] == 3)
                {
                    //Give Cooldown 1 Sec And Set Animator Speed To One;
                    SwordAnimator.speed = 3;
                    switch (AnimIndex)
                    {
                        case 1:
                            SwordAnimator.Play("SwordAttack1");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[0]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[0]));
                            break;
                        case 2:
                            SwordAnimator.Play("SwordAttack2");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[1]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[1]));
                            break;
                        case 3:
                            SwordAnimator.Play("SwordAttack3");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[2]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[2]));
                            break;
                        case 4:
                            SwordAnimator.Play("SwordAttack4");
                            PSSS.StartCoroutine(PSSS.OpenSwingSound(PSSS.TempPlayShotMS[3]));
                            PSSS.StartCoroutine(PSSS.OpenCutSound(PSSS.TempCutPointMS[3]));
                            break;
                    }
                    CM.CreateCooldown(0.33f);
                    if (!ManaGodActive)
                        Bm.Stam -= 11;
                    LastAnimation = AnimIndex;
                    CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                }
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit  ))
            {
                if (hit.transform.tag == "EnemyMob")
                {
                }
                else if (hit.transform.tag == "Gragoyle")
                {
                }
                else
                {
                    float[] SwordWeapon = SD.GetPureStatArrayFromSlots(SSC.StatSlots);
                    if (SwordWeapon[3] == 1)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 1;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                break;
                        }
                        CM.CreateCooldown(1);
                        if (!ManaGodActive)
                            Bm.Stam -= 42;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                        LastAnimation = AnimIndex;
                    }
                    if (SwordWeapon[3] == 2)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 2;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                break;
                        }
                        CM.CreateCooldown(0.5f);
                        if (!ManaGodActive)
                            Bm.Stam -= 21;
                        LastAnimation = AnimIndex;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                    }
                    if (SwordWeapon[3] == 3)
                    {
                        //Give Cooldown 1 Sec And Set Animator Speed To One;
                        SwordAnimator.speed = 3;
                        switch (AnimIndex)
                        {
                            case 1:
                                SwordAnimator.Play("SwordAttack1");
                                break;
                            case 2:
                                SwordAnimator.Play("SwordAttack2");
                                break;
                            case 3:
                                SwordAnimator.Play("SwordAttack3");
                                break;
                            case 4:
                                SwordAnimator.Play("SwordAttack4");
                                break;
                        }
                        CM.CreateCooldown(0.33f);
                        if (!ManaGodActive)
                            Bm.Stam -= 11;
                        LastAnimation = AnimIndex;
                        CameraShaker.Instance.ShakeOnce(1, 1, 0.5f, 1f);
                    }
                }   
            }
        }
        if (Input.GetMouseButton(1) && !Act.SomeoneActive)
        {
            if(Bm.Stam >= 30)
            {
                PostProcessingManager.instance.gameObject.GetComponent<BarManager>().GuardPos = true;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    if (hit.transform.tag == "EnemyMob")
                    {
                        if (hit.transform.GetComponent<EnemyHitCenter>() != null)
                        {
                            EnemyHitCenter EHCenemy = hit.transform.GetComponent<EnemyHitCenter>();
                            if (EHCenemy.ParryWindow != true)
                            {
                                SwordAnimator.SetBool("Blocking", true);
                            }
                        }
                    }
                    else
                    {
                        SwordAnimator.SetBool("Blocking", true);
                    }
                }
                else
                {
                    SwordAnimator.SetBool("Blocking", true);
                }
            }
        }
        else if (SwordAnimator.GetBool("Blocking"))
        {
            SwordAnimator.SetBool("Blocking", false);
            PostProcessingManager.instance.gameObject.GetComponent<BarManager>().GuardPos = false;
        }
        if (Input.GetMouseButtonDown(1) && !Act.SomeoneActive && Bm.Stam >= 30)
        {
            if(!ManaGodActive)
            Bm.Stam -= 30;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                if (hit.transform.tag == "EnemyMob")
                {
                    if (hit.transform.GetComponent<EnemyHitCenter>() != null)
                    {
                        EnemyHitCenter EHCenemy = hit.transform.GetComponent<EnemyHitCenter>();
                        if (EHCenemy.ParryWindow == true)
                        {
                            SwordAnimator.Play("ParrySword");
                            ParryVoid(EHCenemy);
                            Debug.Log("Parried");
                        }
                    }
                }
            }
        }
    }
    public void ParryVoid(EnemyHitCenter ehc)
    {
        Invoke("CreateParticles", 0.05f);
        ShakeSize = ehc.CameraShakeSize;
        EHCParry = ehc;
    }
    float ShakeSize;
    EnemyHitCenter EHCParry;
    public void CreateParticles()
    {
        EffectManager EM = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
        EM.CreateSparkEffect(this.gameObject.transform);
        UISfxManager USM = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
        USM.PlayParrySound();
        EHCParry.Parried = true;
        CameraShaker.Instance.ShakeOnce(ShakeSize, 1 * 1.5f, 0f, 1f);
    }
}

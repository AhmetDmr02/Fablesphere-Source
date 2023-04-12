using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISfxManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ButtonClickSound;
    public AudioClip ButtonEnterSound;
    public AudioClip[] ErrorSfx;
    public AudioClip[] WearSfx;
    public AudioClip[] ClothHitSfx;
    public AudioClip DrinkSfx;
    public AudioClip[] SwordSwingSound;
    public AudioClip[] RockHitSound;
    public AudioClip[] ParrySound;
    public AudioClip Critical;
    public AudioClip PlayHealSpell;
    public AudioClip PlayManaSpell;
    public AudioClip GolemKnockbackSFX;


    public static UISfxManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(this.gameObject);
        }
    }
    public void PlayErrorSfx()
    {
        audioSource.PlayOneShot(ErrorSfx[Random.Range(0, ErrorSfx.Length)], 0.05f);
    }
    public void PlayWearSfx()
    {
        audioSource.PlayOneShot(WearSfx[Random.Range(0, WearSfx.Length)], 0.05f);
    }
    public void PlayDrinkSfx()
    {
        audioSource.PlayOneShot(DrinkSfx, 0.05f);
    }
    private int BeforeFloat;
    public void PlayClothHitSound()
    {
        var Floated = Random.Range(0, ClothHitSfx.Length);
        while (Floated == BeforeFloat)
        {
            Floated = Random.Range(0, ClothHitSfx.Length);
        }
        audioSource.PlayOneShot(ClothHitSfx[Floated],0.25f);
        BeforeFloat = Floated;
    }
    private int BeforeSwordSwingIndex;
    public void PlaySwordSwingSound()
    {
        var Floated_ = Random.Range(0, SwordSwingSound.Length);
        while (Floated_ == BeforeSwordSwingIndex)
        {
            Floated_ = Random.Range(0, SwordSwingSound.Length);
        }
        audioSource.PlayOneShot(SwordSwingSound[Floated_], 0.2f);
        BeforeSwordSwingIndex = Floated_;
    }
    private int RockFloat;
    public void PlayRockHitSound()
    {
        var Floated = Random.Range(0, RockHitSound.Length);
        while (Floated == RockFloat)
        {
            Floated = Random.Range(0, RockHitSound.Length);
        }
        audioSource.PlayOneShot(RockHitSound[Floated], 0.35f);
        RockFloat = Floated;
    }
    public void PlayCriticalShot()
    {
        audioSource.PlayOneShot(Critical, 0.35f);
    }
    public void PlayParrySound()
    {
        var randomInt = Random.Range(0, ParrySound.Length);
        audioSource.PlayOneShot(ParrySound[randomInt], 0.5f);
    }
    public void PlayHealSound()
    {
        audioSource.PlayOneShot(PlayHealSpell, 0.35f);
    }
    public void PlayManaSound()
    {
        audioSource.PlayOneShot(PlayManaSpell, 0.35f);
    }
    public void PlayGolemWhooshSound()
    {
        audioSource.PlayOneShot(GolemKnockbackSFX, 0.35f);
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(ButtonClickSound, 0.45f);
    }
    public void PlayButtonEnter()
    {
        audioSource.PlayOneShot(ButtonEnterSound, 0.30f);
    }
}

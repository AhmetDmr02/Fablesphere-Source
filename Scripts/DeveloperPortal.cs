using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DeveloperPortal : MonoBehaviour
{
    public float Percent;
    public StatData SD;
    public SlotStatCalculator SSC;
    public EnemyHitCenter EHC;
    public GameObject LavaBeam;
    private UISfxManager USM;
    [SerializeField] private PostprocessingData[] Pd;
    [SerializeField] private GameObject LavaGolemSpawner, LavaGolemRock, LavaGolemMeteorSound;
    void Start()
    {

    }

    void Update()
    {
        if (USM == null)
        {
            USM = GameObject.FindGameObjectWithTag("Manager").GetComponent<UISfxManager>();
        }
        if (Input.GetKeyDown(KeyCode.F3)) //hitplayer
        {
            float[] arrayedAnswer = SD.GetPureStatArrayFromSlots(SSC.StatSlots);
            float percentAge = (arrayedAnswer[1] / 1000) * 100; //PhysicalPercent
            float percentAgeMagic = (arrayedAnswer[2] / 1000) * 100; //MagicalPercent
            EHC.HitPlayer();
        }
        if (Input.GetKeyDown(KeyCode.F4)) //Create Item Id List
        {
            WriteTextForItemIds WTFII = gameObject.GetComponent<WriteTextForItemIds>();
            WTFII.Clicked = true;
        }
        if (Input.GetKeyDown(KeyCode.F5))//Create Fading Text
        {
            StartCoroutine(FadingTextCreator.instance.InitFadeText("Test", 1, 5, null, Color.blue));
        }
        if (Input.GetKeyDown(KeyCode.F6))//Create Lava Beam
        {
            Transform PlayerT = FPSController.PlayerObject.transform;
            GameObject GO = Instantiate(LavaBeam, new Vector3(PlayerT.position.x, LavaBeam.transform.position.y, PlayerT.position.z), LavaBeam.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.F7))//Player Knockback
        {

            USM.PlayGolemWhooshSound();
            Invoke("GiveImpact", 0.6f);
        }
        if (Input.GetKeyDown(KeyCode.F8))//LavaGolemRockSpawner
        {
            Instantiate(LavaGolemRock, LavaGolemSpawner.transform.position, LavaGolemRock.transform.rotation);
            Instantiate(LavaGolemMeteorSound, LavaGolemSpawner.transform.position, transform.rotation);
            CameraShaker.Instance.ShakeOnce(1, 1, 0, 1);
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            PostProcessingManager.instance.handlePostProcessingSettings(Pd[0]);
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            PostProcessingManager.instance.handlePostProcessingSettings(PostProcessingManager.instance.getActiveModuleObject().GetComponent<BasicModuleInformations>().getProcessSettings());
        }
    }

    private void GiveImpact()
    {
        FPSController fpscont = FPSController.PlayerObject.GetComponent<FPSController>();
        fpscont.KnockbackImpact((-fpscont.transform.forward * 2) + (fpscont.transform.up / 2), 700);
    }

}

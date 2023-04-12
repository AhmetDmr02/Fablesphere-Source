using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;
public class LavaMainCont : MonoBehaviour,IMapGetSerializer
{
    private GameObject ManagerObj;
    public GameObject PlayerCam, SceneCam,ModuleBoss,PreventionWall,PortalObject;
    public AudioSource DissloveEffect;
    public DynamicChestSpawner[] ChestSpawner;
    public List<Chest> bountChests;
    public Canvas PlayerCanvas, InvCanvas;
    private bool OpenMusic,DecreaseMusic,BossDead;
    public bool isBossDead => BossDead;

    public static LavaMainCont instance;

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
    private void Update()
    {
        if (ManagerObj == null)
        {
            ManagerObj = GameObject.FindGameObjectWithTag("Manager");
        }
        if (PlayerCanvas == null)
        {
            PlayerCanvas = GameObject.FindGameObjectWithTag("CanvasPopUp").GetComponent<Canvas>();
        }
        if (InvCanvas == null)
        {
            InvCanvas = GameObject.FindGameObjectWithTag("CanvasInventory").GetComponent<Canvas>();
        }
        if (PlayerCam == null)
        {
            PlayerCam = GameObject.FindGameObjectWithTag("PlayerCam");
        }
        if (OpenMusic)
        {
            AudioSource Music = this.gameObject.GetComponent<AudioSource>();
            if (Music.volume < 0.004f)
            {
                Music.volume += 0.0005f * Time.deltaTime;
            }
            else
            {
                OpenMusic = false;
            }
        }
        if (DecreaseMusic)
        {
            AudioSource Music = this.gameObject.GetComponent<AudioSource>();
            if (Music.volume >= 0.000f)
            {
                Music.volume -= 0.0005f * Time.deltaTime;
            }
            else
            {
                OpenMusic = false;
                Music.volume = 0;
            }
        }
    }
    public IEnumerator LaunchScene()
    {
        PlayerCam.SetActive(false);
        SceneCam.SetActive(true);
        PlayerCanvas.enabled = false;
        InvCanvas.enabled = false;
        OpenMusic = true;
        AudioSource Music = this.gameObject.GetComponent<AudioSource>();
        Music.Play();
        yield return new WaitForSeconds(5);
        this.gameObject.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(12.50f);
        EffectManager EM = ManagerObj.GetComponent<EffectManager>();
        EM.CreateBlackoutEffect(0.5f, 2, true, 1);
        yield return new WaitForSeconds(3);
        GiveControlPlayerAgain();
        ModuleBoss.GetComponent<Animator>().Play("LavaIdle");
        ModuleBoss.GetComponent<LavaGolemMain>().StartLavaGolem();
        StopCoroutine(LaunchScene());
    }
    public void GiveControlPlayerAgain()
    {
        PlayerCanvas.enabled = true;
        PlayerCam.SetActive(true);
        SceneCam.SetActive(false);
        ManagerObj.GetComponent<ActivatorManager>().CutsceneActive = false;
    }
    public void OurBossDied()
    {
        //GiveLilShake
        //Maybe Some Chest Open Noises
        Debug.Log("Lava golem died");
        DecreaseMusic = true;
        BossDead = true;
        DissloveEffect.Play();
        PortalObject.SetActive(true);
        PreventionWall.SetActive(false);
        serializeMe();
        foreach (Chest C in bountChests)
        {
            C.Openable = true;
        }
        foreach (DynamicChestSpawner DCS in ChestSpawner)
        {
            if (DCS == null) return;
            DCS.OpenChestBound();
        }
    }
    public void serializeMe()
    {
        Debug.Log("serialize on lava called");
        SerializationInfoClass SIC = HelperOfDmr.UtilitesOfDmr.CreateDefaultSIC(this.gameObject, this);
        SIC.dontDeleteThis = true;
        SIC.serialBoolList.Add(BossDead);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        if (!SIC.serialBoolList[0]) return;
        DecreaseMusic = true;
        BossDead = true;
        DissloveEffect.Play();
        PortalObject.SetActive(true);
        PreventionWall.SetActive(false);
        Destroy(ModuleBoss);
    }
}

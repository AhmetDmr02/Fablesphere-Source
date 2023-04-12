using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    [SerializeField] private GameObject _loadingPanel, ContButton;
    [SerializeField] private Image _fillBar, Banner;
    [SerializeField] private TextMeshProUGUI _whatIsHappening, _moduleDesc, _moduleName;
    public float desiredFloatVolume;
    public bool infoPierced;
    public bool reverseComing;
    public Transform StartPos_,StartPosReverse_;
    int lastIndexOfLoadingManager = -1;
    private float target;
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
    private void Update()
    {
        _fillBar.fillAmount = Mathf.MoveTowards(_fillBar.fillAmount, target, 3 * Time.deltaTime);
    }
    
    public IEnumerator LoadGivenScene(string _sceneDex,int index,bool isNewlyCreated,List<SerializationInfoClass> SIC,ModuleInformations MI)
    {
        StartPos_ = null;
        FPSController.instance.gameObject.GetComponent<CharacterController>().enabled = false;
        FPSController.instance.transform.position = new Vector3(15000, -15000, 15000);
        target = 0;
        _fillBar.fillAmount = 0;
        _loadingPanel.SetActive(true);
        var loadingScene = SceneManager.LoadSceneAsync(_sceneDex);
        _whatIsHappening.gameObject.SetActive(true);
        ContButton.SetActive(false);
        loadingScene.allowSceneActivation = true;
        _whatIsHappening.text = "Loading Scene Resources...";
        do
        {
            yield return new WaitForSecondsRealtime(0.2f);
            target = loadingScene.progress;
        } 
        while (loadingScene.progress < 1f);
        if (loadingScene.progress <= 1f)
        {
            loadingScene.allowSceneActivation = true;
            if (!isNewlyCreated)
            {
                ProceduralModuleGenerator.instance.currentIndex = MI.mapIndex;
                _whatIsHappening.text = "Serializing Objects...";
                SerializationManager.instance.startSerializing(SIC);
                while (!SerializationManager.instance.finished)
                {
                    yield return new WaitForSecondsRealtime(1f);
                }
                //Serializing
            }
            AudioListener.volume = desiredFloatVolume;
            _whatIsHappening.gameObject.SetActive(false);
            //FPSController.instance.Camera.GetChild(0).GetChild(0).GetComponent<AudioListener>().enabled = false;
            while (StartPos_ == null || StartPosReverse_ == null)
            {
                yield return new WaitForSecondsRealtime(1f); //wait sec
            }
            ContButton.SetActive(true);
            Debug.Log("Loading done!");
        }
        //Add on this;
        yield return null;
    }
    public void closePanelAndPlay()
    {
        _loadingPanel.SetActive(false);
        PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = false;
        PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = null;
        AudioListener.volume = infoPierced ? desiredFloatVolume : 1;
        FPSController.instance.gameObject.GetComponent<CharacterController>().velocity.Set(0,0,0);
        FPSController.instance.gameObject.GetComponent<CharacterController>().enabled = false;
        if (!reverseComing)
        {
            FPSController.instance.gameObject.transform.position = StartPos_.position;
            FPSController.instance.gameObject.transform.eulerAngles = StartPos_.eulerAngles;
        }else
        {
            FPSController.instance.gameObject.transform.position = StartPosReverse_.position;
            FPSController.instance.gameObject.transform.eulerAngles = StartPosReverse_.eulerAngles;
        }
        FPSController.instance.gameObject.GetComponent<CharacterController>().enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        //set activator
    }
    public void setTextByData(DataScene DS)
    {
        _moduleName.text = DS.SceneName;
        _moduleDesc.text = DS.SceneExplanation;
        Banner.sprite = DS.loadingBanner;
    }
}

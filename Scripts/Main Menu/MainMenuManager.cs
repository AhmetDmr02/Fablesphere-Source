using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Camera cam;
    private cameraState camState;
    public float lerpSpeed;
    private Transform desiredTransform;
    public Transform NonState, CreditState, SettingsState,PlayState;
    public Canvas innerCanvas,outerNonWorld;
    public GameObject mainLine,databaseEnterance,scrollScreen,settingsScreen;
    public Animator creditPlay;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private TMP_Dropdown resDropdown, graphicDropdown,targetFramerateDropdown;
    [SerializeField] private Toggle fullscreenToggle,isVsyncOnToggle;
    Resolution[] resolutions;
    [Space(15)]
    public InfoPiercer infoPiercer;
    public static MainMenuManager instance;
    [Header("Database Stuff")]
    public TMP_InputField discordIdField;
    public TMP_InputField tokenField;
    public TextMeshProUGUI buttonText;
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
    public void Start()
    {
        #region screen settings stuff
        resDropdown.onValueChanged.AddListener(val => setRes(val));
        graphicDropdown.onValueChanged.AddListener(val => setGraphic(val));
        targetFramerateDropdown.onValueChanged.AddListener(val => setTargetFrameRate(val));
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentIndex = i;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = currentIndex;
        resDropdown.RefreshShownValue();
        _soundSlider.onValueChanged.AddListener(val => setAudio(val));
        fullscreenToggle.onValueChanged.AddListener(val => setFullscreenTick(val));
        isVsyncOnToggle.onValueChanged.AddListener(val => switchVsyncCycle(val));
        #endregion

        desiredTransform = NonState;
        camState = cameraState.Non;
        calculateState();
        outerNonWorld.enabled = false;
        innerCanvas.enabled = true;
        if (MainDatabaseManager.instance.closeDATABASE)
        {
            mainLine.SetActive(true);
            databaseEnterance.SetActive(false);
        }
        else
        {
            mainLine.SetActive(false);
            databaseEnterance.SetActive(true);
        }
        if (PlayerPrefs.HasKey("VolumeMain"))
        {
            float volumeS = PlayerPrefs.GetFloat("VolumeMain");
            _soundSlider.value = volumeS;
            AudioListener.volume = volumeS;
        }
        if (PlayerPrefs.HasKey("GraphicIndex"))
        {
            int indexGraphic = PlayerPrefs.GetInt("GraphicIndex");
            graphicDropdown.SetValueWithoutNotify(indexGraphic);
            setGraphic(indexGraphic);
        }
        if (PlayerPrefs.HasKey("targetFramerate"))
        {
            int targetFramerate = PlayerPrefs.GetInt("targetFramerate");
            targetFramerateDropdown.SetValueWithoutNotify(targetFramerate);
            setTargetFrameRate(targetFramerate);
        }
        if (PlayerPrefs.HasKey("VsyncCycle"))
        {
            int playerPrefsCall = PlayerPrefs.GetInt("VsyncCycle");
            bool isActivee = playerPrefsCall == 0 ? false : true;
            isVsyncOnToggle.SetIsOnWithoutNotify(isActivee);
            switchVsyncCycle(isActivee);
        }
    }
    private void FixedUpdate()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, desiredTransform.position, lerpSpeed * Time.deltaTime);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, desiredTransform.rotation, lerpSpeed * Time.deltaTime);
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public void GoSettings()
    {
        innerCanvas.enabled = false;
        outerNonWorld.enabled = true;
        settingsScreen.SetActive(true);
        camState = cameraState.Settings;
        calculateState();
    }
    public void GoPlay()
    {
        innerCanvas.enabled = false;
        outerNonWorld.enabled = true;
        camState = cameraState.Play;
        calculateState();
    }
    public void GoCredit()
    {
        innerCanvas.enabled = false;
        camState = cameraState.Credit;
        creditPlay.Play("OpenBark");
        calculateState();
        Invoke("GoCreditDelayedPart", 1.4f);
    }
    public async void GetPlayerDataFromIdAndApprove()
    {
        if (MainDatabaseManager.instance.calculating) return;
        string DiscordId = discordIdField.text;
        string TokenId = tokenField.text;
        string fixedDiscordID = DiscordId.Trim();
        string fixedTokenID = TokenId.Trim();
        bool isItOkay = false;
        isItOkay = await MainDatabaseManager.instance.GetDataFromIdAndToken(fixedDiscordID, fixedTokenID);
        if (isItOkay)
        {
            databaseEnterance.SetActive(false);
            mainLine.SetActive(true);
        }else
        {
            buttonText.text = "Unable To Login";
        }
    }
    public void GoCreditDelayedPart()
    {
        outerNonWorld.enabled = true;
        scrollScreen.transform.GetChild(0).GetChild(0).GetChild(0).localPosition = new Vector3(scrollScreen.transform.GetChild(0).GetChild(0).GetChild(0).localPosition.x, 0, scrollScreen.transform.GetChild(0).GetChild(0).GetChild(0).localPosition.z);
        scrollScreen.SetActive(true);
        scrollScreen.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<AutoScroller>().scroll = true;
    }
    public void returnToNon()
    {
        if (camState == cameraState.Credit)
        {
            creditPlay.Play("CloseBark");
            scrollScreen.SetActive(false);
            scrollScreen.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<AutoScroller>().scroll = false;
        }
        innerCanvas.enabled = true;
        outerNonWorld.enabled = false;
        camState = cameraState.Non;
        settingsScreen.SetActive(false);
        calculateState();
    }
    public void calculateState()
    {
        switch (camState)
        {
            case cameraState.Non:
                desiredTransform = NonState;
                break;
            case cameraState.Settings:
                desiredTransform = SettingsState;
                break;
            case cameraState.Play:
                desiredTransform = PlayState;
                break;
            case cameraState.Credit:
                desiredTransform = CreditState;
                break;
        }
                   
    }
    public void setRes(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
    }
    public void setAudio(float volume)
    {
        if (AudioListener.pause == true) AudioListener.pause = false;
        AudioListener.volume = volume;
        infoPiercer.audioFloat = volume;
        PlayerPrefs.SetFloat("VolumeMain", volume);
        PlayerPrefs.Save();
    }
    public void setGraphic(int index)
    {
        QualitySettings.SetQualityLevel(index);
        switchVsyncCycle(true);
        PlayerPrefs.SetInt("GraphicIndex", index);
        PlayerPrefs.Save();
    }
    public void setFullscreenTick(bool isScreenFull)
    {
        Screen.fullScreen = isScreenFull;
    }
    public void setTargetFrameRate(int index)
    {
        switch (index)
        {
            case 0:
                Application.targetFrameRate = -1;
                break;
            case 1:
                Application.targetFrameRate = 144;
                break;
            case 2:
                Application.targetFrameRate = 80;
                break;
            case 3:
                Application.targetFrameRate = 60;
                break;
            case 4:
                Application.targetFrameRate = 30;
                break;
        }
        PlayerPrefs.SetInt("targetFramerate",index);
        PlayerPrefs.Save();
    }
    public void switchVsyncCycle(bool val)
    {
        if (val)
        {
            QualitySettings.vSyncCount = 1;
            PlayerPrefs.SetInt("VsyncCycle", 1);
            PlayerPrefs.Save();
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            PlayerPrefs.SetInt("VsyncCycle", 0);
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// arrays order : market,lab,main,blacksmith
    /// </summary>
    public RandomRooms createQuest(int starterMercy,int extraMercyPercentage, int minSector, int maxSector, int[] minModules,int[] minmaxModules, int[] maxModules,bool isMaxRoomRandom,int ifIsRandomRoomHowManyPlus)
    {
        RandomRooms rr = new RandomRooms();
        rr.mercyCount += starterMercy;
        if (extraMercyPercentage >= Random.Range(0, 101))
        {
            rr.mercyCount += 1;
        }
        rr.maxSector = Random.Range(minSector,maxSector + 1);
        rr.maxMarket = maxModules[0];
        rr.maxLab = maxModules[1];
        rr.maxMain = maxModules[2];
        rr.maxBlacksmith = maxModules[3];
        for (int i = 0; i < minmaxModules.Length; i++)
        {
            if (minmaxModules[i] > maxModules[i])
            {
                minmaxModules[i] = maxModules[i];
            }
        }
        rr.minMarket = Random.Range(minModules[0], minmaxModules[0] +1);
        rr.minLab = Random.Range(minModules[1], minmaxModules[1] +1 );
        rr.minMain = Random.Range(minModules[2], minmaxModules[2] + 1);
        rr.minBlacksmith = Random.Range(minModules[3], minmaxModules[3] +1);
        rr.maxRooms += rr.minBlacksmith + rr.minLab + rr.minMain + rr.minMarket;
        int incase = rr.maxBlacksmith + rr.maxLab + rr.maxMain + rr.maxMarket;
        rr.maxRooms = isMaxRoomRandom ? rr.maxRooms = Random.Range(rr.maxRooms, incase + ifIsRandomRoomHowManyPlus) : rr.maxRooms = incase;
        rr.maxRooms += 1;
        return rr;
    }
}

public enum cameraState
{
    Non,
    Settings,
    Play,
    Credit,
}

[System.Serializable]
public class RandomRooms
{
    public int maxRooms, maxSector, mercyCount;
    public int minLab, minMarket, minBlacksmith, minMain;
    public int maxLab, maxMarket, maxBlacksmith, maxMain;
}

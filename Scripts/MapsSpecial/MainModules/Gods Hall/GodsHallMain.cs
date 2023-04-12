using HelperOfDmr;
using UnityEngine;

public class GodsHallMain : MonoBehaviour, IMapGetSerializer
{
    [SerializeField] private bool _vegvisPuzzleOpen;
    [SerializeField] private bool _vegvisCompleted, _gCompleted;
    [SerializeField] private GameObject _vegvisPortal,_gPortal,_vegvisPuzzle,_gPuzzle;
    [SerializeField] private VegvisirPlatform _platform1, _platform2,_platform3;
    [HideInInspector] public bool isPhilterActive = false;
    public bool VegvisCompleted => _vegvisCompleted;
    public string firstPlatform, secondPlatform, thirdPlatform;
    private bool serialized;

    private string plat1serialized = "", plat2serialized = "", plat3serialized = "";

    public static GodsHallMain instance;

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

    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
        if (serialized) return;
        int i = Random.Range(0, 101);
        //Gora puzzle cancelled for now
        _ = (true) ? _vegvisPuzzleOpen = true : _vegvisPuzzleOpen = false;
        CalculateMe();
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        _vegvisPuzzleOpen = SIC.serialBoolList[0];
        _vegvisCompleted = SIC.serialBoolList[1];
        _gCompleted = SIC.serialBoolList[2];
        firstPlatform = SIC.serialStringList[0];
        secondPlatform = SIC.serialStringList[1];
        thirdPlatform = SIC.serialStringList[2];
        plat1serialized = SIC.serialStringList[3];
        plat2serialized = SIC.serialStringList[4];
        plat3serialized = SIC.serialStringList[5];
        CalculateMe();
    }
    public void serializeMe(int currentIndex)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;

        SerializationInfoClass SIC = UtilitesOfDmr.CreateDefaultSIC(this.gameObject, this);
        /*
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.scriptName = this.GetType().Name;
        SIC.dontDeleteThis = false;
        SIC.generatedBySomething = false;
        */

        SIC.serialBoolList.Add(_vegvisPuzzleOpen);
        SIC.serialBoolList.Add(_vegvisCompleted);
        SIC.serialBoolList.Add(_gCompleted);
        SIC.serialStringList.Add(firstPlatform);
        SIC.serialStringList.Add(secondPlatform);
        SIC.serialStringList.Add(thirdPlatform);
        SIC.serialStringList.Add(_platform1.currentPlatform);
        SIC.serialStringList.Add(_platform2.currentPlatform);
        SIC.serialStringList.Add(_platform3.currentPlatform);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void CalculateMe()
    {
        if (_vegvisPuzzleOpen)
        {
            _vegvisPuzzle.SetActive(true);
            _gPuzzle.SetActive(false);
            if (plat1serialized != "" || plat2serialized != "" || plat3serialized != "")
            {
                _platform1.getInformationFromAnother(plat1serialized);
                _platform2.getInformationFromAnother(plat2serialized);
                _platform3.getInformationFromAnother(plat3serialized);
            }
        }else
        {
            _vegvisPuzzle.SetActive(false);
            _gPuzzle.SetActive(true);
        }
        if (_vegvisCompleted) {_vegvisPortal.SetActive(true);} else {_vegvisPortal.SetActive(false);}
        if (_gCompleted) {_gPortal.SetActive(true);} else {_gPortal.SetActive(false);}
        if (!serialized)
        {
            int randomSwitch = Random.Range(0, 4);
            switch (randomSwitch)
            {
                case 0:
                    firstPlatform = _platform1.Platform1;
                    break;
                case 1:
                    firstPlatform = _platform1.Platform2;
                    break;
                case 2:
                    firstPlatform = _platform1.Platform3;
                    break;
                case 3:
                    firstPlatform = _platform1.Platform4;
                    break;
            }
            randomSwitch = Random.Range(0, 4);
            switch (randomSwitch)
            {
                case 0:
                    secondPlatform = _platform2.Platform1;
                    break;
                case 1:
                    secondPlatform = _platform2.Platform2;
                    break;
                case 2:
                    secondPlatform = _platform2.Platform3;
                    break;
                case 3:
                    secondPlatform = _platform2.Platform4;
                    break;
            }
            randomSwitch = Random.Range(0, 4);
            switch (randomSwitch)
            {
                case 0:
                    thirdPlatform = _platform3.Platform1;
                    break;
                case 1:
                    thirdPlatform = _platform3.Platform2;
                    break;
                case 2:
                    thirdPlatform = _platform3.Platform3;
                    break;
                case 3:
                    thirdPlatform = _platform3.Platform4;
                    break;
            }
        }
    }
    public void openVegvis()
    {
        _vegvisCompleted = true;
        if (_vegvisCompleted) { _vegvisPortal.SetActive(true); } else { _vegvisPortal.SetActive(false); }
    }
    public bool isPhilterOfRealityActive()
    {
        return isPhilterActive;
    }

    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
}

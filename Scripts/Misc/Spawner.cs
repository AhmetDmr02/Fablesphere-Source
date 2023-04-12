using UnityEngine;

public class Spawner : MonoBehaviour,IMapGetSerializer
{
    [SerializeField] private SpawnableUnit[] _spawnableUnit;
    private int EmptyPercentage;
    [SerializeField] private bool _spawnOnStart = true;
    [SerializeField] private bool _destroySpawnerAfterSpawn; 
    [HideInInspector] public bool isSpawned;
    [HideInInspector] public bool isGizmoDrawing;
    
    [ShowWhen("isGizmoDrawing",true)]
    [SerializeField] private bool _showRootPos;
    [SerializeField] private bool serialized;
    [SerializeField] private GameObject createdObj;
    private Vector3 Origin;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        //This void executes when you change variables in editor
        EmptyPercentage = 100;
        if (_spawnableUnit == null) return;
        if (_spawnableUnit.Length <= 0) return;
        foreach (SpawnableUnit SU in _spawnableUnit)
        {
            if (SU.UnitPrefab == null && !SU.emptyItem) return;
            EmptyPercentage -= SU.Percentage;
        }
        if (EmptyPercentage > 0)
        {
            foreach (SpawnableUnit SI in _spawnableUnit)
            {
                if (SI.Percentage > 100)
                {
                    SI.Percentage = 100;
                }
                int PercentageRate = SI.Percentage + EmptyPercentage;
                if (PercentageRate <= 100)
                {
                    SI.Percentage = PercentageRate;
                    break;
                }
                else
                {
                    SI.Percentage = 100;
                }
            }
            //SpawnableItems[0].Percentage += EmptyPercentage;
        }
        else if (EmptyPercentage < 0)
        {
            foreach (SpawnableUnit SI in _spawnableUnit)
            {
                if (SI.Percentage > 100)
                {
                    SI.Percentage = 100;
                }
                int PercentageRate = SI.Percentage + EmptyPercentage;
                if (PercentageRate >= 0)
                {
                    SI.Percentage = PercentageRate;
                    break;
                }
                else
                {
                    SI.Percentage = 0;
                }
            }
        }
        EmptyPercentage = 100;
        foreach (SpawnableUnit SI in _spawnableUnit)
        {
            //For Resetting Empty Percentage
            EmptyPercentage -= SI.Percentage;
        }
    }
    
    private void OnDrawGizmos()
    {
        isGizmoDrawing = false;
        if (_spawnableUnit == null) return;
        if (_spawnableUnit.Length < 1) return;
        if (_spawnableUnit[0].UnitPrefab == null) return;
        if (_spawnableUnit[0].emptyItem) return;
        isGizmoDrawing = true;
        if (_spawnableUnit[0].UnitPrefab.GetComponent<MeshFilter>() != null)
        {
            Gizmos.DrawWireMesh(_spawnableUnit[0].UnitPrefab.GetComponent<MeshFilter>().sharedMesh, this.transform.position, this.transform.rotation, _spawnableUnit[0].UnitPrefab.transform.lossyScale);
        }
        else
        {
            this.transform.localScale = _spawnableUnit[0].UnitPrefab.transform.lossyScale;
            for (int i = 0; i < _spawnableUnit[0].UnitPrefab.transform.childCount; i++)
            {
                if (this._spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null)
                {
                    if (_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh != null)
                    {
                        if (_showRootPos)
                        {
                            if (_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().rootBone != null)
                            {
                                Origin = _spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().rootBone.transform.localPosition;
                            }
                            else
                            {
                                Origin = _spawnableUnit[0].UnitPrefab.transform.GetChild(i).transform.localPosition;
                            }
                        }
                        else
                        {
                            Origin = _spawnableUnit[0].UnitPrefab.transform.GetChild(i).transform.localPosition;
                        }

                        if (_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().rootBone == null)
                        {
                            Gizmos.DrawWireMesh(_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh, this.transform.TransformPoint(Origin), this.transform.rotation, _spawnableUnit[0].UnitPrefab.transform.GetChild(i).lossyScale);
                        }
                        else 
                        {
                            if (_spawnableUnit[0].UnitPrefab.transform.GetChild(i).transform.localEulerAngles.x == 270)
                            {
                                Quaternion fixedRotation = Quaternion.Euler(_spawnableUnit[0].UnitPrefab.transform.GetChild(i).transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
                                Gizmos.DrawWireMesh(_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh, this.transform.TransformPoint(Origin), fixedRotation, _spawnableUnit[0].UnitPrefab.transform.GetChild(i).lossyScale);
                            }
                            else
                            {
                                Gizmos.DrawWireMesh(_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh, this.transform.TransformPoint(Origin), this.transform.rotation, _spawnableUnit[0].UnitPrefab.transform.GetChild(i).lossyScale);
                            }
                        }
                    }
                }
                if (_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<MeshFilter>() != null)
                {
                    if (_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh != null)
                    {
                        Origin = _spawnableUnit[0].UnitPrefab.transform.GetChild(i).transform.localPosition;
                        Gizmos.DrawWireMesh(_spawnableUnit[0].UnitPrefab.transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh, this.transform.TransformPoint(Origin), this.transform.rotation, _spawnableUnit[0].UnitPrefab.transform.GetChild(i).lossyScale);
                    }
                }
            }
        }
    }
    
#endif

    private void Start()
    {
        if (_spawnOnStart && !serialized) SpawnUnits();
    }

    private void Update()
    {
        if (!isSpawned && !_spawnOnStart) return;
    }

    public void SpawnUnits()
    {

        //0 to 100 random number
        int percentNum = Random.Range(0, 101);
        for (int i = 0; i < _spawnableUnit.Length; i++)
        {
            if (i == 0)
            {
                if (_spawnableUnit[i].UnitPrefab == null && !_spawnableUnit[i].emptyItem)
                {
                    Debug.LogError("Unit Prefab Was Null!");
                    break;
                }
                if (percentNum >= 0 && percentNum <= _spawnableUnit[i].Percentage)
                {
                    if (_spawnableUnit[i].emptyItem)
                    {
                        break;
                        return;
                    }
                    GameObject GO = Instantiate(_spawnableUnit[i].UnitPrefab, this.gameObject.transform.position, this.transform.rotation);
                    createdObj = GO;
                    GO.AddComponent<GeneratedObject>();
                    GO.GetComponent<GeneratedObject>()._objInAssets = _spawnableUnit[i].UnitPrefab;
                    isSpawned = true;
                    foreach (IAfterSpawnerMethods _interface in this.gameObject.GetComponents<IAfterSpawnerMethods>())
                    {
                        _interface.editGO(GO);
                    }
                    //if this boolean is true object will destroy himself in 10 sec
                    if (_destroySpawnerAfterSpawn) Destroy(this.gameObject, 10f);

                    break;
                    //Object Created
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (_spawnableUnit[i].UnitPrefab == null && !_spawnableUnit[i].emptyItem)
                {
                    Debug.LogError("Unit Prefab Was Null!");
                    break;
                }
                int MaxItemChance = 0;
                for (int y = 0; y <= i; y++)
                {
                    MaxItemChance += _spawnableUnit[y].Percentage;
                }
                int minItemChance = _spawnableUnit[i - 1].Percentage;
                if (percentNum > minItemChance && percentNum <= MaxItemChance)
                {
                    if (_spawnableUnit[i].emptyItem)
                    {
                        break;
                        return;
                    }
                    GameObject GO = Instantiate(_spawnableUnit[i].UnitPrefab, this.gameObject.transform.position, this.transform.rotation);
                    createdObj = GO;
                    GO.AddComponent<GeneratedObject>();
                    GO.GetComponent<GeneratedObject>()._objInAssets = _spawnableUnit[i].UnitPrefab;
                    isSpawned = true;
                    foreach (IAfterSpawnerMethods _interface in this.gameObject.GetComponents<IAfterSpawnerMethods>())
                    {
                        _interface.editGO(GO);
                    }
                    //if this boolean is true object will destroy himself in 10 sec
                    if (_destroySpawnerAfterSpawn) Destroy(this.gameObject, 10f);

                    break;
                    //Object Created
                }
            }
        }
        serializeMe();
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        //Deserialize
        serialized = true;
        if (createdObj != null && SIC.serialBoolList[0])
        {
            Destroy(createdObj);
            Destroy(this.gameObject);
        }
        if (SIC.serialBoolList[0])
        {
            Destroy(this.gameObject);
        }
    }
    public void serializeMe()
    {
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.scriptName = this.gameObject.GetComponent<MonoBehaviour>().GetType().Name;
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.dontDeleteThis = true;
        bool Created__ = true;
        SIC.serialBoolList.Add(Created__);
        if (ProceduralModuleGenerator.instance.isPMGContainsSIC(SIC, ProceduralModuleGenerator.instance.currentIndex, this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w))
        {
            ProceduralModuleGenerator.instance.deleteSICbyIndexAndInstanceID(this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w, ProceduralModuleGenerator.instance.currentIndex);
        }
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
}


[System.Serializable]
public class SpawnableUnit
{
    [ShowWhen("emptyItem",false)]
    public GameObject UnitPrefab;
    public bool emptyItem;
    public int Percentage;
}

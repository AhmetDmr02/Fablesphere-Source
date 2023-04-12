using UnityEngine;
public class DynamicItemSpawner : MonoBehaviour,IMapGetSerializer
{
    private int EmptyPercentage;
    private GameObject _mainObject;
    [SerializeField] private SpawnableItem[] SpawnableItems;
    [SerializeField] private bool IsSpawned;
    [HideInInspector] public GameObject SpawnedItem;
    private bool serialized;


    #region Editor Side
    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (SpawnableItems == null) return;
        if (SpawnableItems.Length <= 0) return;
        EmptyPercentage = 100;
        foreach (SpawnableItem SI in SpawnableItems)
        {
            if (SI.Item != null)
            {
                #region Basic Stuff
                if (SI.Min <= 0)
                {
                    SI.Min = 1;
                }
                if (SI.Max <= 0)
                {
                    SI.Max = 1;
                }
                if (SI.Min >= SI.Max && SI.Max != 0 && SI.Max != 1)
                {
                    SI.Min = SI.Max - 1;
                }
                if (SI.Max > SI.Item.MaxStack)
                {
                    SI.Max = SI.Item.MaxStack;
                }
                #endregion
                EmptyPercentage -= SI.Percentage;
            }
        }
        if (EmptyPercentage > 0)
        {
            foreach (SpawnableItem SI in SpawnableItems)
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
            foreach (SpawnableItem SI in SpawnableItems)
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
                }else
                {
                    SI.Percentage = 0;
                }
            }
        }
        EmptyPercentage = 100;
        foreach (SpawnableItem SI in SpawnableItems)
        {
            //For Resetting Empty Percentage
            EmptyPercentage -= SI.Percentage;
        }
    }
    #endif
    #endregion
    private void Start()
    {
        if (!IsSpawned && SpawnableItems.Length > 0 && !serialized) SpawnItem(Random.Range(0, 101));
    }
    private void SpawnItem(int PercentNum)
    {
        for (int i = 0; i < SpawnableItems.Length;i++)
        {
            if (i == 0)
            {
                if (!SpawnableItems[i].EmptyItem && SpawnableItems[i].Item == null) break;
                _mainObject = Resources.Load<GameObject>("PickUpSpecial");
                if (PercentNum >= 0 && PercentNum <= SpawnableItems[i].Percentage)
                {
                    if (SpawnableItems[i].EmptyItem)
                    {
                        Debug.Log("Empty Item");
                        IsSpawned = true;
                        SerializeMe();
                        break;
                    }
                    else
                    {
                        //Spawn
                        GameObject GO = Instantiate(_mainObject, this.gameObject.transform.position, this.gameObject.transform.rotation);
                        if (SpawnableItems[i].PrefabMesh != null && SpawnableItems[i].DoesHaveCustomMesh)
                        {
                            GO.transform.GetChild(0).gameObject.SetActive(false);
                            GameObject G = Instantiate(SpawnableItems[i].PrefabMesh, GO.transform.position,GO.transform.rotation);
                            GO.GetComponent<PickableItem>().customprefab = SpawnableItems[i].PrefabMesh;
                            GO.GetComponent<PickableItem>().warmprefab = _mainObject;
                            Debug.Log("GO +" + GO.name);
                            G.transform.parent = GO.transform;
                            G.gameObject.transform.position = GO.transform.position;
                        }
                        GO.AddComponent<GeneratedObject>();
                        GO.GetComponent<GeneratedObject>()._objInAssets = _mainObject;
                        PickableItem PI = GO.GetComponent<PickableItem>();
                        PI.Id = SpawnableItems[i].Item.Id;
                        PI.Count = Random.Range(SpawnableItems[i].Min, SpawnableItems[i].Max + 1);
                        PI.Icon = SpawnableItems[i].Item.Photo;
                        PI.ItemName = SpawnableItems[i].Item.ItemName;
                        IsSpawned = true;
                        SpawnedItem = GO;
                        break;
                    }
                }
            }else
            {
                if (SpawnableItems[i].EmptyItem)
                {
                    Debug.Log("Empty Item");
                    IsSpawned = true;
                    SerializeMe();
                    break;
                }
                else
                {
                    //int MaxItemChance = SpawnableItems[i - 1].Percentage + SpawnableItems[i].Percentage;
                    int MaxItemChance = 0;
                    for (int x = 0; x <= i; x++)
                    {
                        MaxItemChance += SpawnableItems[x].Percentage;
                    }
                    int MinItemChance = SpawnableItems[i - 1].Percentage;
                    if (PercentNum > MinItemChance && PercentNum <= MaxItemChance)
                    {
                        //Spawn
                        GameObject GO = Instantiate(_mainObject, this.gameObject.transform.position, this.gameObject.transform.rotation);
                        if (SpawnableItems[i].PrefabMesh != null && SpawnableItems[i].DoesHaveCustomMesh)
                        {
                            GO.transform.GetChild(0).gameObject.SetActive(false);
                            GameObject G = Instantiate(SpawnableItems[i].PrefabMesh, GO.transform.position, GO.transform.rotation);
                            GO.GetComponent<PickableItem>().customprefab = SpawnableItems[i].PrefabMesh;
                            GO.GetComponent<PickableItem>().warmprefab = _mainObject;
                            Debug.Log("GO +" + GO.name);
                            G.transform.parent = GO.transform;
                            G.gameObject.transform.position = GO.transform.position;
                        }
                        GO.AddComponent<GeneratedObject>();
                        GO.GetComponent<GeneratedObject>()._objInAssets = _mainObject;
                        PickableItem PI = GO.GetComponent<PickableItem>();
                        PI.Id = SpawnableItems[i].Item.Id;
                        PI.Count = Random.Range(SpawnableItems[i].Min, SpawnableItems[i].Max + 1);
                        PI.Icon = SpawnableItems[i].Item.Photo;
                        PI.ItemName = SpawnableItems[i].Item.ItemName;
                        IsSpawned = true;
                        SpawnedItem = GO;
                        break;
                    }
                }
            }
        }
        SerializeMe();
    }

    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        if (SpawnedItem != null && SIC.serialBoolList[0])
        {
            Destroy(SpawnedItem);
            Destroy(this.gameObject);
        }
    }
    public void SerializeMe()
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
public class SpawnableItem 
{
    public bool EmptyItem; //Don't Spawn Anything
    [ShowWhen("EmptyItem",false)]
    public bool DoesHaveCustomMesh;
    [ShowWhen("EmptyItem", false)]
    public DataItem Item;
    [ShowWhen("DoesHaveCustomMesh", true)]
    public GameObject PrefabMesh;
    public int Min, Max;
    public int Percentage;
}

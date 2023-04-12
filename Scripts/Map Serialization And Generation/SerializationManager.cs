using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationManager : MonoBehaviour
{
    public static SerializationManager instance;
    public bool finished;
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
    public void startSerializing(List<SerializationInfoClass> SICList)
    {
        Debug.Log("Started Serializing");
        finished = false;
        if (SICList == null)
        {
            finished = true;
            Debug.Log("Serialization Failed");
            return;
        }
        for(int x = 0; x < SICList.Count; x++)
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            for (int i = 0; i < allObjects.Length; i++)
            {
                if (SICList[x].generatedBySomething)
                {
                    if (SICList[x].serialAssetGameObject[0] == null) { Debug.Log("GameObject Null"); return; }
                    GameObject GO = Instantiate(SICList[x].serialAssetGameObject[0], SICList[x].serialVec3Single, SICList[x].serialQuaternionSingle);
                    if (GO.GetComponent<IMapGetSerializer>() == null) { Debug.Log("Interface Null On Generated Object"); return; }
                    GO.AddComponent<GeneratedObject>();
                    GO.GetComponent<GeneratedObject>()._objInAssets = SICList[x].serialAssetGameObject[0];
                    GO.GetComponent<IMapGetSerializer>().GetMapInfoClass(SICList[x]);
                    break;
                }
                else
                {
                    if (SICList[x].instanceID == allObjects[i].name + allObjects[i].transform.position.x + allObjects[i].transform.position.y + allObjects[i].transform.position.z + allObjects[i].transform.rotation.x + allObjects[i].transform.rotation.y + allObjects[i].transform.rotation.z + allObjects[i].transform.rotation.w)
                    {
                        if (allObjects[i].GetComponents<IMapGetSerializer>().Length > 1)
                        {
                            IMapGetSerializer[] IMGS = allObjects[i].GetComponents<IMapGetSerializer>();
                            foreach (IMapGetSerializer IMG in IMGS)
                            {
                                if (IMG.GetType().Name == SICList[x].scriptName)
                                {
                                    if (SICList[x] != null)
                                    {
                                        IMG.GetMapInfoClass(SICList[x]);
                                    }
                                }
                            }
                        }
                        else if (allObjects[i].GetComponents<IMapGetSerializer>().Length == 1)
                        {
                            if (SICList[x] != null)
                            {
                                allObjects[i].GetComponent<IMapGetSerializer>().GetMapInfoClass(SICList[x]);
                            }
                        }
                    }
                }
            }
        }
        finished = true;
    }
}

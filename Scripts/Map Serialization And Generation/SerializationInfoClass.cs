using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializationInfoClass
{
    public string instanceID;
    public string scriptName;
    public bool dontDeleteThis = false;
    public bool generatedBySomething = false;
    public List<int> serialIntList = new List<int>();
    public List<float> serialFloatList = new List<float>();
    public List<string> serialStringList = new List<string>();
    public List<bool> serialBoolList = new List<bool>();
    public List<Sprite> serialSpriteList = new List<Sprite>();
    public List<GameObject> serialAssetGameObject = new List<GameObject>();
    public List<Vector3> serialVec3List = new List<Vector3>();
    public Vector3 serialVec3Single;
    public Quaternion serialQuaternionSingle;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegvisirPlatform : MonoBehaviour,IInteractable,IMapGetSerializer
{
    public string Platform1, Platform2, Platform3, Platform4;
    public string currentPlatform;
    [SerializeField] private AudioSource rockSliding;
    [SerializeField] private float desiredRotation,lerpSpeed;
    [SerializeField] private Transform rockPos;
    private bool readyForTurn;
    private bool seralized;

    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
        if (!seralized)
        {
            currentPlatform = Platform1;
            desiredRotation = 0;
        }
    }
    private void Update()
    {
        if (Mathf.RoundToInt(rockPos.localEulerAngles.y) == (int)desiredRotation)
            readyForTurn = true;
        rockPos.localEulerAngles = Vector3.Lerp(rockPos.localEulerAngles, new Vector3(0, desiredRotation, 0), lerpSpeed * Time.deltaTime);
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        seralized = true;
        currentPlatform = SIC.serialStringList[0];
        if (SIC.serialStringList[0] == Platform1)
        {
            desiredRotation = 0;
        }
        if (SIC.serialStringList[0] == Platform2)
        {
            desiredRotation = 90;
        }
        if (SIC.serialStringList[0] == Platform3)
        {
            desiredRotation = 180;
        }
        if (SIC.serialStringList[0] == Platform4)
        {
            desiredRotation = 270;
        }
    }
    public void getInformationFromAnother(string currentStr)
    {
        seralized = true;
        currentPlatform = currentStr;
        if (currentStr == Platform1)
        {
            desiredRotation = 0;
        }
        if (currentStr == Platform2)
        {
            desiredRotation = 90;
        }
        if (currentStr  == Platform3)
        {
            desiredRotation = 180;
        }
        if (currentStr == Platform4)
        {
            desiredRotation = 270;
        }
    }
    public string GetLookAtDescription()
    {
        if (readyForTurn)
        {
            return "[F] To Rotate Block";
        }
        else
        {
            return "";
        }
    }
    public Color GetTextColor()
    {
        return Color.white;
    }
    public void OnInteract()
    {
        if (!readyForTurn) return;
        rockSliding.pitch = Random.Range(0.800f, 1f);
        rockSliding.Play();
        readyForTurn = false;
        if (currentPlatform == Platform1)
        {
            currentPlatform = Platform2;
            desiredRotation = 90;
        }
        else if (currentPlatform == Platform2)
        {
            currentPlatform = Platform3;
            desiredRotation = 180;
        }
        else if (currentPlatform == Platform3)
        {
            currentPlatform = Platform4;
            desiredRotation = 270;
        }
        else if (currentPlatform == Platform4)
        {
            currentPlatform = Platform1;
            desiredRotation = 0;
        }
    }
    public void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        SerializationInfoClass SIC = new SerializationInfoClass();
        SIC.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
        SIC.scriptName = this.GetType().Name;
        SIC.dontDeleteThis = false;
        SIC.generatedBySomething = false;
        SIC.serialStringList.Add(currentPlatform);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
}

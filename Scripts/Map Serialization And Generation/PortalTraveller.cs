using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour,IMapGetSerializer
{
    [ShowWhen("NextPortal",false)]
    public bool BackPortal;
    [ShowWhen("BackPortal", false)]
    public bool NextPortal = true;
    [ShowWhen("NextPortal",false)]
    public int LoadIndex = -1;
    private bool serialized;

    private bool passedThroughMe = false;
    public bool preventSpamming = false;
    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += getCurrentIndex;
        if (BackPortal && !serialized)
        {
            LoadIndex = ProceduralModuleGenerator.instance.lastIndex;
        }
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= getCurrentIndex;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !preventSpamming)
        {
            preventSpamming = true;
            if (NextPortal)
            {
                this.passedThroughMe = true;
                LoadingManager.instance.reverseComing = false;
                EffectManager.instance.CreateBlackoutEffect(0.5f, 2, false, 1);
                PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = true;
                PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = PostProcessingManager.instance.gameObject;
                AudioListener.volume = 0;
                StartCoroutine(callNewGenerate());
            }
            else
            {
                if (ProceduralModuleGenerator.instance.currentIndex > LoadIndex)
                {
                    LoadingManager.instance.reverseComing = true;
                }
                else
                {
                    LoadingManager.instance.reverseComing = false;
                }
                this.passedThroughMe = true;
                EffectManager.instance.CreateBlackoutEffect(0.5f, 2, false, 1);
                PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().SomeoneActive = true;
                PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>().ActiveObject = PostProcessingManager.instance.gameObject;
                AudioListener.volume = 0;
                StartCoroutine(callLoadIndex());
            }
        }
    }
    IEnumerator callNewGenerate()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        ProceduralModuleGenerator.instance.StartCoroutine(ProceduralModuleGenerator.instance.delayedGenerateNewModuleStatement());
        StopCoroutine(callNewGenerate());
    }
    IEnumerator callLoadIndex()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        ProceduralModuleGenerator.instance.StartCoroutine(ProceduralModuleGenerator.instance.delayedLoadModuleFromIndex(LoadIndex,3));
        StopCoroutine(callLoadIndex());
    }
    public void serializeThisScript(SerializationInfoClass SIC)
    {
        ProceduralModuleGenerator.instance.GetComponent<IMapSetSerializer>().saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        //Serialization Manager Will Call This
        serialized = true;
        BackPortal = SIC.serialBoolList[0];
        NextPortal = SIC.serialBoolList[1];
        LoadIndex = SIC.serialIntList[0];
    }
    public void getCurrentIndex(int index)
    {
        if (!NextPortal)
        {
            bool BackkPortal = true;
            bool NexttPortal = false;
            int LoadIndexx = LoadIndex;
            SerializationInfoClass mySerializationInfoClass = new SerializationInfoClass();
            mySerializationInfoClass.dontDeleteThis = false;
            mySerializationInfoClass.serialBoolList.Add(BackkPortal);
            mySerializationInfoClass.serialBoolList.Add(NexttPortal);
            mySerializationInfoClass.serialIntList.Add(LoadIndexx);
            mySerializationInfoClass.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
            MonoBehaviour mb = this;
            mySerializationInfoClass.scriptName = mb.GetType().Name;
            serializeThisScript(mySerializationInfoClass);
        }
        if (passedThroughMe && NextPortal)
        {
            bool BackkPortal = true;
            bool NexttPortal = false;
            int LoadIndex = index;
            SerializationInfoClass mySerializationInfoClass = new SerializationInfoClass();
            mySerializationInfoClass.dontDeleteThis = false;
            mySerializationInfoClass.serialBoolList.Add(BackkPortal);
            mySerializationInfoClass.serialBoolList.Add(NexttPortal);
            mySerializationInfoClass.serialIntList.Add(index);
            mySerializationInfoClass.instanceID = this.gameObject.name + this.transform.position.x + this.transform.position.y + this.transform.position.z + this.transform.rotation.x + this.transform.rotation.y + this.transform.rotation.z + this.transform.rotation.w;
            MonoBehaviour mb = this;
            mySerializationInfoClass.scriptName = mb.GetType().Name;
            serializeThisScript(mySerializationInfoClass);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinesweeperMine : MonoBehaviour, IMapGetSerializer,IInteractable,IInteractableV2
{
    public bool mineOpened, isThisMine,isThisFlagged;
    public int mineCount;
    public GameObject openedObject,unopenedObject;
    public GameObject flagSprite;
    public GameObject breakEffect, soundEffect;
    public TextMeshProUGUI mineCount_Text;
    private bool serialized;
    [SerializeField] private MinesweeperManager minesweeperManager;
    private void Start()
    {
        if (!serialized)
        {
            unopenedObject.SetActive(true);
            openedObject.SetActive(false);
            flagSprite.SetActive(false);
        }
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
    }
    public void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        SerializationInfoClass SIC = HelperOfDmr.UtilitesOfDmr.CreateDefaultSIC(this.gameObject, this);
        SIC.serialBoolList.Add(mineOpened);
        SIC.serialBoolList.Add(isThisMine);
        SIC.serialBoolList.Add(isThisFlagged);
        SIC.serialIntList.Add(mineCount);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        serialized = true;
        mineOpened = SIC.serialBoolList[0];
        isThisMine = SIC.serialBoolList[1];
        isThisFlagged = SIC.serialBoolList[2];
        mineCount = SIC.serialIntList[0];
        recalculateMe();
    }
    public void OnInteract()
    {
        if (mineOpened || isThisFlagged) return;
        if (isThisMine)
        {
            minesweeperManager.openMine(this);
        }
        else
        {
            minesweeperManager.revealArea(this);
        }
    }
    public void OnInteractV2()
    {
        if (mineOpened) return;
        isThisFlagged = !isThisFlagged;
        if (isThisFlagged)
        {
            flagSprite.SetActive(true);
        }else
        {
            flagSprite.SetActive(false);
        }
    }
    public string GetLookAtDescription()
    {
        if (!mineOpened && !isThisFlagged)
        {
            return "[F] To Reveal Minefield \n" +
                "[E] To Flag";
        }
        if (isThisFlagged)
        {
            return "[E] To Remove Flag";
        }
        return "";
    }
    public Color GetTextColor()
    {
        return Color.white;
    }
    public void recalculateMe()
    {
        if (mineOpened)
        {
            unopenedObject.SetActive(false);
            openedObject.SetActive(true);
            mineCount_Text.text = mineCount.ToString();
        }
        if (isThisFlagged)
        {
            flagSprite.SetActive(true);
        }
    }
    public void openVeil(int mineC)
    {
        mineOpened = true;
        soundEffect.GetComponent<AudioSource>().Play();
        breakEffect.GetComponent<ParticleSystem>().Play();
        unopenedObject.SetActive(false);
        openedObject.SetActive(true);
        mineCount = mineC;
        mineCount_Text.text = mineCount.ToString();
    }
    public void removeSerialization()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
}

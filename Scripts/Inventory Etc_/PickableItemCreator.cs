using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperOfDmr;

public class PickableItemCreator : MonoBehaviour
{
    public static PickableItemCreator instance;
    private GameObject PickableItemInstance; //Load From Resources
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Debug.Log("Found PickableItemCreator Clone Destroying Object...");
            Destroy(this.gameObject);
        }
    }
    public void CreatePickUpAbleItem(int ItemId,int ItemCount,Transform SpawnLoc,float PushItemPower)
    {
        GameObject PickableItemInstance = Resources.Load<GameObject>("PickUpItemInstance");
        Quaternion RandomRotation = RotationHelper.GiveRandomRotation(false);
        GameObject GO = Instantiate(PickableItemInstance, SpawnLoc.position, RandomRotation);
        GO.AddComponent<GeneratedObject>();
        GO.GetComponent<GeneratedObject>()._objInAssets = PickableItemInstance;
        GO.GetComponent<Rigidbody>().AddForce(transform.forward * PushItemPower * Time.deltaTime);
        Inventory inv = gameObject.GetComponent<Inventory>();
        for (int i = 0; i < inv.DataItems.Length; i++)
        {
            if (inv.DataItems[i].Id == ItemId)
            {
                PickableItem PI = GO.GetComponent<PickableItem>();
                PI.Id = inv.DataItems[i].Id;
                PI.Count = ItemCount;
                PI.Icon = inv.DataItems[i].Photo;
                PI.ItemName = inv.DataItems[i].ItemName;
                break;
            }
        }
    }
}
